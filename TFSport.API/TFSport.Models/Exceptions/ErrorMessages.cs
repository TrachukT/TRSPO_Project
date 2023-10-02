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

        public const string NoAuthorsArticles = "You have no articles posted.";

        public const string NoArticlesPublished = "There are no articles published now.";

        public const string ArticleWithThisTitleExists = "An article with the same title already exists.";

        public const string ArticleDoesntExist = "An article with such ID doesn't exist.";

        public const string BlobContainerDoesntExist = "Such blob container doesn't exist.";

        public const string BlobDoesntExist = "Such blob client doesn't exist.";

        public const string TitleIsRequired = "Title is required.";

        public const string DescriptionIsRequired = "Description is required.";

        public const string AuthorIsRequired = "Author is required.";

        public const string InvalidAuthorId = "Author ID has to be a GUID.";

        public const string FileExists = "This file is already uploaded.";

        public const string NoImageProvided = "No image provided.";

        public const string ChangeStatusNotPermitted = "You are not authorized to change the status of this article.";

        public const string UpdateNotPermitted = "You are not authorized to update this article.";

        public const string CantUpdatePublished = "You can't edit articles that are already published.";

        public const string TitleLength = "The title must be between 15 and 70 characters.";

        public const string DescriptionLength = "Description length must be between 50 and 150 characters.";

        public const string TagsQuantity = "Up to 5 tags are allowed.";

        public const string ContentLength = "The content must be between 200 and 5000 characters.";

        public const string ImageSizeExceeded = "Image size could not be bigger than 2MB.";

        public const string InvalidImageFormat = "Only .jpeg and .png image formats are allowed.";

        public const string CommentDoesntExist = "Comment does not exist.";

        public const string UnauthorizedToDeleteComment = "You are not authorized to delete this comment.";

        public const string ContentIsRequired = "Content of the comment cannot be empty.";
    }
}
