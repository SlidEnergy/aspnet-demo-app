using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Models
{
	/// <summary>
	/// Пользователь
	/// </summary>
	public class ApplicationUser : IdentityUser
    {
		/// <summary>
		/// Баланс средств PW у пользователя.
		/// </summary>
		public int Balance { get; set; }

		/// <summary>
		/// Создает новый UserIdentity
		/// </summary>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            return userIdentity;
        }
    }
}