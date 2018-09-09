using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreApiDirect.Flow.Steps;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiDirect.Flow
{
    internal class Flow<TEntity, TInDto> : FlowDelete<TEntity>, IFlow<TEntity, TInDto>
    {
        private readonly IFlowStepBeforePost<TInDto, TEntity> _beforePost;
        private readonly IFlowStepBeforeUpdate<TInDto, TEntity> _beforeUpdate;
        private readonly IFlowStepBeforeSave<TInDto, TEntity> _beforeSave;
        private readonly IFlowStepAfterPost<TInDto, TEntity> _afterPost;
        private readonly IFlowStepAfterUpdate<TInDto, TEntity> _afterUpdate;
        private readonly IFlowStepAfterSave<TInDto, TEntity> _afterSave;

        public Flow(
            IFlowStepBeforePost<TInDto, TEntity> beforePost,
            IFlowStepBeforeUpdate<TInDto, TEntity> beforeUpdate,
            IFlowStepBeforeSave<TInDto, TEntity> beforeSave,
            IFlowStepBeforeDelete<TEntity> beforeDelete,
            IFlowStepAfterPost<TInDto, TEntity> afterPost,
            IFlowStepAfterUpdate<TInDto, TEntity> afterUpdate,
            IFlowStepAfterSave<TInDto, TEntity> afterSave,
            IFlowStepAfterDelete<TEntity> afterDelete)
            : base(beforeDelete, afterDelete)
        {
            _beforePost = beforePost;
            _beforeUpdate = beforeUpdate;
            _beforeSave = beforeSave;
            _afterPost = afterPost;
            _afterUpdate = afterUpdate;
            _afterSave = afterSave;
        }

        public async Task<IActionResult> Post(TInDto dto, TEntity entity, Func<Task<IActionResult>> saveFunc)
        {
            return await ProcessFlow(new List<FlowStepInfo>() {
                new FlowStepInfo(_beforePost, dto, entity),
                new FlowStepInfo(_beforeSave, dto, entity),
                new FlowStepInfo(new SaveStep(saveFunc)),
                new FlowStepInfo(_afterPost, dto, entity),
                new FlowStepInfo(_afterSave, dto, entity)
            });
        }

        public async Task<IActionResult> Post(IEnumerable<TInDto> dtoList, IEnumerable<TEntity> entityList, Func<Task<IActionResult>> saveFunc)
        {
            return await ProcessFlow(new List<FlowStepInfo>() {
                new FlowStepInfo(_beforePost, dtoList, entityList),
                new FlowStepInfo(_beforeSave, dtoList, entityList),
                new FlowStepInfo(new SaveStep(saveFunc)),
                new FlowStepInfo(_afterPost, dtoList, entityList),
                new FlowStepInfo(_afterSave, dtoList, entityList)
            });
        }

        public async Task<IActionResult> Update(TInDto dto, TEntity entity, Func<Task<IActionResult>> saveFunc)
        {
            return await ProcessFlow(new List<FlowStepInfo>() {
                new FlowStepInfo(_beforeUpdate, dto, entity),
                new FlowStepInfo(_beforeSave, dto, entity),
                new FlowStepInfo(new SaveStep(saveFunc)),
                new FlowStepInfo(_afterUpdate, dto, entity),
                new FlowStepInfo(_afterSave, dto, entity)
            });
        }

        public async Task<IActionResult> Update(IEnumerable<TInDto> dtoList, IEnumerable<TEntity> entityList, Func<Task<IActionResult>> saveFunc)
        {
            return await ProcessFlow(new List<FlowStepInfo>() {
                new FlowStepInfo(_beforeUpdate, dtoList, entityList),
                new FlowStepInfo(_beforeSave, dtoList, entityList),
                new FlowStepInfo(new SaveStep(saveFunc)),
                new FlowStepInfo(_afterUpdate, dtoList, entityList),
                new FlowStepInfo(_afterSave, dtoList, entityList)
            });
        }
    }

    internal class Flow<TEntity> : FlowDelete<TEntity>, IFlow<TEntity>
    {
        private readonly IFlowStepBeforePost<TEntity> _beforePost;
        private readonly IFlowStepBeforeUpdate<TEntity> _beforeUpdate;
        private readonly IFlowStepBeforeSave<TEntity> _beforeSave;
        private readonly IFlowStepAfterPost<TEntity> _afterPost;
        private readonly IFlowStepAfterUpdate<TEntity> _afterUpdate;
        private readonly IFlowStepAfterSave<TEntity> _afterSave;

        public Flow(
            IFlowStepBeforePost<TEntity> beforePost,
            IFlowStepBeforeUpdate<TEntity> beforeUpdate,
            IFlowStepBeforeSave<TEntity> beforeSave,
            IFlowStepBeforeDelete<TEntity> beforeDelete,
            IFlowStepAfterPost<TEntity> afterPost,
            IFlowStepAfterUpdate<TEntity> afterUpdate,
            IFlowStepAfterSave<TEntity> afterSave,
            IFlowStepAfterDelete<TEntity> afterDelete)
            : base(beforeDelete, afterDelete)
        {
            _beforePost = beforePost;
            _beforeUpdate = beforeUpdate;
            _beforeSave = beforeSave;
            _afterPost = afterPost;
            _afterUpdate = afterUpdate;
            _afterSave = afterSave;
        }

        public async Task<IActionResult> Post(TEntity entity, Func<Task<IActionResult>> saveFunc)
        {
            return await ProcessFlow(new List<FlowStepInfo>() {
                new FlowStepInfo(_beforePost, entity),
                new FlowStepInfo(_beforeSave, entity),
                new FlowStepInfo(new SaveStep(saveFunc)),
                new FlowStepInfo(_afterPost, entity),
                new FlowStepInfo(_afterSave, entity)
            });
        }

        public async Task<IActionResult> Post(IEnumerable<TEntity> entityList, Func<Task<IActionResult>> saveFunc)
        {
            return await ProcessFlow(new List<FlowStepInfo>() {
                new FlowStepInfo(_beforePost, entityList),
                new FlowStepInfo(_beforeSave, entityList),
                new FlowStepInfo(new SaveStep(saveFunc)),
                new FlowStepInfo(_afterPost, entityList),
                new FlowStepInfo(_afterSave, entityList)
            });
        }

        public async Task<IActionResult> Update(TEntity entity, Func<Task<IActionResult>> saveFunc)
        {
            return await ProcessFlow(new List<FlowStepInfo>() {
                new FlowStepInfo(_beforeUpdate, entity),
                new FlowStepInfo(_beforeSave, entity),
                new FlowStepInfo(new SaveStep(saveFunc)),
                new FlowStepInfo(_afterUpdate, entity),
                new FlowStepInfo(_afterSave, entity)
            });
        }

        public async Task<IActionResult> Update(IEnumerable<TEntity> entityList, Func<Task<IActionResult>> saveFunc)
        {
            return await ProcessFlow(new List<FlowStepInfo>() {
                new FlowStepInfo(_beforeUpdate, entityList),
                new FlowStepInfo(_beforeSave, entityList),
                new FlowStepInfo(new SaveStep(saveFunc)),
                new FlowStepInfo(_afterUpdate, entityList),
                new FlowStepInfo(_afterSave, entityList)
            });
        }
    }
}
