using Core.Application.Common;

namespace Order.Application.Models.Contracts
{
    public class CheckUserResult : BaseHttpResult
    {
        public bool IsActive { get; set; }
        public DateTime ActivationDate { get; set; }
    }
}
