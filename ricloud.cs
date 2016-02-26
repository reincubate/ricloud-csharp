using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Security.Cryptography.X509Certificates;

using Newtonsoft.Json;

namespace Reincubate.ricloud {
    /// <summary>
    /// Main iCloud Extractor API client class
    /// </summary>
    public class ricloud {

        #region Constants
        public const string ENDPOINT_LOGIN = "/c/sign-in/";
        public const string ENDPOINT_CHALLENGE_2FA = "/c/perform-2fa-challenge/";
        public const string ENDPOINT_SUBMIT_2FA = "/c/submit-2fa-challenge/";
        public const string ENDPOINT_DOWNLOAD_DATA = "/c/download-data/";
        public const string ENDPOINT_DOWNLOAD_FILE = "/c/download-file/";
        #endregion

        #region Public properties and associated private variables

        protected const string mAPIURLBase = "https://api.icloudextractor.com";
        /// <summary>
        /// Base URL for the Reincubate iCloud Extractor API
        /// </summary>
        public string APIURLBase {
            get { return mAPIURLBase; }
        }

        protected string mAPIUser = "120455";
        /// <summary>
        /// API username
        /// </summary>
        public string APIUser {
            get { return mAPIUser; }
            set { mAPIUser = value; }
        }

        protected string mAPIKey = "1413537101";
        /// <summary>
        /// API key
        /// </summary>
        public string APIKey {
            get { return mAPIKey; }
            set { mAPIKey = value; }
        }

        protected string mAppleID = "";
        /// <summary>
        /// Apple ID Email Address of the account you wish to access
        /// </summary>
        public string AppleID {
            get { return mAppleID; }
            set { mAppleID= value; }
        }

        protected string mPassword = "";
        /// <summary>
        /// Password for specified Apple ID
        /// </summary>
        public string Password {
            get { return mPassword; }
            set { mPassword = value; }
        }

        protected string mSessionKey = "";
        /// <summary>
        /// API session key - set on successful login and used to maintain user session
        /// </summary>
        public string SessionKey {
            get { return mSessionKey; }
            set { mSessionKey = value; }
        }

        protected dynamic[] mDevices;
        /// <summary>
        /// List of all iDevices associated with the Apple ID used to log in. Set automatically after login, will be null before then
        /// </summary>
        public dynamic[] Devices {
            get { return mDevices; }
            set { mDevices = value; }
        }

        protected dynamic[] mTrustedDevices;
        /// <summary>
        /// List of all trusted devices that can be used for 2FA authentication
        /// </summary>
        public dynamic[] TrustedDevices {
            get {
                return mTrustedDevices;
            }
            set {
                mTrustedDevices = value;
            }
        }
        #endregion

        /// <summary>
        /// Default constructor. Not normally used
        /// </summary>
        public ricloud() {}

        /// <summary>
        /// ricloud constructor - instantiates API object with specified API credentials
        /// </summary>
        /// <param name="mUser">API username</param>
        /// <param name="mKey">Matching API key</param>
        public ricloud(string mUser, string mKey) {
            APIUser = mUser;
            APIKey = mKey;
        }

        /// <summary>
        /// Log into the specified iCloud account and fetch the list of devices associated with that account
        /// </summary>
        /// <param name="mUsername">Apple ID to log in as</param>
        /// <param name="mPassword">Apple ID password</param>
        public void Login(string mUsername, string mPassword) {
            Dictionary<string, string> mData = new Dictionary<string, string>();
            mData.Add("email", mUsername);
            mData.Add("password", mPassword);
            if (SessionKey != "") {
                mData.Add("key", SessionKey);
            }

            dynamic jsonObj = MakeRequest(ENDPOINT_LOGIN, mData);
            if ( jsonObj != null ) {
                // Check for 2FA required
                if (jsonObj.error != null && jsonObj.error.Value == "2fa-required") {
                    SessionKey = jsonObj.data.key;
                    List<dynamic> mTrustedDevices = new List<dynamic>();
                    foreach (dynamic trustedDeviceObj in jsonObj.data.trustedDevices) {
                        mTrustedDevices.Add(trustedDeviceObj);
                    }
                    // Populate list of trusted devices which can be used for two-factor authentication
                    TrustedDevices = mTrustedDevices.ToArray<dynamic>();
                    // Raise an exception so the calling program can properly handle the need for 2FA authentication
                    throw new TwoFactorAuthenticationRequiredException();
                } else {
                    // Successfully logged in, set session key
                    SessionKey = jsonObj.key;
                    List<Device> mDevices = new List<Device>();
                    // Build list of devices from returned JSON
                    foreach (dynamic deviceObj in jsonObj.devices) {
                        Device mDevice = JsonConvert.DeserializeObject<Device>(deviceObj.Value.ToString());
                        mDevice.Id = deviceObj.Name;
                        mDevices.Add(mDevice);
                    }
                    Devices = mDevices.ToArray<Device>();
                    // Clear the Apple ID details from memory, we don't need them any more
                    AppleID = "";
                    Password = "";
                }
            }
        }

        public void Request2FAChallenge(dynamic challengeDevice) {
            Dictionary<string, string> mData = new Dictionary<string, string>();
            mData.Add("challenge", challengeDevice.Value);
            if (SessionKey != "") {
                mData.Add("key", SessionKey);
            }

            dynamic jsonObj = MakeRequest(ENDPOINT_CHALLENGE_2FA, mData);
        }

        public void Submit2FAChallenge( string mCode ) {
            Dictionary<string, string> mData = new Dictionary<string, string>();
            mData.Add("code", mCode);
            if (SessionKey != "") {
                mData.Add("key", SessionKey);
            }

            dynamic jsonObj = MakeRequest(ENDPOINT_SUBMIT_2FA, mData);
            //Login(AppleID, Password);
        }


        /// <summary>
        /// Builds an HTTP request to the Reincubate iCloud Extractor API. This function should not normally need to be called directly
        /// </summary>
        /// <param name="mEndpoint">API Endpoint URL to query</param>
        /// <param name="mData">Post data to pass to the API</param>
        /// <returns>JSON object containing the API response</returns>
        public dynamic MakeRequest( string mEndpoint, Dictionary<string, string> mData ) {
            return MakeRequest(mEndpoint, mData, false);
        }

        /// <summary>
        /// Builds an HTTP request to the Reincubate iCloud Extractor API. This function should not normally need to be called directly
        /// </summary>
        /// <param name="mEndpoint">API Endpoint URL to query</param>
        /// <param name="mData">Post data to pass to the API</param>
        /// <param name="isBinary">Return data as a byte array rather than a JSON object</param>
        /// <returns>JSON object or byte array containing the API response</returns>
        public dynamic MakeRequest(string mEndpoint, Dictionary<string,string> mData, bool isBinary) {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(APIURLBase + mEndpoint);

            var webClient = new WebClient();

            // Since SSL certificate won't be trusted, work round SSL/TLS error. Ref: http://stackoverflow.com/questions/536352/webclient-https-issues
            ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(bypassAllCertificateStuff);

            // create credentials, base64 encode of username:password
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(APIUser + ":" + APIKey));

            // Inject this string as the Authorization header
            webRequest.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", credentials);
            webRequest.Accept = "application/vnd.icloud-api.v1";
            webRequest.Method = "POST";

            string mPayload = Utils.dictionaryToPostString(mData);
            // turn our request string into a byte stream
            byte[] postBytes = Encoding.UTF8.GetBytes(mPayload);

            // this is important - make sure you specify type this way
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = postBytes.Length;
            // Nice long timeout as the response could take a while if lots of data has been requested
            webRequest.Timeout = 180000;
            Stream requestStream = webRequest.GetRequestStream();

            // now send it
            requestStream.Write(postBytes, 0, postBytes.Length);
            requestStream.Close();

            try {
                // grab the response
                using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse()) {
                    string result;
                    if (isBinary) {
                        if (response.StatusCode == HttpStatusCode.OK) {
                            Stream stream = response.GetResponseStream();
                            MemoryStream memStream = new MemoryStream();
                            byte[] buffer = new byte[1024];
                            int byteCount;
                            do {
                                byteCount = stream.Read(buffer, 0, buffer.Length);
                                memStream.Write(buffer, 0, byteCount);
                            } while (byteCount > 0);
                            memStream.Seek(0, SeekOrigin.Begin);
                            byte[] byteData = memStream.ToArray();
                            return byteData;
                        } else {
                            throw new WebException("Invalid HTTP response. Status code: " + response.StatusCode.ToString());
                        }
                    } else {
                        using (StreamReader rdr = new StreamReader(response.GetResponseStream())) {
                            result = rdr.ReadToEnd();
                        }
                        if (response.StatusCode == HttpStatusCode.OK) {
                            // Parse result string into JSON
                            dynamic jsonObj = JsonConvert.DeserializeObject(result);
                            return jsonObj;
                        } else if (response.StatusCode == HttpStatusCode.Conflict) {
                            // Parse result string into JSON
                            dynamic jsonObj = JsonConvert.DeserializeObject(result);
                            // Check for HTTP 409 status, could indicate 2FA required
                            if (response.StatusCode == HttpStatusCode.Conflict) {
                                return jsonObj;
                            }
                        } else {
                            throw new WebException("Invalid HTTP response. Status code: " + response.StatusCode.ToString());
                        }
                    }
                }
            } catch (WebException we) {
                if (we.Response != null && ((HttpWebResponse)we.Response).StatusCode == HttpStatusCode.Conflict) {
                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    using (StreamReader rdr = new StreamReader(response.GetResponseStream())) {
                        string result = rdr.ReadToEnd();
                        dynamic jsonObj = JsonConvert.DeserializeObject(result);
                        return jsonObj;
                    }
                }
            }
            return null;
        }

        private static bool bypassAllCertificateStuff(object sender, X509Certificate cert, X509Chain chain, System.Net.Security.SslPolicyErrors error) {
            return true;
        }


    }
}
