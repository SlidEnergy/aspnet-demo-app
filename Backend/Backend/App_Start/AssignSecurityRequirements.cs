﻿using Swashbuckle.Swagger;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Backend
{
	/// <summary>
	/// Задает требования авторизации для доступа к контроллерам.
	/// </summary>
	public class AssignSecurityRequirements : IOperationFilter
	{
		/// <summary>
		/// Применяет фильтр для переданного ApiDescription.
		/// </summary>
		public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
		{
			var actFilters = apiDescription.ActionDescriptor.GetFilterPipeline();
			var allowsAnonymous = actFilters.Select(f => f.Instance).OfType<OverrideAuthorizationAttribute>().Any();
			if (allowsAnonymous)
				return; // must be an anonymous method

			//var scopes = apiDescription.ActionDescriptor.GetFilterPipeline()
			//    .Select(filterInfo => filterInfo.Instance)
			//    .OfType<AllowAnonymousAttribute>()
			//    .SelectMany(attr => attr.Roles.Split(','))
			//    .Distinct();

			if (operation.security == null)
				operation.security = new List<IDictionary<string, IEnumerable<string>>>();

			var oAuthRequirements = new Dictionary<string, IEnumerable<string>>
			{
				{"oauth2", new string[] { } },
				{"bearer", new string[] { } }
			};

			operation.security.Add(oAuthRequirements);
		}
	}
}