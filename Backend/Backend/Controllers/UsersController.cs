using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using NLog;
using Backend.Models;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace Backend
{
	/// <summary>
	/// Регистрация новых пользователей, получение списка пользователей. 
	/// </summary>
	[Authorize]
    public class UsersController : ApiController
    {
		private Logger logger = NLog.LogManager.GetCurrentClassLogger();

		private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

		public UsersController(ApplicationUserManager userManager)
		{
			UserManager = userManager;
		}

		public UsersController() { }

		// GET api/users
		/// <summary>
		/// Возвращает информации по пользователю
		/// </summary>
		/// <param name="userName">Имя пользователя</param>
		[HttpGet]
		[Route("api/users/current", Name = "GetCurrentUser")]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
		[ResponseType(typeof(UserInfo))]
		public IHttpActionResult GetCurrentUser()
        {
			this.logger.Debug("Web Api. Получение текущего пользователя");

			var user = this.UserManager.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();

			if (user == null)
				return NotFound();

            return Ok(new UserInfo
            {
                Name = User.Identity.GetUserName(),
				Balance = user.Balance
            });
        }

		// GET api/users
		/// <summary>
		/// Возвращает список пользователей
		/// </summary>
		[HttpGet]
		[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
		[ResponseType(typeof(string[]))]
		public IHttpActionResult GetList()
		{
			this.logger.Debug("Web Api. Получение списка пользователей");

			// Выдаем список всех имен пользователей. Другую информацию выдавать нельзя.
			return Ok(this.UserManager.Users.Select(x => x.UserName).ToArray());
		}

        // POST api/users
		/// <summary>
		/// Регистрирует нового пользователя в системе
		/// </summary>
		/// <param name="model">Данные нового пользователя.</param>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
			this.logger.Debug("Web Api. Регистрация нового пользователя");

			// проверяем вхдоные данные
			if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

			// Создаем нового пользователя и даем ему 500 PW
            var user = new ApplicationUser() { UserName = model.UserName, Email = model.Email, Balance = 500 };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

			// Если произошла ошибка, выдаем сообщение пользователю
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

			// Возвращаем созданного пользователя с сылкой на ресурс
			return Created(user.UserName, new UserInfo { Name = user.UserName, Balance = user.Balance });
        }

        protected override void Dispose(bool disposing)
        {
			// очищаем ресурсы
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

		/// <summary>
		/// Формирует ошибку на основе ответа при создании нового пользователя
		/// </summary>
        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
			// result не должен быть null, т.к. выше должна быть проверка
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
					// Формируем список ошибок
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
					// Нет информации по ошибкам
                    return BadRequest();
                }

				// Выдаем список ошибок
                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
