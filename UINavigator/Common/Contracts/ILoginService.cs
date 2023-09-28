
namespace UINavigator.Common.Contracts
{
    /// <summary>
    /// Site login service
    /// </summary>
    public interface ILoginService
    {
        /// <summary>
        /// Open web application based on URL location.
        /// </summary>
        /// <param name="location"></param>
        void OpenBrowser(string location);

        /// <summary>
        ///  Open web application based on URL location, and performs the login operation. 
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="location">Site login URL</param>
        /// <param name="isSSOUser">Flag to indicate if the user is SSO</param>
        void OpenBrowserAndLogin(string username, string password, string location, bool isSSOUser);
    }
}
