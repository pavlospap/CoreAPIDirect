using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreApiDirect.Base;
using CoreApiDirect.Controllers.Filters;
using CoreApiDirect.Controllers.Results;
using CoreApiDirect.Controllers.Shaping;
using CoreApiDirect.Entities;
using CoreApiDirect.Flow;
using CoreApiDirect.Mapping;
using CoreApiDirect.Options;
using CoreApiDirect.Query;
using CoreApiDirect.Repositories;
using CoreApiDirect.Resources;
using CoreApiDirect.Response;
using CoreApiDirect.Routing;
using CoreApiDirect.Url;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CoreApiDirect.Controllers
{
    /// <summary>
    /// The base class for every API controller.
    /// </summary>
    /// <typeparam name="TKey">The entity ID type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TOutDto">The DTO type for the HTTP GET requests.</typeparam>
    /// <typeparam name="TInDto">The DTO type for the HTTP POST, PUT and PATCH requests.</typeparam>
    /// <typeparam name="TFlow">The flow type.</typeparam>
    public abstract class ControllerApiBase<TKey, TEntity, TOutDto, TInDto, TFlow> : ControllerBase
        where TEntity : class, IKey<TKey>
        where TOutDto : IKey<TKey>
        where TInDto : class
        where TFlow : class, IFlowDelete<TEntity>
    {
        private IResponseBuilder _responseBuilder;

        /// <summary>
        /// Gets the CoreApiDirect.Response.IResponseBuilder for the current request.
        /// </summary>
        protected IResponseBuilder ResponseBuilder
        {
            get
            {
                return _responseBuilder ?? (_responseBuilder = HttpContext?.RequestServices?.GetRequiredService<IResponseBuilder>());
            }
        }

        private IRepository<TEntity, TKey> _repository;

        /// <summary>
        /// Gets the CoreApiDirect.Repositories.IRepository for the current request.
        /// </summary>
        protected IRepository<TEntity, TKey> Repository
        {
            get
            {
                return _repository ?? (_repository = HttpContext?.RequestServices?.GetRequiredService<IRepository<TEntity, TKey>>());
            }
        }

        private bool _checkedForApiMapper;
        private IApiMapper _apiMapper;

        /// <summary>
        /// Gets the CoreApiDirect.Controllers.IApiMapper.
        /// </summary>
        protected IApiMapper ApiMapper
        {
            get
            {
                if (!_checkedForApiMapper)
                {
                    _apiMapper = HttpContext?.RequestServices?.GetService<IApiMapper>();
                    _checkedForApiMapper = true;
                }

                return _apiMapper ?? (_apiMapper = HttpContext?.RequestServices?.GetRequiredService<IEntityMapper<TEntity>>());
            }
        }

        private CoreOptions _options;

        /// <summary>
        /// Gets the CoreApiDirect.Options.CoreOptions.
        /// </summary>
        protected CoreOptions Options
        {
            get
            {
                return _options ?? (_options = HttpContext?.RequestServices?.GetRequiredService<IOptions<CoreOptions>>().Value);
            }
        }

        private TFlow _flow;
        internal TFlow Flow
        {
            get
            {
                return _flow ?? (_flow = HttpContext?.RequestServices?.GetRequiredService<TFlow>());
            }
        }

        private IRouteValidator _routeValidator;
        private IRouteValidator RouteValidator
        {
            get
            {
                return _routeValidator ?? (_routeValidator = HttpContext?.RequestServices?.GetRequiredService<IRouteValidator>());
            }
        }

        private IRouteFilterBuilder _routeFilterBuilder;
        private IRouteFilterBuilder RouteFilterBuilder
        {
            get
            {
                return _routeFilterBuilder ?? (_routeFilterBuilder = HttpContext?.RequestServices?.GetRequiredService<IRouteFilterBuilder>());
            }
        }

        private IQueryBuilder<TEntity> _queryBuilder;
        private IQueryBuilder<TEntity> QueryBuilder
        {
            get
            {
                return _queryBuilder ?? (_queryBuilder = HttpContext?.RequestServices?.GetRequiredService<IQueryBuilder<TEntity>>());
            }
        }

        private ILogger<ControllerBase> _logger;
        private ILogger<ControllerBase> Logger
        {
            get
            {
                return _logger ?? (_logger = (ILogger<ControllerBase>)HttpContext?.RequestServices?.GetRequiredService(typeof(ILogger<>).MakeGenericType(GetType())));
            }
        }

        private bool _checkedForEntityLocalizer;
        private IEntityLocalizer _entityLocalizer;
        private IEntityLocalizer EntityLocalizer
        {
            get
            {
                if (!_checkedForEntityLocalizer)
                {
                    _entityLocalizer = HttpContext?.RequestServices?.GetService<IEntityLocalizer>();
                    _checkedForEntityLocalizer = true;
                }

                return _entityLocalizer;
            }
        }

        private IPagingHeaderBuilder _pagingHeaderBuilder;
        private IPagingHeaderBuilder PagingHeaderBuilder
        {
            get
            {
                return _pagingHeaderBuilder ?? (_pagingHeaderBuilder = HttpContext?.RequestServices?.GetRequiredService<IPagingHeaderBuilder>());
            }
        }

        private IShaper _shaper;
        private IShaper Shaper
        {
            get
            {
                return _shaper ?? (_shaper = HttpContext?.RequestServices?.GetRequiredService<IShaper>());
            }
        }

        private IModelStateResolver _modelStateResolver;
        private IModelStateResolver ModelStateResolver
        {
            get
            {
                return _modelStateResolver ?? (_modelStateResolver = HttpContext?.RequestServices?.GetRequiredService<IModelStateResolver>());
            }
        }

        /// <summary>
        /// An action that supports the HTTP GET method.
        /// </summary>
        /// <param name="queryString">Represents the URL query string.</param>
        /// <returns>The result of the action.</returns>
        [HttpGet]
        [ValidateQueryString]
        public virtual async Task<IActionResult> Get(QueryString queryString)
        {
            var query = Repository.Query.AsNoTracking();
            query = ApplyRouteParams(query);
            query = QueryBuilder.Build(query, queryString);

            var entityList = await PagedList<TEntity>.Create(query, queryString);

            if (!entityList.Any())
            {
                await ValidateRoute(queryString);
                if (ResponseBuilder.HasErrors())
                {
                    return NotFound(ResponseBuilder.Build());
                }
            }

            Response.Headers.Add("X-Paging", PagingHeaderBuilder.Build(this, queryString, entityList));

            var data = ApiMapper.Map<IEnumerable<TOutDto>>(entityList).Select(p => Shaper.Shape(p, queryString));
            LogData(data);

            return Ok(ResponseBuilder
                .AddData(data)
                .Build());
        }

        /// <summary>
        /// An action that supports the HTTP GET method.
        /// </summary>
        /// <param name="id">The entity ID specified in the route.</param>
        /// <param name="queryString">Represents the URL query string.</param>
        /// <returns>The result of the action.</returns>
        [HttpGet("{id}")]
        [ValidateQueryString]
        public virtual async Task<IActionResult> GetById(TKey id, QueryString queryString)
        {
            var query = Repository.Query.AsNoTracking().Where(p => p.Id.Equals(id));
            query = ApplyRouteParams(query);
            query = QueryBuilder.Build(query, queryString);

            var entity = await query.FirstOrDefaultAsync();

            if (entity == null)
            {
                return await BuildResponseForNotFoundEntity(id, queryString);
            }

            var data = Shaper.Shape(ApiMapper.Map<TOutDto>(entity), queryString);
            LogData(data);

            return Ok(ResponseBuilder
                .AddData(data)
                .Build());
        }

        /// <summary>
        /// An action that supports the HTTP GET method.
        /// </summary>
        /// <param name="ids">A typed list of entity IDs specified in the route.</param>
        /// <param name="queryString">Represents the URL query string.</param>
        /// <returns>The result of the action.</returns>
        [HttpGet]
        [BatchRoute("({ids})")]
        [ValidateQueryString]
        public virtual async Task<IActionResult> GetBatch(KeyList<TKey> ids, QueryString queryString)
        {
            var query = Repository.Query.AsNoTracking().Where(p => ids.Contains(p.Id));
            query = ApplyRouteParams(query);
            query = QueryBuilder.Build(query, queryString);

            var entityList = await query.ToListAsync();

            if (ids.Count != entityList.Count)
            {
                var missingIds = ids.Except(entityList.Select(p => p.Id));
                return await BuildResponseForNotFoundEntity(missingIds, queryString);
            }

            var data = ApiMapper.Map<IEnumerable<TOutDto>>(entityList).Select(p => Shaper.Shape(p, queryString));
            LogData(data);

            return Ok(ResponseBuilder
                .AddData(data)
                .Build());
        }

        /// <summary>
        /// An action that supports the HTTP POST method.
        /// </summary>
        /// <param name="dto">The DTO specified in the request body.</param>
        /// <returns>The result of the action.</returns>
        [HttpPost]
        [ValidateDto]
        public virtual async Task<IActionResult> Post([FromBody] TInDto dto)
        {
            LogData(dto);

            await ValidateRoute(forceValidation: true);
            if (ResponseBuilder.HasErrors())
            {
                return NotFound(ResponseBuilder.Build());
            }

            var entity = ApiMapper.Map<TEntity>(dto);

            Repository.Add(entity);

            var result = await PostFlow(dto, entity, Save);
            if (result != null)
            {
                return result;
            }

            var data = ApiMapper.Map<TOutDto>(entity);
            LogData(data);

            return CreatedAtAction(nameof(Get), new { id = entity.Id }, ResponseBuilder
                .AddData(data)
                .Build());
        }

        /// <summary>
        /// An action that supports the HTTP POST method.
        /// </summary>
        /// <param name="dtoList">An enumerable of DTOs specified in the request body.</param>
        /// <returns>The result of the action.</returns>
        [HttpPost]
        [BatchRoute]
        [ValidateDtoList]
        public virtual async Task<IActionResult> PostBatch([FromBody] IEnumerable<TInDto> dtoList)
        {
            LogData(dtoList);

            await ValidateRoute(forceValidation: true);
            if (ResponseBuilder.HasErrors())
            {
                return NotFound(ResponseBuilder.Build());
            }

            var entityList = ApiMapper.Map<IEnumerable<TEntity>>(dtoList);

            Repository.AddRange(entityList);

            var result = await PostFlow(dtoList, entityList, Save);
            if (result != null)
            {
                return result;
            }

            var outDtoList = ApiMapper.Map<IEnumerable<TOutDto>>(entityList);
            var ids = string.Join(',', outDtoList.Select(p => p.Id));

            LogData(outDtoList);

            return CreatedAtAction(nameof(GetBatch), new { ids }, ResponseBuilder
                .AddData(outDtoList)
                .Build());
        }

        /// <summary>
        /// An action that supports the HTTP PUT method.
        /// </summary>
        /// <param name="id">The entity ID specified in the route.</param>
        /// <param name="dto">The DTO specified in the request body.</param>
        /// <param name="queryString">Represents the URL query string.</param>
        /// <returns>The result of the action.</returns>
        [HttpPut("{id}")]
        [ValidateDto]
        [ValidateQueryString]
        public virtual async Task<IActionResult> Put(TKey id, [FromBody] TInDto dto, QueryString queryString)
        {
            LogData(dto);

            var query = Repository.Query.Where(p => p.Id.Equals(id));
            query = ApplyRouteParams(query);

            var entity = await query.FirstOrDefaultAsync();

            if (entity == null)
            {
                return await BuildResponseForNotFoundEntity(id, queryString);
            }

            ApiMapper.Map(dto, entity);

            var result = await UpdateFlow(dto, entity, Save);
            if (result != null)
            {
                return result;
            }

            return NoContent();
        }

        /// <summary>
        /// An action that supports the HTTP PUT method.
        /// </summary>
        /// <param name="ids">A typed list of entity IDs specified in the route.</param>
        /// <param name="dtoList">An enumerable of DTOs specified in the request body.</param>
        /// <param name="queryString">Represents the URL query string.</param>
        /// <returns>The result of the action.</returns>
        [HttpPut]
        [BatchRoute("({ids})")]
        [ValidateDtoList]
        [ValidateQueryString]
        public virtual async Task<IActionResult> PutBatch(KeyList<TKey> ids, [FromBody] IEnumerable<TInDto> dtoList, QueryString queryString)
        {
            LogData(dtoList);

            if (ids.Count() != dtoList.Count())
            {
                return BadRequest(ResponseBuilder.AddError(ApiResources.IdAndDtoNumNotEqual).Build());
            }

            var query = Repository.Query.Where(p => ids.Contains(p.Id));
            query = ApplyRouteParams(query);

            var entityList = await query.ToListAsync();

            if (ids.Count() != entityList.Count())
            {
                var missingIds = ids.Except(entityList.Select(p => p.Id));
                return await BuildResponseForNotFoundEntity(missingIds, queryString);
            }

            var sortedEntityList = new List<TEntity>();
            int cnt = 0;

            foreach (var dto in dtoList)
            {
                var entity = entityList.Find(p => p.Id.Equals(ids[cnt]));
                ApiMapper.Map(dto, entity);
                sortedEntityList.Add(entity);
                cnt++;
            }

            var result = await UpdateFlow(dtoList, sortedEntityList, Save);
            if (result != null)
            {
                return result;
            }

            return NoContent();
        }

        /// <summary>
        /// An action that supports the HTTP PATCH method.
        /// </summary>
        /// <param name="id">The entity ID specified in the route.</param>
        /// <param name="patchDto">The JSON patch specified in the request body.</param>
        /// <param name="queryString">Represents the URL query string.</param>
        /// <returns>The result of the action.</returns>
        [HttpPatch("{id}")]
        [ValidateQueryString]
        public virtual async Task<IActionResult> Patch(TKey id, [FromBody] JsonPatchDocument<TInDto> patchDto, QueryString queryString)
        {
            LogData(patchDto);

            if (patchDto == null)
            {
                return new ApiUnprocessableEntityResult(ResponseBuilder.AddError(ApiResources.PatchDataMissing).Build());
            }

            var query = Repository.Query.Where(p => p.Id.Equals(id));
            query = ApplyRouteParams(query);

            var entity = await query.FirstOrDefaultAsync();

            if (entity == null)
            {
                return await BuildResponseForNotFoundEntity(id, queryString);
            }

            var dto = ApiMapper.Map<TInDto>(entity);

            patchDto.ApplyTo(dto, ModelState);
            TryValidateModel(dto);

            if (!ModelState.IsValid)
            {
                ModelStateResolver.GetModelErrors(ModelState).ForEach(error => ResponseBuilder.AddError(error.Message, error.Field));
                return new ApiUnprocessableEntityResult(ResponseBuilder.Build());
            }

            ApiMapper.Map(dto, entity);

            var result = await UpdateFlow(dto, entity, Save);
            if (result != null)
            {
                return result;
            }

            return NoContent();
        }

        /// <summary>
        /// An action that supports the HTTP DELETE method.
        /// </summary>
        /// <param name="id">The entity ID specified in the route.</param>
        /// <param name="queryString">Represents the URL query string.</param>
        /// <returns>The result of the action.</returns>
        [HttpDelete("{id}")]
        [ValidateQueryString]
        public virtual async Task<IActionResult> Delete(TKey id, QueryString queryString)
        {
            var query = Repository.Query.Where(p => p.Id.Equals(id));
            query = ApplyRouteParams(query);

            var entity = await query.FirstOrDefaultAsync();

            if (entity == null)
            {
                return await BuildResponseForNotFoundEntity(id, queryString);
            }

            Repository.Remove(entity);

            var flowResult = await Flow.Delete(entity, Save);
            if (flowResult != null)
            {
                return flowResult;
            }

            return NoContent();
        }

        /// <summary>
        /// An action that supports the HTTP DELETE method.
        /// </summary>
        /// <param name="ids">A typed list of entity IDs specified in the route.</param>
        /// <param name="queryString">Represents the URL query string.</param>
        /// <returns>The result of the action.</returns>
        [HttpDelete]
        [BatchRoute("({ids})")]
        [ValidateQueryString]
        public virtual async Task<IActionResult> DeleteBatch(KeyList<TKey> ids, QueryString queryString)
        {
            var query = Repository.Query.Where(p => ids.Contains(p.Id));
            query = ApplyRouteParams(query);

            var entityList = await query.ToListAsync();

            if (ids.Count() != entityList.Count())
            {
                var missingIds = ids.Except(entityList.Select(p => p.Id));
                return await BuildResponseForNotFoundEntity(missingIds, queryString);
            }

            Repository.RemoveRange(entityList);

            var flowResult = await Flow.Delete(entityList, Save);
            if (flowResult != null)
            {
                return flowResult;
            }

            return NoContent();
        }

        private IQueryable<TEntity> ApplyRouteParams(IQueryable<TEntity> query)
        {
            var filter = (Expression<Func<TEntity, bool>>)RouteFilterBuilder.BuildFilter(GetType());
            return filter != null ? query.Where(filter) : query;
        }

        private async Task<IActionResult> BuildResponseForNotFoundEntity(TKey id, QueryString queryString)
        {
            return await BuildResponseForNotFoundEntity(new List<TKey> { id }, queryString);
        }

        private async Task<IActionResult> BuildResponseForNotFoundEntity(IEnumerable<TKey> ids, QueryString queryString)
        {
            await ValidateRoute(ids, queryString);
            if (!ResponseBuilder.HasErrors())
            {
                AddRecordNotFoundError(ids);
            }

            return NotFound(ResponseBuilder.Build());
        }

        private async Task ValidateRoute(QueryString queryString)
        {
            await ValidateRoute(new List<TKey>(), queryString);
        }

        private async Task ValidateRoute(TKey id, QueryString queryString)
        {
            await ValidateRoute(new List<TKey>() { id }, queryString);
        }

        private async Task ValidateRoute(bool forceValidation)
        {
            await ValidateRoute(new List<TKey>(), null, forceValidation);
        }

        private async Task ValidateRoute(IEnumerable<TKey> ids, QueryString queryString, bool forceValidation = false)
        {
            if (forceValidation || queryString.ValidateRoute)
            {
                var result = await RouteValidator.ValidateRoute(GetType(), ids.Select(p => p as object));

                if (result != null)
                {
                    switch (result.ErrorType)
                    {
                        case RecordErrorType.RecordNotExist:
                            if (result.EntityType != typeof(TEntity))
                            {
                                AddRecordNotFoundError(result);
                            }
                            break;
                        case RecordErrorType.RecordRelationNotValid:
                            AddRecordNotBelongToParentError(result);
                            break;
                    }
                }
            }
        }

        private void AddRecordNotFoundError(TKey id)
        {
            AddRecordNotFoundError(new KeyList<TKey>() { id });
        }

        private void AddRecordNotFoundError(IEnumerable<TKey> ids)
        {
            foreach (var id in ids)
            {
                AddRecordNotFoundError(new RecordError
                {
                    ErrorType = RecordErrorType.RecordNotExist,
                    EntityType = typeof(TEntity),
                    EntityId = id
                });
            }
        }

        private void AddRecordNotFoundError(RecordError error)
        {
            ResponseBuilder.AddError(
                $"{GetLocalizedEntityName(error.EntityType.Name)} : {ApiResources.RecordNotFound}",
                $"{ApiResources.Id} = {error.EntityId}");
        }

        private void AddRecordNotBelongToParentError(RecordError error)
        {
            ResponseBuilder.AddError(
                $"{GetLocalizedEntityName(error.ParentEntityType.Name)} - {GetLocalizedEntityName(error.EntityType.Name)} : {ApiResources.RecordNotBelongToParent}",
                $"{GetLocalizedEntityName(error.ParentEntityType.Name)} : {ApiResources.Id} = {error.ParentEntityId}",
                $"{GetLocalizedEntityName(error.EntityType.Name)} : {ApiResources.Id} = {error.EntityId}");
        }

        private async Task<IActionResult> Save()
        {
            return await Repository.SaveAsync() ?
                null :
                new InternalServerErrorResult(ResponseBuilder
                    .AddError($"{GetLocalizedEntityName(typeof(TEntity).Name)} : {ApiResources.InternalServerError}")
                    .Build());
        }

        private string GetLocalizedEntityName(string entityName)
        {
            return EntityLocalizer?.GetLocalizedEntityName(entityName) ?? entityName;
        }

        internal abstract Task<IActionResult> PostFlow(TInDto dto, TEntity entity, Func<Task<IActionResult>> saveFunc);

        internal abstract Task<IActionResult> PostFlow(IEnumerable<TInDto> dtoList, IEnumerable<TEntity> entityList, Func<Task<IActionResult>> saveFunc);

        internal abstract Task<IActionResult> UpdateFlow(TInDto dto, TEntity entity, Func<Task<IActionResult>> saveFunc);

        internal abstract Task<IActionResult> UpdateFlow(IEnumerable<TInDto> dtoList, IEnumerable<TEntity> entityList, Func<Task<IActionResult>> saveFunc);

        /// <summary>
        /// Formats and writes a trace log message.
        /// </summary>
        /// <param name="data">An object that contains the data to be logged.</param>
        protected void LogTrace(object data)
        {
            Logger.LogTrace(Format(data));
        }

        /// <summary>
        /// Formats and writes a debug log message.
        /// </summary>
        /// <param name="data">An object that contains the data to be logged.</param>
        protected void LogDebug(object data)
        {
            Logger.LogDebug(Format(data));
        }

        /// <summary>
        /// Formats and writes an informational log message.
        /// </summary>
        /// <param name="data">An object that contains the data to be logged.</param>
        protected void LogInformation(object data)
        {
            Logger.LogInformation(Format(data));
        }

        /// <summary>
        /// Formats and writes a warning log message.
        /// </summary>
        /// <param name="data">An object that contains the data to be logged.</param>
        protected void LogWarning(object data)
        {
            Logger.LogWarning(Format(data));
        }

        /// <summary>
        /// Formats and writes an error log message.
        /// </summary>
        /// <param name="data">An object that contains the data to be logged.</param>
        protected void LogError(object data)
        {
            Logger.LogError(Format(data));
        }

        /// <summary>
        /// Formats and writes a critical log message.
        /// </summary>
        /// <param name="data">An object that contains the data to be logged.</param>
        protected void LogCritical(object data)
        {
            Logger.LogCritical(Format(data));
        }

        private string Format(object data)
        {
            return data is string ?
                Convert.ToString(data) :
                data.ToJson(Formatting.Indented);
        }

        private void LogData(object data)
        {
            if (Options.LogData)
            {
                LogDebug(data);
            }
        }
    }
}
