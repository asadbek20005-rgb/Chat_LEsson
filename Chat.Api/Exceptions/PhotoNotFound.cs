namespace Chat.Api.Exceptions
{
    public class PhotoNotFound : Exception
    {
        public PhotoNotFound():base("Photo is not found")
        {
            
        }
    }
}
