using Backend.Data;
using Backend.Models;
using Microsoft.AspNet.Identity;
using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace Backend.Tests
{
    [TestFixture]
	public class TransactionsControllerTest
	{
		ApplicationUserManager manager;

		/// <summary>
		/// Инициализация тестов
		/// </summary>
		[OneTimeSetUp]
		public void Init()
		{
			this.manager = CreateMockUserManager();
		}

		private static IDbSet<T> GetDbSetTestDouble<T>(IQueryable<T> data) where T : class
		{
			IDbSet<T> dbSet = MockRepository.GenerateMock<IDbSet<T>, IQueryable>();

			dbSet.Stub(m => m.Provider).Return(data.Provider);
			dbSet.Stub(m => m.Expression).Return(data.Expression);
			dbSet.Stub(m => m.ElementType).Return(data.ElementType);
			dbSet.Stub(m => m.GetEnumerator()).Return(data.GetEnumerator());

			return dbSet;
		}

		public ApplicationUserManager CreateMockUserManager()
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

			return mockUserManager;
		}

		[Test]
		public void GetTransactionList_ReturnList()
		{
			var user = new ApplicationUser() { UserName = "user1", Email = "user1@test.ru", Balance = 500 };

			var transaction1 = new Transaction()
			{
				Id = 1,
				Amount = 10,
				Balance = 490,
				CorrespondentName = "user1",
				DateTime = DateTime.Now,
				SenderName = user.UserName
			};

			var transaction2 = new Transaction()
			{
				Id = 2,
				Amount = 10,
				Balance = 480,
				CorrespondentName = "user1",
				DateTime = DateTime.Now,
				SenderName = user.UserName
			};

			var repository = MockRepository.GenerateMock<ITransactionsRepository>();
			repository.Stub(x => x.GetList(Arg<string>.Is.Anything)).Return(new Transaction[] { transaction1, transaction2 });

			var controller = new TransactionsController(this.manager, repository);

			Thread.CurrentPrincipal = new GenericPrincipal
			(
			   new GenericIdentity(user.UserName, "Passport"),
			   new string[0]
			);

			var result = controller.GetTransactions();

			Assert.IsNotNull(result);
			Assert.IsInstanceOf<OkNegotiatedContentResult<Transaction[]>>(result);

			var content = (result as OkNegotiatedContentResult<Transaction[]>).Content;
			Assert.AreEqual(2, content.Length);
		}

		[Test]
		public void GetTransaction_ReturnExpected()
		{
			var user = new ApplicationUser() { UserName = "user1", Email = "user1@test.ru", Balance = 500 };

			var transaction = new Transaction() { Id = 1, Amount = 10, Balance = 490, CorrespondentName = "user1",
				DateTime = DateTime.Now, SenderName = user.UserName};

			var repository = MockRepository.GenerateMock<ITransactionsRepository>();
			repository.Stub(x => x.Get(Arg<int>.Is.Anything)).Return(transaction);

			var controller = new TransactionsController(this.manager, repository);

			Thread.CurrentPrincipal = new GenericPrincipal
			(
			   new GenericIdentity(user.UserName, "Passport"),
			   new string[0]
			);

			var result = controller.GetTransaction(1);

			Assert.IsNotNull(result);
			Assert.IsInstanceOf<OkNegotiatedContentResult<Transaction>>(result);

			var content = (result as OkNegotiatedContentResult<Transaction>).Content;
			Assert.AreEqual(transaction.Id, content.Id);
			Assert.AreEqual(transaction.Balance, content.Balance);
			Assert.AreEqual(transaction.Amount, content.Amount);
			Assert.AreEqual(transaction.CorrespondentName, content.CorrespondentName);
			Assert.AreEqual(transaction.SenderName, content.SenderName);
		}

		[Test]
		public async Task PostTransaction_ReturnOk()
		{
			var user1 = new ApplicationUser() { UserName = "user1", Email = "user1@test.ru", Balance = 500 };
			var user2 = new ApplicationUser() { UserName = "user2", Email = "user2@test.ru", Balance = 500 };

			this.manager.Stub(x => x.Users).Return(new ApplicationUser[] { user1, user2 }.AsQueryable());

			var transaction = new Transaction()
			{
				Id = 1,
				Amount = 10,
				Balance = 490,
				CorrespondentName = user2.UserName,
				DateTime = DateTime.Now,
				SenderName = user1.UserName
			};

			var repository = MockRepository.GenerateMock<ITransactionsRepository>();
			repository.Stub(x => x.Post(Arg<ApplicationUser>.Is.Anything, Arg<ApplicationUser>.Is.Anything, Arg<int>.Is.Anything))
				.Return(Task.FromResult(transaction));

			var controller = new TransactionsController(this.manager, repository);

			Thread.CurrentPrincipal = new GenericPrincipal
			(
			   new GenericIdentity(user1.UserName, "Passport"),
			   new string[0]
			);

			var result = await controller.PostTransaction(new TransactionBindingModel { Amount = 10, UserName = user2.UserName });

			Assert.IsNotNull(result);
			Assert.IsInstanceOf<CreatedAtRouteNegotiatedContentResult<Transaction>>(result);

			var content = (result as CreatedAtRouteNegotiatedContentResult<Transaction>).Content;
			Assert.AreEqual(transaction.Id, content.Id);
			Assert.AreEqual(transaction.Balance, content.Balance);
			Assert.AreEqual(transaction.Amount, content.Amount);
			Assert.AreEqual(transaction.CorrespondentName, content.CorrespondentName);
			Assert.AreEqual(transaction.SenderName, content.SenderName);
		}
	}
}
