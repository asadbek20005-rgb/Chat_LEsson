namespace Chat.Api.Exceptions
{
    public class UserNotFound : Exception
    {
        public UserNotFound(): base("User Not Found")
        {
            
        }
    }
}
