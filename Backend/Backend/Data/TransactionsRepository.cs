using NLog;
using Backend.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Data
{
	public class TransactionsRepository: ITransactionsRepository, IDisposable
	{
		private Logger logger = NLog.LogManager.GetCurrentClassLogger();

		private ApplicationDbContext db = new ApplicationDbContext();

		public Transaction Get(int id)
		{
			// Ищем транзакцию
			return db.Transactions.Find(id);
		}

		public Transaction[] GetList(string userName)
		{
			this.logger.Debug("Получение списка транзакций");

			return db.Transactions.Where(x => x.SenderName == userName || x.CorrespondentName == userName).ToArray();
		}

		public async Task<Transaction> Post(ApplicationUser recipient, ApplicationUser sender, int amount)
		{
			this.logger.Debug("Проведение транзакции");

			Transaction transaction = null;

			// Открываем новую транзакцию, чтобы операции списания и зачисления средства входили в неё, и она была атомарной.
			// В случае ошибки будет произведет откат всей транзакции
			using (var efTransaction = db.Database.BeginTransaction())
			{
				recipient = db.Users.Where(x => x.UserName == recipient.UserName).FirstOrDefault();
				sender = db.Users.Where(x => x.UserName == sender.UserName).FirstOrDefault();

				// Списываем и зачесляем PW
				sender.Balance -= amount;
				recipient.Balance += amount;

				// Добавляем историю о транзакции
				transaction = db.Transactions.Add(new Transaction
				{
					Amount = amount,
					Balance = sender.Balance,
					CorrespondentName = recipient.UserName,
					SenderName = sender.UserName,
					DateTime = DateTime.Now
				});

				await db.SaveChangesAsync();

				// выполняем транзакцию
				efTransaction.Commit();
			}

			this.logger.Debug("Транзакция проведена успешно");

			return transaction;
		}

		public void Dispose()
		{
			// очищаем ресурс
			db.Dispose();
		}
	}
}