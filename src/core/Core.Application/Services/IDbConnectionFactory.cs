using System.Data;

namespace Core.Application.Services
{
    public interface IDbConnectionFactory : IDisposable
    {
        IDbConnection GetOpenConnection();
    }
}
