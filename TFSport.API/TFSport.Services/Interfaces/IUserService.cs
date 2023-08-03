using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSport.Models;

namespace TFSport.Services.Interfaces
{
	public interface IUserService
	{
		public Task RegisterUser(User user);
	}
}
