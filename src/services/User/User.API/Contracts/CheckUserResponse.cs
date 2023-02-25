namespace User.API.Contracts
{
    public class CheckUserResponse
    {
        public bool IsActive { get; set; }
        public DateTime ActivationDate { get; set; }
    }
}
