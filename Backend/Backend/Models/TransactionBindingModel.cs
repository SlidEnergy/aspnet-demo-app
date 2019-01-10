namespace Backend.Models
{
	/// <summary>
	/// Данные для проведения транзакции
	/// </summary>
	public class TransactionBindingModel
	{
		/// <summary>
		/// Имя получателя
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// Количество для перевода
		/// </summary>
		public int Amount { get; set; }
	}
}