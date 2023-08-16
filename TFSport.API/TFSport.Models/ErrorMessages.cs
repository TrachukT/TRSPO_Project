﻿namespace TFSport.Models
{
    public class ErrorMessages
    {
        public const string EmailIsRegistered = "This email is already used by another account.";

        public const string PasswordMatch = "The password and repeat password do not match.";

        public const string NamesValidation = "First and last name should start with an uppercase letter and contain only letters.";

        public const string PasswordValidation = "Password needs to be with 8 symbols, only Latin letters, at least one uppercase letter, one lowercase letter, one number, and one special character like -,_,+,=.";

        public const string EmailNotValid = "Email you entered is not valid.";

        public const string NotRegisteredEmail = "This email is not registered in our system. Try another one.";

        public const string NotValidLink = "This link is already not valid.";

        public const string InvalidCredentials = "Invalid credentials.";

        public const string InvalidRefreshToken = "Invalid refresh token.";

        public const string FirstNameIsRequired = "First name is required.";

        public const string LastNameIsRequired = "Last name is required.";

        public const string EmailIsRequired = "Email field cannot be empty.";

        public const string PasswordIsRequired = "Password field cannot be empty.";

        public const string PasswordMinLength = "Password must be at least 8 characters.";

        public const string UserNotFound = "User not found.";

        public const string RoleIsRequired = "Role field cannot be empty.";
    }
}
