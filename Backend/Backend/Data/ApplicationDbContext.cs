using Microsoft.AspNet.Identity.EntityFramework;
using Backend.Models;
using System.Data.Entity;

namespace Backend.Data
{
	/// <summary>
	/// Контекст базы данных для Entity Framework
	/// </summary>
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		/// <summary>
		/// Создает новый контекст
		/// </summary>
		public ApplicationDbContext()
			: base("DefaultConnection", throwIfV1Schema: false)
		{
		}

		/// <summary>
		/// Создает новый контекст
		/// </summary>
		/// <returns></returns>
		public static ApplicationDbContext Create()
		{
			return new ApplicationDbContext();
		}

		/// <summary>
		/// Список транзакций
		/// </summary>
		public IDbSet<Transaction> Transactions { get; set; }
	}
}