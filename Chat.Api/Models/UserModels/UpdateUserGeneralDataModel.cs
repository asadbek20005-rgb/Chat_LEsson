namespace Chat.Api.Models.UserModels
{
    public class UpdateUserGeneralDataModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string? Age { get; set; }
    }
}