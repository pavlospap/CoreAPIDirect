using CoreApiDirect.Entities;
using Microsoft.Extensions.Localization;

namespace CoreApiDirect.Demo.Entities
{
    internal class EntityLocalizer : IEntityLocalizer
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public EntityLocalizer(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        public string GetLocalizedEntityName(string entityName)
        {
            return _localizer[entityName];
        }
    }
}
