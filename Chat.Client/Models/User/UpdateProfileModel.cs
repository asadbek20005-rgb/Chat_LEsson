namespace Chat.Client.Models.User
{
    public class UpdateProfileModel
    {
        //      "firstName": "string",
        //"lastName": "string",
        //"gender": "string",
        //"age": "string"

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public string? Age { get; set; }
    }
}