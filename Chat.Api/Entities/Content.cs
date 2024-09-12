namespace Chat.Api.Entities
{
    public class Content
    {
        public int Id { get; set; }
        public string FileUrl { get; set; }
        public string Caption { get; set; }
        public string? Type { get; set; }
        public int? MessageId { get; set; }
    }
}
