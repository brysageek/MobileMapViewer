using System.Threading.Tasks;

namespace MobileMapViewer.Services
{
    /// <summary>
    /// Authorization Service to Authenticate
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> Authenticate(string username, string password);

        /// <summary>
        /// 
        /// </summary>
        void RemoveAuthentication();

    }
}
