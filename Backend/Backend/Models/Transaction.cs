using System;

namespace Backend.Models
{
	/// <summary>
	/// Транзакция
	/// </summary>
	public class Transaction
	{
		/// <summary>
		/// Идентификатор транзакции
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Время проведения транзакции
		/// </summary>
		public DateTime DateTime { get; set; }

		/// <summary>
		/// Имя получателя
		/// </summary>
		public string CorrespondentName { get; set; }

		/// <summary>
		/// Имя отправителя
		/// </summary>
		public string SenderName { get; set; }

		/// <summary>
		/// Количество PW для отправки
		/// </summary>
		public int Amount { get; set; }

		/// <summary>
		/// Баланс PW после отправки
		/// </summary>
		public int Balance { get; set; }
	}
}