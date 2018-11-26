using Braintree;

namespace ValueFurniture.POCO_Classes
{
    /// <summary>
    /// Interface for Briantree Configuration.
    /// </summary>
    public interface IBraintreeConfiguration
    {
        /// <summary>
        /// Creates the gateway.
        /// </summary>
        /// <returns></returns>
        IBraintreeGateway CreateGateway();

        /// <summary>
        /// Gets the configuration setting.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns></returns>
        string GetConfigurationSetting(string setting);

        /// <summary>
        /// Gets the gateway.
        /// </summary>
        /// <returns></returns>
        IBraintreeGateway GetGateway();
    }
}