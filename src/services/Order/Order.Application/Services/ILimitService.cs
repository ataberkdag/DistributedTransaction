using Order.Application.Models.Contracts;

namespace Order.Application.Services
{
    public interface ILimitService
    {
        public Task<CheckLimitResult> CheckLimit(CheckLimitRequest request);
    }
}
