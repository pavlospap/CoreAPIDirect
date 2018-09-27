using System;
using CoreApiDirect.Base;
using CoreApiDirect.Boot.Generators;
using CoreApiDirect.Controllers;
using CoreApiDirect.Controllers.Shaping;
using CoreApiDirect.Entities;
using CoreApiDirect.Flow;
using CoreApiDirect.Flow.Steps;
using CoreApiDirect.Mapping;
using CoreApiDirect.Mapping.Configuration;
using CoreApiDirect.Options;
using CoreApiDirect.Query;
using CoreApiDirect.Query.Detail;
using CoreApiDirect.Query.Filter;
using CoreApiDirect.Repositories;
using CoreApiDirect.Response;
using CoreApiDirect.Routing;
using CoreApiDirect.Url.Parsing;
using CoreApiDirect.Url.Parsing.Parameters;
using CoreApiDirect.Url.Parsing.Parameters.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CoreApiDirect.Boot
{
    /// <summary>
    /// Extensions for the Microsoft.Extensions.DependencyInjection.IServiceCollection.
    /// </summary>
    public static class IServiceCollectionExtentions
    {
        /// <summary>
        /// Adds CoreAPIDirect services.
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddCoreApiDirect(this IServiceCollection services)
        {
            var coreOptions = services.BuildServiceProvider().GetService<IOptions<CoreOptions>>().Value;
            var routePrefixConvention = new RoutePrefixConvention(coreOptions.RoutePrefix);

            return services.Configure<MvcOptions>(options => options.Conventions.Add(routePrefixConvention))
                .AddApiServices(coreOptions)
                .AddRepositories()
                .AddFlows()
                .AddFlowSteps()
                .AddQueryBuilders()
                .AddQueryPropertyWalkers()
                .AddQueryPropertyWalkerVisitors()
                .AddOneToManyQueryDetailPropertyWalkers()
                .AddOneToManyQueryDetailPropertyWalkerVisitors()
                .AddOneToOneQueryDetailPropertyWalkers()
                .AddOneToOneQueryDetailPropertyWalkerVisitors()
                .AddEntityMappers();
        }

        /// <summary>
        /// Adds CoreAPIDirect services with configuration.
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        /// <param name="setupAction">A System.Action`1 to configure the provided CoreApiDirect.Options.CoreOptions.</param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddCoreApiDirect(this IServiceCollection services, Action<CoreOptions> setupAction)
        {
            return services.Configure(setupAction)
                .AddCoreApiDirect();
        }

        internal static IServiceCollection AddApiServices(this IServiceCollection services, CoreOptions options)
        {
            return services.AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                .AddScoped<IBooleanParameterParser, BooleanParameterParser>()
                .AddTransient<IEntityMapperConfigPropertyWalker, EntityMapperConfigPropertyWalker>()
                .AddTransient<IEntityMapperConfigPropertyWalkerVisitor, EntityMapperConfigPropertyWalkerVisitor>()
                .AddScoped<IEntityMapperConfigurator, EntityMapperConfigurator>()
                .AddSingleton<IFieldNameResolver, FieldNameResolver>()
                .AddSingleton<IFieldsFieldValidator, FieldsFieldValidator>()
                .AddScoped<IFieldsParameterParser, FieldsParameterParser>()
                .AddSingleton<IFilterFieldValidator, FilterFieldValidator>()
                .AddScoped<IFilterParameterParser, FilterParameterParser>()
                .AddSingleton<IForeignKeysResolver, ForeignKeysResolver>()
                .AddScoped<IIntegerParameterParser, IntegerParameterParser>()
                .AddSingleton<IMethodProvider, MethodProvider>()
                .AddSingleton<IModelStateResolver, ModelStateResolver>()
                .AddScoped<IPagingHeaderBuilder, PagingHeaderBuilder>()
                .AddScoped<IParameterParserFactory, ParameterParserFactory>()
                .AddScoped<IPlainFilterBuilder, PlainFilterBuilder>()
                .AddSingleton<IPropertyProvider, PropertyProvider>()
                .AddTransient<IQueryFilterPropertyWalker, QueryFilterPropertyWalker>()
                .AddTransient<IQueryFilterPropertyWalkerVisitor, QueryFilterPropertyWalkerVisitor>()
                .AddScoped<IQueryStringParser, QueryStringParser>()
                .AddScoped(typeof(IResponseBuilder), options.ResponseBuilderType ?? throw new InvalidOperationException("CoreApiDirect.Response.IResponseBuilder implementation is missing."))
                .AddScoped<IRouteFilterBuilder, RouteFilterBuilder>()
                .AddScoped<IRouteValidator, RouteValidator>()
                .AddTransient<IShapePropertyWalker, ShapePropertyWalker>()
                .AddTransient<IShapePropertyWalkerVisitor, ShapePropertyWalkerVisitor>()
                .AddScoped<IShaper, Shaper>()
                .AddSingleton<ISortFieldValidator, SortFieldValidator>()
                .AddScoped<ISortParameterParser, SortParameterParser>()
                .AddScoped<IStringParameterParser, StringParameterParser>();
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services.AddFromServiceGenerator(new RepositoryServiceGenerator(TypeProvider.Instance, typeof(Entity<>), typeof(IRepository<,>), typeof(Repository<,,>)));
        }

        private static IServiceCollection AddFlows(this IServiceCollection services)
        {
            return services.AddFromServiceGenerator(new ServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,,,>), typeof(IFlow<,>), typeof(Flow<,>), ServiceLifetime.Scoped))
                .AddFromServiceGenerator(new ServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,>), typeof(IFlow<>), typeof(Flow<>), ServiceLifetime.Scoped));
        }

        private static IServiceCollection AddFlowSteps(this IServiceCollection services)
        {
            return services.AddFromServiceGenerator(new FlowStepServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,,,>), typeof(IFlowStepBeforePost<,>), typeof(FlowStepBeforePost<,>)))
                .AddFromServiceGenerator(new FlowStepServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,,,>), typeof(IFlowStepBeforeUpdate<,>), typeof(FlowStepBeforeUpdate<,>)))
                .AddFromServiceGenerator(new FlowStepServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,,,>), typeof(IFlowStepBeforeSave<,>), typeof(FlowStepBeforeSave<,>)))
                .AddFromServiceGenerator(new FlowStepServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,,,>), typeof(IFlowStepBeforeDelete<>), typeof(FlowStepBeforeDelete<>)))
                .AddFromServiceGenerator(new FlowStepServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,,,>), typeof(IFlowStepAfterPost<,>), typeof(FlowStepAfterPost<,>)))
                .AddFromServiceGenerator(new FlowStepServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,,,>), typeof(IFlowStepAfterUpdate<,>), typeof(FlowStepAfterUpdate<,>)))
                .AddFromServiceGenerator(new FlowStepServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,,,>), typeof(IFlowStepAfterSave<,>), typeof(FlowStepAfterSave<,>)))
                .AddFromServiceGenerator(new FlowStepServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,,,>), typeof(IFlowStepAfterDelete<>), typeof(FlowStepAfterDelete<>)))
                .AddFromServiceGenerator(new FlowStepServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,>), typeof(IFlowStepBeforePost<>), typeof(FlowStepBeforePost<>)))
                .AddFromServiceGenerator(new FlowStepServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,>), typeof(IFlowStepBeforeUpdate<>), typeof(FlowStepBeforeUpdate<>)))
                .AddFromServiceGenerator(new FlowStepServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,>), typeof(IFlowStepBeforeSave<>), typeof(FlowStepBeforeSave<>)))
                .AddFromServiceGenerator(new FlowStepServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,>), typeof(IFlowStepBeforeDelete<>), typeof(FlowStepBeforeDelete<>)))
                .AddFromServiceGenerator(new FlowStepServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,>), typeof(IFlowStepAfterPost<>), typeof(FlowStepAfterPost<>)))
                .AddFromServiceGenerator(new FlowStepServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,>), typeof(IFlowStepAfterUpdate<>), typeof(FlowStepAfterUpdate<>)))
                .AddFromServiceGenerator(new FlowStepServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,>), typeof(IFlowStepAfterSave<>), typeof(FlowStepAfterSave<>)))
                .AddFromServiceGenerator(new FlowStepServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,>), typeof(IFlowStepAfterDelete<>), typeof(FlowStepAfterDelete<>)));
        }

        private static IServiceCollection AddQueryBuilders(this IServiceCollection services)
        {
            return services.AddFromServiceGenerator(new ServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,,,>), typeof(IQueryBuilder<>), typeof(QueryBuilder<>), ServiceLifetime.Scoped))
                .AddFromServiceGenerator(new ServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,>), typeof(IQueryBuilder<>), typeof(QueryBuilder<>), ServiceLifetime.Scoped));
        }

        private static IServiceCollection AddQueryPropertyWalkers(this IServiceCollection services)
        {
            return services.AddFromServiceGenerator(new ServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,,,>), typeof(IQueryPropertyWalker<>), typeof(QueryPropertyWalker<>), ServiceLifetime.Transient))
                .AddFromServiceGenerator(new ServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,>), typeof(IQueryPropertyWalker<>), typeof(QueryPropertyWalker<>), ServiceLifetime.Transient));
        }

        private static IServiceCollection AddQueryPropertyWalkerVisitors(this IServiceCollection services)
        {
            return services.AddFromServiceGenerator(new ServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,,,>), typeof(IQueryPropertyWalkerVisitor<>), typeof(QueryPropertyWalkerVisitor<>), ServiceLifetime.Transient))
                .AddFromServiceGenerator(new ServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,>), typeof(IQueryPropertyWalkerVisitor<>), typeof(QueryPropertyWalkerVisitor<>), ServiceLifetime.Transient));
        }

        private static IServiceCollection AddOneToManyQueryDetailPropertyWalkers(this IServiceCollection services)
        {
            return services.AddFromServiceGenerator(new ServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,,,>), typeof(IOneToManyQueryDetailPropertyWalker<>), typeof(OneToManyQueryDetailPropertyWalker<>), ServiceLifetime.Transient))
                .AddFromServiceGenerator(new ServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,>), typeof(IOneToManyQueryDetailPropertyWalker<>), typeof(OneToManyQueryDetailPropertyWalker<>), ServiceLifetime.Transient));
        }

        private static IServiceCollection AddOneToManyQueryDetailPropertyWalkerVisitors(this IServiceCollection services)
        {
            return services.AddFromServiceGenerator(new ServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,,,>), typeof(IOneToManyQueryDetailPropertyWalkerVisitor<>), typeof(OneToManyQueryDetailPropertyWalkerVisitor<>), ServiceLifetime.Transient))
                .AddFromServiceGenerator(new ServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,>), typeof(IOneToManyQueryDetailPropertyWalkerVisitor<>), typeof(OneToManyQueryDetailPropertyWalkerVisitor<>), ServiceLifetime.Transient));
        }

        private static IServiceCollection AddOneToOneQueryDetailPropertyWalkers(this IServiceCollection services)
        {
            return services.AddFromServiceGenerator(new ServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,,,>), typeof(IOneToOneQueryDetailPropertyWalker<>), typeof(OneToOneQueryDetailPropertyWalker<>), ServiceLifetime.Transient))
                .AddFromServiceGenerator(new ServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,>), typeof(IOneToOneQueryDetailPropertyWalker<>), typeof(OneToOneQueryDetailPropertyWalker<>), ServiceLifetime.Transient));
        }

        private static IServiceCollection AddOneToOneQueryDetailPropertyWalkerVisitors(this IServiceCollection services)
        {
            return services.AddFromServiceGenerator(new ServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,,,>), typeof(IOneToOneQueryDetailPropertyWalkerVisitor<>), typeof(OneToOneQueryDetailPropertyWalkerVisitor<>), ServiceLifetime.Transient))
                .AddFromServiceGenerator(new ServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,>), typeof(IOneToOneQueryDetailPropertyWalkerVisitor<>), typeof(OneToOneQueryDetailPropertyWalkerVisitor<>), ServiceLifetime.Transient));
        }

        private static IServiceCollection AddEntityMappers(this IServiceCollection services)
        {
            return services.AddFromServiceGenerator(new ServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,,,>), typeof(IEntityMapper<>), typeof(EntityMapper<>), ServiceLifetime.Scoped))
                .AddFromServiceGenerator(new ServiceGenerator(TypeProvider.Instance, typeof(ControllerApi<,>), typeof(IEntityMapper<>), typeof(EntityMapper<>), ServiceLifetime.Scoped));
        }

        private static IServiceCollection AddFromServiceGenerator(this IServiceCollection services, IServiceGenerator generator)
        {
            return generator.Generate(services);
        }
    }
}
