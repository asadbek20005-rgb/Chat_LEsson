namespace Chat.Api.Entities
{
    public class Chat
    {
        public Guid Id { get; set; }
        public List<string>? ChatNames { get; set; }

        public virtual List<User_Chat>? User_Chats { get; set; }

        public virtual List<Message>? Messages { get; set; }

        //internal void Deconstruct(out object checking, out object chat)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
