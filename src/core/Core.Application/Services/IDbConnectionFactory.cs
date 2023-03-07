using System.Data;

namespace Core.Application.Services
{
    public interface IDbConnectionFactory : IDisposable
    {
        string ApplicationName { get; }
        IDbConnection GetOpenConnection();
    }
}
