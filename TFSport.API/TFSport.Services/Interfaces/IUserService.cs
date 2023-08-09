﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSport.Models;

namespace TFSport.Services.Interfaces
{
	public interface IUserService
	{
        public Task<User> GetUserByEmailAsync(string email);

        public Task<IList<UserRoles>> GetUserRolesByEmailAsync(string email);

		public Task RegisterUser(User user);

		public Task ForgotPassword(string email);

		public Task RestorePassword(string token,string password);

		public Task EmailVerification(string email, string verificationToken);
	}
}
