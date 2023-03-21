using Core.Application.Common;

namespace Order.Application.Models.Contracts
{
    public class CheckLimitResult : BaseHttpResult
    {
        public bool IsLimitExceeded { get; set; }
    }
}
