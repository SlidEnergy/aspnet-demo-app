using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Data
{
	public interface ITransactionsRepository
	{
		Transaction Get(int id);
		Transaction[] GetList(string userName);
		Task<Transaction> Post(ApplicationUser recipient, ApplicationUser sender, int amount);
	}
}
