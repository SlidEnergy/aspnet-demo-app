using Swashbuckle.Swagger;
using System.Collections.Generic;
using System.Web.Http.Description;

namespace Backend
{
	/// <summary>
	/// Добавляет описание конечного пути для получения токена.
	/// </summary>
	public class TokenEndpointDocumentFilter : IDocumentFilter
	{
		/// <summary>
		/// Применяет фильтр к документу.
		/// </summary>
		public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
		{
			swaggerDoc.paths.Add("/auth/v2/token", new PathItem
			{
				post = new Operation
				{
					tags = new string[] { "Authentication" },
					summary = "Проводит аутентификацию пользователя и выдает токен",
					operationId = "Authentication_GetToken",
					consumes = new string[] { "application/x-www-form-url-encoded" },
					produces = new string[] { "application/json" },
					parameters = new List<Parameter>
					{
						new Parameter
						{
							name = "username",
							@in = "formData",
							type = "string",
							required = true,
							description = "Имя входа"
						},
						new Parameter
						{
							name = "password",
							@in = "formData",
							type = "string",
							required = true,
							format = "password",
							description = "Пароль"
						},
						new Parameter
						{
							name = "grant_type",
							@in = "formData",
							type = "string",
							required = true,
							@default = "password",
							description = "Тип авторизации"
						}
					},
					responses = new Dictionary<string, Response>
					{
						{
							"200",
							new Response
							{
								description = "Успешно",
								schema = new Schema
								{
									@ref = "#/definitions/AuthInfoWithToken"
								}
							}
						},
						{
							"500",
							new Response
							{
								description = "Необработанная ошибка сервера.",
							}
						},
					}
				}
			});

			swaggerDoc.definitions.Add("AuthInfoWithToken", new Schema
			{
				type = "object",
				properties = new Dictionary<string, Schema>
				{
					{
						"access_token",
						new Schema
						{
							type = "string",
							description = "Токен для доступа к Api"
						}
					},
					{
						"expires_in",
						new Schema
						{
							type = "integer",
							description = "Время, через которое истекет время действия токена"
						}
					},
					{
						"userName",
						new Schema
						{
							type = "string",
							description = "Отображаемое имя пользователя"
						}
					},
					{
						"isAdmin",
						new Schema
						{
							type = "string",
							description = "Json строка. Флаг, указывающий, что пользователь является администратором"
						}
					},
					{
						"permissions",
						new Schema
						{
							type = "string",
							description = "Json строка. Массив доступных разрешений пользователю"
						}
					},
					{
						".issued",
						new Schema {
							type = "string",
							description = "Дата выпуска токена"
						}
					},
					{
						".expires",
						new Schema {
							type = "string",
							description = "Дата истечения действия токена"
						}
					},
				},
				required = new List<string> {
					"access_token",
					"expires_in",
					"userName",
					"isAdmin",
					"permissions",
					".issued",
					".expires"
				}
			});
		}
	}
}