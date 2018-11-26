using Braintree;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ValueFurniture.POCO_Classes
{
    /// <summary>
    /// Confidguration for Braintree
    /// </summary>
    /// <seealso cref="ValueFurniture.POCO_Classes.IBraintreeConfiguration" />
    public class BraintreeConfiguration : IBraintreeConfiguration
    {
        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        /// <value>
        /// The environment.
        /// </value>
        public string Environment { get; set; }

        /// <summary>
        /// Gets or sets the merchant identifier.
        /// </summary>
        /// <value>
        /// The merchant identifier.
        /// </value>
        public string MerchantId { get; set; }

        /// <summary>
        /// Gets or sets the public key.
        /// </summary>
        /// <value>
        /// The public key.
        /// </value>
        public string PublicKey { get; set; }


        /// <summary>
        /// Gets or sets the private key.
        /// </summary>
        /// <value>
        /// The private key.
        /// </value>
        public string PrivateKey { get; set; }

        /// <summary>
        /// Gets or sets the braintree gateway.
        /// </summary>
        /// <value>
        /// The braintree gateway.
        /// </value>
        private IBraintreeGateway BraintreeGateway { get; set; }


        /// <summary>
        /// Creates the gateway.
        /// </summary>
        /// <returns></returns>
        public IBraintreeGateway CreateGateway()
        {
            Environment = System.Environment.GetEnvironmentVariable("BraintreeEnvironment");
            MerchantId = System.Environment.GetEnvironmentVariable("BraintreeMerchantId");
            PublicKey = System.Environment.GetEnvironmentVariable("BraintreePublicKey");
            PrivateKey = System.Environment.GetEnvironmentVariable("BraintreePrivateKey");

            if (MerchantId == null || PublicKey == null || PrivateKey == null)
            {
                Environment = GetConfigurationSetting("BraintreeEnvironment");
                MerchantId = GetConfigurationSetting("BraintreeMerchantId");
                PublicKey = GetConfigurationSetting("BraintreePublicKey");
                PrivateKey = GetConfigurationSetting("BraintreePrivateKey");
            }

            return new BraintreeGateway(Environment, MerchantId, PublicKey, PrivateKey);
        }


        /// <summary>
        /// Gets the configuration settings.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns></returns>
        public string GetConfigurationSetting(string setting)
        {
            return ConfigurationManager.AppSettings[setting];
        }

        /// <summary>
        /// Gets the gateway.
        /// </summary>
        /// <returns></returns>
        public IBraintreeGateway GetGateway()
        {
            if (BraintreeGateway == null)
            {
                BraintreeGateway = CreateGateway();
            }

            return BraintreeGateway;
        }
    }
}