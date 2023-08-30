
namespace UINavigator.Common.Contracts
{
    /// <summary>
    /// Customer selector service for SSO user.
    /// </summary>
    public interface ICustomerSelectorService
    {
        /// <summary>
        /// Selects the customer by code after SSO user login.
        /// </summary>
        /// <param name="customerCode">Company code</param>
        void SelectCustomerCodeWithComponent(string customerCode);
    }
}
