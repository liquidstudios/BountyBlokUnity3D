using System.Collections.Generic;

namespace bountyblok.client
{
    /// <summary>
    /// Defines the options to use with the Bountyblok Client
    /// </summary>
    public class BountyblokClientOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BountyblokClientOptions"/> class.
        /// </summary>
        public BountyblokClientOptions()
        {
            this.RequestHeaders = new Dictionary<string, string>();
            this.Host = "https://api.bountyblok.io";
            this.Version = "v1";
        }

        /// <summary>
        /// Gets or sets the Bountyblok Client API key
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the request headers to use on HttpRequests sent to Bountyblok Client
        /// </summary>
        public Dictionary<string, string> RequestHeaders { get; set; }

        /// <summary>
        /// Gets or sets base url (e.g. https://api.bountyblok.io, this is the default)
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets API version, override AddVersion to customize
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the path to the API endpoint.
        /// </summary>
        public string UrlPath { get; set; }
    }
}
