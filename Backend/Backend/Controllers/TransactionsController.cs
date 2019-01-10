using Microsoft.AspNet.Identity.Owin;
using NLog;
using Backend.Data;
using Backend.Models;
using System;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Backend
{
	/// <summary>
	/// Проведение и просмотр транзакций
	/// </summary>
	[Authorize]
	public class TransactionsController : ApiController
    {
		private Logger logger = NLog.LogManager.GetCurrentClassLogger();

		private ITransactionsRepository repository;

		public ITransactionsRepository Repository
		{
			get
			{
				return repository ?? Request.GetOwinContext().Get<TransactionsRepository>();
			}
			private set
			{
				repository = value;
			}
		}

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

		public TransactionsController(ApplicationUserManager userManager, ITransactionsRepository repository) {
			this.repository = repository;
			this.UserManager = userManager;
		}

		public TransactionsController() { }

		// GET: api/Transactions
		/// <summary>
		/// Возвращает список транзакций для текущего пользователя
		/// </summary>
		[HttpGet]
		[ResponseType(typeof(Transaction[]))]
		public IHttpActionResult GetTransactions()
        {
			this.logger.Debug("Web Api. Получение списка транзакций");

			var list = Repository.GetList(User.Identity.Name);

			return Ok(list);
        }

        // GET: api/Transactions/5
		/// <summary>
		/// Возвращает транзакцию по идентификатору.
		/// </summary>
		/// <param name="id">Идентификатор транзакции.</param>
		[Route("api/transactions/{id}", Name = "GetTransaction")]
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult GetTransaction(int id)
        {
			this.logger.Debug("Web Api. Получение транзакции по идентификатору");

			if (id <= 0)
				return BadRequest();

			// Ищем транзакцию
			var transaction = Repository.Get(id);

			// Если транзакция не найдена или запрещена для текущего пользователя, отправляем ошибку
            if (transaction == null || transaction.SenderName != User.Identity.Name)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        // POST: api/Transactions
		/// <summary>
		/// Проводит транзакцию.
		/// </summary>
		/// <param name="transactionData">Параметры транзакции.</param>
        [ResponseType(typeof(Transaction))]
        public async Task<IHttpActionResult> PostTransaction(TransactionBindingModel transactionData)
        {
			this.logger.Debug("Web Api. Проведение транзакции");

			if (transactionData == null)
				return BadRequest();

			if (string.IsNullOrEmpty(transactionData.UserName) || transactionData.Amount <= 0)
				return BadRequest();

			// Находим получателя
			var recipient = this.UserManager.Users.Where(x => x.UserName == transactionData.UserName).FirstOrDefault();

			// Выдаем ошибку если получатель не найден
			if (recipient == null)
				return NotFound();

			// Получаем текущего пользователя
			var sender = this.UserManager.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();

			// Выдаем ошибку если не удалось найти текущего пользователя
			if (sender == null)
				return InternalServerError();

			// Выдаем ошибку если баланса не достаточно для транзакции
			if (sender.Balance < transactionData.Amount)
				return BadRequest();

			Transaction transaction = await Repository.Post(recipient, sender, transactionData.Amount);

			// возвращаем историю проведенной транзакции и ссылку на новый ресурс
            return CreatedAtRoute("GetTransaction", new { id = transaction.Id }, transaction);
        }
    }
}