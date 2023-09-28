﻿using OpenQA.Selenium;
using UINavigator.Models.UIModels;

namespace UINavigator.Common.Contracts
{
    /// <summary>
    /// Navigation service.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        ///  Open web application based on URL location, and performs the login operation.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="location">Site login URL</param>
        /// <param name="isSSOUser">Flag to indicate if the user is SSO</param>
        Navigation Login(string username, string password, string location, bool isSSOUser);

        /// <summary>
        /// Navigate web application on specific route path.
        /// </summary>
        /// <param name="navigation"></param>
        /// <returns></returns>
        Navigation Path(UINavigation navigation);

        /// <summary>
        /// Selenium web driver.
        /// </summary>
        /// <returns></returns>
        IWebDriver WebDriver();
    }
}
