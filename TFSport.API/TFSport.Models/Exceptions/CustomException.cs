namespace TFSport.Models.Exceptions
{
    public class CustomException : Exception
    {
        public string DisplayMessage { get; set; }

        public CustomException(string displayMessage)
            : base(displayMessage)
        {
            DisplayMessage = displayMessage;
        }

        public CustomException(string displayMessage, Exception innerException)
            : base(displayMessage, innerException)
        {
            DisplayMessage = displayMessage;
        }

        public override string ToString()
        {
            return DisplayMessage;
        }
    }
}
