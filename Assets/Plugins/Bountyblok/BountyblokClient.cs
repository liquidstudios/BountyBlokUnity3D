using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bountyblok.client
{
    /// <summary>
    /// A HTTP client wrapper for interacting bountyblok.io API
    /// </summary>
    public class BountyblokClient : IBountyblokClient
    {
        private const string Scheme = "Bearer";
        private const string ContentType = "Content-Type";
        private const string DefaultMediaType = "application/json";

        /// <summary>
        /// The <see cref="BountyblokClient"/> assembly version to send in request User-Agent header
        /// </summary>
        private static readonly string ClientVersion = typeof(BountyblokClient).GetTypeInfo().Assembly.GetName().Version.ToString();

        /// <summary>
        /// The default configuration settings to use with the Bountyblok Client
        /// </summary>
        private static readonly BountyblokClientOptions DefaultOptions = new BountyblokClientOptions();

        /// <summary>
        /// The configuration to use with current <see cref="BountyblokClient"/> instance
        /// </summary>
        private readonly BountyblokClientOptions options;

        /// <summary>
        /// The HttpClient instance to use for all calls from this BountyblokClient instance.
        /// </summary>
        private readonly HttpClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="BountyblokClient"/> class.
        /// </summary>
        /// <param name="webProxy">Web proxy.</param>
        /// <param name="apiKey">Your bountyblok API key.</param>
        /// <param name="host">Base url (e.g. https://api.bountyblok.io)</param>
        /// <param name="requestHeaders">A dictionary of request headers</param>
        /// <param name="version">API version, override AddVersion to customize</param>
        /// <param name="urlPath">Path to endpoint (e.g. /path/to/endpoint)</param>
        /// <returns>Interface to the bountyblok REST API</returns>
        public BountyblokClient(IWebProxy webProxy, string apiKey, string host = null, Dictionary<string, string> requestHeaders = null, string version = "v1", string urlPath = null)
            : this(CreateHttpClientWithWebProxy(webProxy), new BountyblokClientOptions { ApiKey = apiKey, Host = host, RequestHeaders = requestHeaders, Version = version, UrlPath = urlPath })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BountyblokClient"/> class.
        /// </summary>
        /// <param name="options">A <see cref="BountyblokClientOptions"/> instance that defines the configuration settings to use with the client </param>
        /// <returns>Interface to the bountyblok REST API</returns>
        public BountyblokClient(BountyblokClientOptions options)
            : this(null, options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BountyblokClient"/> class.
        /// </summary>
        /// <param name="httpClient">An optional http client which may me injected in order to facilitate testing.</param>
        /// <param name="apiKey">Your bountyblok API key.</param>
        /// <param name="host">Base url (e.g. https://api.bountyblok.io)</param>
        /// <param name="requestHeaders">A dictionary of request headers</param>
        /// <param name="version">API version, override AddVersion to customize</param>
        /// <param name="urlPath">Path to endpoint (e.g. /path/to/endpoint)</param>
        /// <returns>Interface to the bountyblok REST API</returns>
        public BountyblokClient(HttpClient httpClient, string apiKey, string host = null, Dictionary<string, string> requestHeaders = null, string version = "v1", string urlPath = null)
            : this(httpClient, new BountyblokClientOptions { ApiKey = apiKey, Host = host, RequestHeaders = requestHeaders, Version = version, UrlPath = urlPath })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BountyblokClient"/> class.
        /// </summary>
        /// <param name="apiKey">Your bountyblok API key.</param>
        /// <param name="host">Base url (e.g. https://api.bountyblok.io)</param>
        /// <param name="requestHeaders">A dictionary of request headers</param>
        /// <param name="version">API version, override AddVersion to customize</param>
        /// <param name="urlPath">Path to endpoint (e.g. /path/to/endpoint)</param>
        /// <returns>Interface to the bountyblok REST API</returns>
        public BountyblokClient(string apiKey, string host = null, Dictionary<string, string> requestHeaders = null, string version = "v1", string urlPath = null)
            : this(null, new BountyblokClientOptions { ApiKey = apiKey, Host = host, RequestHeaders = requestHeaders, Version = version, UrlPath = urlPath })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BountyblokClient"/> class.
        /// </summary>
        /// <param name="httpClient">An optional HTTP client which may me injected in order to facilitate testing.</param>
        /// <param name="options">A <see cref="BountyblokClientOptions"/> instance that defines the configuration settings to use with the client </param>
        /// <returns>Interface to the bountyblok REST API</returns>
        internal BountyblokClient(HttpClient httpClient, BountyblokClientOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.client = httpClient ?? CreateHttpClientWithRetryHandler();
            if (this.options.RequestHeaders != null && this.options.RequestHeaders.TryGetValue(ContentType, out var contentType))
            {
                this.MediaType = contentType;
            }
        }

        /// <summary>
        /// The supported API methods.
        /// </summary>
        public enum Method
        {
            /// <summary>
            /// Get a resource.
            /// </summary>
            GET,

            /// <summary>
            /// Create a resource or execute a function. (e.g send an email)
            /// </summary>
            POST,
        }

        /// <summary>
        /// Gets or sets the path to the API resource.
        /// </summary>
        public string UrlPath
        {
            get => this.options.UrlPath;
            set => this.options.UrlPath = value;
        }

        /// <summary>
        /// Gets or sets the API version.
        /// </summary>
        public string Version
        {
            get => this.options.Version;
            set => this.options.Version = value;
        }

        /// <summary>
        /// Gets or sets the request media type.
        /// </summary>
        public string MediaType { get; set; } = DefaultMediaType;

        /// <summary>
        /// Add the authorization header, override to customize
        /// </summary>
        /// <param name="header">Authorization header</param>
        /// <returns>Authorization value to add to the header</returns>
        public virtual AuthenticationHeaderValue AddAuthorization(KeyValuePair<string, string> header)
        {
            string[] split = header.Value.Split();
            return new AuthenticationHeaderValue(split[0], split[1]);
        }

        /// <summary>
        /// Make the call to the API server, override for testing or customization
        /// </summary>
        /// <param name="request">The parameters for the API call</param>
        /// <param name="cancellationToken">Cancel the asynchronous call</param>
        /// <returns>Response object</returns>
        public virtual async Task<BountyblokResponse> MakeRequest(HttpRequestMessage request, CancellationToken cancellationToken = default(CancellationToken))
        {
            HttpResponseMessage response = await this.client.SendAsync(request, cancellationToken).ConfigureAwait(false);
            return new BountyblokResponse(response.StatusCode, response.Content, response.Headers);
        }

        /// <summary>
        /// Prepare for async call to the API server
        /// </summary>
        /// <param name="method">HTTP verb</param>
        /// <param name="requestBody">JSON formatted string</param>
        /// <param name="queryParams">JSON formatted query parameters</param>
        /// <param name="urlPath">The path to the API endpoint.</param>
        /// <param name="cancellationToken">Cancel the asynchronous call.</param>
        /// <returns>Response object</returns>
        /// <exception cref="Exception">The method will NOT catch and swallow exceptions generated by sending a request
        /// through the internal HTTP client. Any underlying exception will pass right through.
        /// In particular, this means that you may expect
        /// a TimeoutException if you are not connected to the Internet.</exception>
        public async Task<BountyblokResponse> RequestAsync(
            BountyblokClient.Method method,
            string requestBody = null,
            string queryParams = null,
            string urlPath = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var baseAddress = new Uri(string.IsNullOrWhiteSpace(this.options.Host) ? DefaultOptions.Host : this.options.Host);
            if (!baseAddress.OriginalString.EndsWith("/"))
            {
                baseAddress = new Uri(baseAddress.OriginalString + "/");
            }

            // Build the final request
            var request = new HttpRequestMessage
            {
                Method = new HttpMethod(method.ToString()),
                RequestUri = new Uri(baseAddress, this.BuildUrl(urlPath, queryParams)),
                Content = requestBody == null ? null : new StringContent(requestBody, Encoding.UTF8, this.MediaType)
            };

            // set header overrides
            if (this.options.RequestHeaders?.Count > 0)
            {
                foreach (var header in this.options.RequestHeaders)
                {
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            // set standard headers
            request.Headers.Authorization = new AuthenticationHeaderValue(Scheme, this.options.ApiKey);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(this.MediaType));
            request.Headers.UserAgent.TryParseAdd($"bountyblok/{ClientVersion} csharp");
            return await this.MakeRequest(request, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Make a request to log a task by Challenge ID through bountyblok asynchronously
        /// </summary>
        /// <param name="request">A LogTaskRequest object with the details for the request.</param>
        /// <param name="cancellationToken">Cancel the asynchronous call.</param>
        /// <returns>A Response object.</returns>
        public async Task<BountyblokResponse> LogTaskAsync(LogTaskRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.RequestAsync(
                Method.POST,
                JsonConvert.SerializeObject(request),
                urlPath: "log_task",
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Make a request to log a task by App ID through bountyblok asynchronously
        /// </summary>
        /// <param name="request">A LogAppRequest object with the details for the request.</param>
        /// <param name="cancellationToken">Cancel the asynchronous call.</param>
        /// <returns>A Response object.</returns>
        public async Task<BountyblokResponse> LogAppAsync(LogAppRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.RequestAsync(
                Method.POST,
                JsonConvert.SerializeObject(request),
                urlPath: "log_app",
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Make a request to log a task by App ID through bountyblok asynchronously
        /// </summary>
        /// <param name="request">A LogAppRequest object with the details for the request.</param>
        /// <param name="cancellationToken">Cancel the asynchronous call.</param>
        /// <returns>A Response object.</returns>
        public async Task<GetChallengeResponse> GetChallengeAsync(GetChallengeRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await this.RequestAsync(
                Method.GET,
                queryParams: JsonConvert.SerializeObject(request),
                urlPath: "get_challenge_progress",
                cancellationToken: cancellationToken).ConfigureAwait(false);

            return JsonConvert.DeserializeObject<GetChallengeResponse>(response.Body.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Create client with WebProxy if set
        /// </summary>
        /// <param name="webProxy">the WebProxy</param>
        /// <returns>HttpClient with RetryDelegatingHandler and WebProxy if set</returns>
        private static HttpClient CreateHttpClientWithWebProxy(IWebProxy webProxy)
        {
            if (webProxy != null)
            {
                var httpClientHandler = new HttpClientHandler()
                {
                    Proxy = webProxy,
                    PreAuthenticate = true,
                    UseDefaultCredentials = false,
                };

                return new HttpClient(httpClientHandler);
            }

            return null;
        }

        private static HttpClient CreateHttpClientWithRetryHandler()
        {
            return new HttpClient();
        }

        /// <summary>
        /// Build the final URL
        /// </summary>
        /// <param name="urlPath">The URL path.</param>
        /// <param name="queryParams">A string of JSON formatted query parameters (e.g {'param': 'param_value'})</param>
        /// <returns>
        /// Final URL
        /// </returns>
        private string BuildUrl(string urlPath, string queryParams = null)
        {
            string url = null;

            // create urlPAth - from parameter if overridden on call or from constructor parameter
            var urlpath = urlPath ?? this.options.UrlPath;

            if (this.options.Version != null)
            {
                url = this.options.Version + "/" + urlpath;
            }
            else
            {
                url = urlpath;
            }

            if (queryParams != null)
            {
                var ds_query_params = this.ParseJson(queryParams);
                string query = "?";
                foreach (var pair in ds_query_params)
                {
                    foreach (var element in pair.Value)
                    {
                        if (query != "?")
                        {
                            query += "&";
                        }

                        query = query + pair.Key + "=" + element;
                    }
                }

                url += query;
            }

            return url;
        }

        /// <summary>
        /// Parses a JSON string without removing duplicate keys.
        /// </summary>
        /// <remarks>
        /// This function flattens all Objects/Array.
        /// This means that for example <code>{'id': 1, 'id': 2, 'id': 3}</code> and
        /// <code>{'id': [1, 2, 3]}</code> result in the same output.
        /// </remarks>
        /// <param name="json">The JSON string to parse.</param>
        /// <returns>A dictionary of all values.</returns>
        private Dictionary<string, List<object>> ParseJson(string json)
        {
            var dict = new Dictionary<string, List<object>>();

            using (var sr = new StringReader(json))
            using (var reader = new JsonTextReader(sr))
            {
                var propertyName = string.Empty;
                while (reader.Read())
                {
                    switch (reader.TokenType)
                    {
                        case JsonToken.PropertyName:
                            {
                                propertyName = reader.Value.ToString();
                                if (!dict.ContainsKey(propertyName))
                                {
                                    dict.Add(propertyName, new List<object>());
                                }

                                break;
                            }

                        case JsonToken.Boolean:
                        case JsonToken.Integer:
                        case JsonToken.Float:
                        case JsonToken.Bytes:
                        case JsonToken.String:
                        case JsonToken.Date:
                            {
                                dict[propertyName].Add(reader.Value);
                                break;
                            }
                    }
                }
            }

            return dict;
        }
    }
}
