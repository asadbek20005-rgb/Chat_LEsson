namespace Chat.Api.Exceptions
{
    [Serializable]
    internal class UserExist : Exception
    {
        public UserExist()
        {
        }

        public UserExist(string? message) : base(message)
        {
        }

        public UserExist(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}