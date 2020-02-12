namespace UploadFile.WebApi.Jwt
{
    public class TokenConfigurations
    {
        public string Key { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int Days { get; set; }
    }
}