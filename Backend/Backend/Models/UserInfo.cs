namespace Backend.Models
{
	/// <summary>
	/// Информация о пользователе, которую можно ему показать
	/// </summary>
	public class UserInfo
    {
		/// <summary>
		/// Имя пользователя
		/// </summary>
        public string Name { get; set; }

		/// <summary>
		/// Баланс пользователя
		/// </summary>
		public int Balance { get; set; }
    }
}
