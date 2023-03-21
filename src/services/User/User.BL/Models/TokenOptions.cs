namespace User.BL.Models
{
    public class TokenOptions
    {
        public string SecretKey { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
    }
}
