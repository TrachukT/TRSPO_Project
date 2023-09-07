namespace TFSport.Models.Exceptions
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

        public const string UserIdIsRequired = "User ID field cannot be empty.";

        public const string EmailNotVerified = "Email must be verified before logging in.";

        public const string AlreadyVerifiedEmail = "This email has already been verified.";

        public const string NoAuthorsArticles = "You have no articles posted or in drafts.";

        public const string NoArticlesForReview = "There are no articles waiting for review.";

        public const string NoArticlesPublished = "There are no articles published now.";

        public const string ArticleWithThisTitleExists = "An article with the same title already exists.";

        public const string ArticleDoesntExist = "An article with such ID doesn't exist.";

        public const string BlobContainerDoesntExist = "Such blob container doesn't exist.";

        public const string BlobDoesntExist = "Such blob client doesn't exist.";

        public const string ArticleNotSentForReview = "Author didn't send this article for review yet.";
    }
}
