namespace Chat.Api.Exceptions
{
    public class MessageNullException : Exception
    {
        public MessageNullException(string errorMessage) : base(errorMessage) 
        {
            
        }
    }
}
