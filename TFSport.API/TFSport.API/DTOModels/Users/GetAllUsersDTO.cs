﻿using TFSport.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TFSport.API.DTOModels.Users
{
	public class GetAllUsersDTO
	{
		public string Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public bool EmailVerified { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public UserRoles UserRole { get; set; }
	}
}