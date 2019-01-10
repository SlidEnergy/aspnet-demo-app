using Backend.Models;
using Microsoft.AspNet.Identity;
using NUnit.Framework;
using Rhino.Mocks;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace Backend.Tests
{
    [TestFixture]
	public class UsersControllerTest
	{
		ApplicationUserManager manager;
		/// <summary>
		/// Инициализация тестов
		/// </summary>
		[OneTimeSetUp]
		public void Init()
		{
			var user = new ApplicationUser() { UserName = "user1", Email = "user1@test.ru", Balance = 500 };

			var mockStore = MockRepository.GenerateMock<IUserStore<ApplicationUser>>();
			mockStore.Stub(x => x
				.CreateAsync(user))
				.Return(Task.FromResult(IdentityResult.Success));

			mockStore.Stub(x => x.FindByNameAsync(user.UserName))
			   .Return(Task.FromResult(user));

			var mockUserManager = MockRepository.GenerateMock<ApplicationUserManager>(mockStore);
			mockUserManager.Stub(x => x.CreateAsync(Arg<ApplicationUser>.Is.Anything, Arg<string>.Is.Anything)).Return(Task.FromResult(IdentityResult.Success));

			this.manager = mockUserManager;
		}

		[Test]
		public async Task RegisterUser_ReturnExpected()
		{
			var controller = new UsersController(this.manager);

			var model = new Models.RegisterBindingModel
			{
				UserName = "user1",
				Email = "user1@test.ru",
				Password = "Password123#",
				ConfirmPassword = "Password123#"
			};

			var result = await controller.Register(model);

			Assert.IsNotNull(result);
			Assert.IsInstanceOf<CreatedNegotiatedContentResult<UserInfo>>(result);

			var content = (result as CreatedNegotiatedContentResult<UserInfo>).Content;
			Assert.AreEqual(model.UserName, content.Name);
			Assert.AreEqual(500, content.Balance);
		}

		[Test]
		public async Task GetList_ReturnExpected()
		{
			var user1 = new ApplicationUser() { UserName = "user1", Email = "user1@test.ru", Balance = 500 };
			var user2 = new ApplicationUser() { UserName = "user2", Email = "user2@test.ru", Balance = 500 };

			this.manager.Stub(x => x.Users).Return(new ApplicationUser[] { user1, user2 }.AsQueryable());

			var controller = new UsersController(this.manager);

			var result = controller.GetList();

			Assert.IsNotNull(result);
			Assert.IsInstanceOf<OkNegotiatedContentResult<string[]>>(result);

			var content = (result as OkNegotiatedContentResult<string[]>).Content;
			Assert.AreEqual(2, content.Length);
			Assert.AreEqual(user1.UserName, content[0]);
			Assert.AreEqual(user2.UserName, content[1]);
		}

		[Test]
		public async Task GetUser_ReturnExpected()
		{
			var user = new ApplicationUser() { UserName = "user1", Email = "user1@test.ru", Balance = 500 };

			this.manager.Stub(x => x.Users).Return(new ApplicationUser[] { user }.AsQueryable());

			var controller = new UsersController(this.manager);

			Thread.CurrentPrincipal = new GenericPrincipal
			(
			   new GenericIdentity(user.UserName, "Passport"),
			   new string [0]
			);

			var result = controller.GetCurrentUser();

			Assert.IsNotNull(result);
			Assert.IsInstanceOf<OkNegotiatedContentResult<UserInfo>>(result);

			var content = (result as OkNegotiatedContentResult<UserInfo>).Content;
			Assert.AreEqual(user.UserName, content.Name);
			Assert.AreEqual(500, content.Balance);
		}
	}
}
