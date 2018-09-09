using System;
using System.Linq;
using System.Reflection;
using CoreApiDirect.Entities;
using Humanizer;
using Microsoft.AspNetCore.Mvc.Routing;

namespace CoreApiDirect.Controllers
{
    /// <summary>
    /// Used in controllers to generate the route template. The template is generated with the names of the entity types given in it's constructor.
    /// </summary>
    public class ApiRouteAttribute : Attribute, IRouteTemplateProvider
    {
        /// <summary>
        /// Gets the entity types that are used to generate the route template.
        /// </summary>
        public Type[] RouteEntityTypes { get; }

        /// <summary>
        /// The route template. It is generated from the the entity types given in the CoreApiDirect.Controllers.ApiRouteAttribute constructor.
        /// </summary>
        public string Template
        {
            get
            {
                string templateWithParams = string.Join("/", RouteEntityTypes.Take(RouteEntityTypes.Length - 1)
                    .Select(p => GetRouteEntityName(p).ToLower() + "/{" + p.Name.ToLower() + "id}"));

                return string.IsNullOrWhiteSpace(templateWithParams) ?
                    GetRouteEntityName(RouteEntityTypes.Last()).ToLower() :
                    templateWithParams + "/" + GetRouteEntityName(RouteEntityTypes.Last()).ToLower();
            }
        }

        /// <summary>
        /// The route order.
        /// </summary>
        public int? Order => null;

        /// <summary>
        /// The route name.
        /// </summary>
        public string Name => null;

        /// <summary>
        /// Initializes a new instance of the CoreApiDirect.Controllers.ApiRouteAttribute class.
        /// </summary>
        /// <param name="routeEntityTypes">The entity types that are used to generate the route template.</param>
        public ApiRouteAttribute(params Type[] routeEntityTypes)
        {
            if (!routeEntityTypes.Any())
            {
                throw new ArgumentException($"{nameof(routeEntityTypes)} are missing.");
            }

            RouteEntityTypes = routeEntityTypes;
        }

        private string GetRouteEntityName(Type type)
        {
            return type.GetCustomAttribute<SingularUrlAttribute>() == null ? type.Name.Pluralize() : type.Name;
        }
    }
}
