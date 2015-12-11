using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;

namespace Reincubate.ricloud {
    /// <summary>
    /// Class to handle API functions relating to iCloud backup data
    /// </summary>
    public class BackupClient {
        public const int DATA_SMS = 1;
        public const int DATA_PHOTOS = 2;
        public const int DATA_BROWSER_HISTORY = 4;
        public const int DATA_CALL_HISTORY = 8;
        public const int DATA_CONTACTS = 16;
        public const int DATA_INSTALLED_APPS = 32;
        public const int DATA_WHATSAPP_MESSAGES = 512;
        public const int DATA_SKYPE_MESSAGES = 1024;
        public const int DATA_APPOINTMENTS = 2048;
        public const int DATA_LINE_MESSAGES = 4096;
        public const int DATA_KIK_MESSAGES = 8192;
        public const int DATA_VIBER_MESSAGES = 16384;

        public const int DATA_WEB_CONTACTS = 64;
        public const int DATA_WEB_LOCATION = 256;

        /// <summary>
        /// Dictionary containing all the types of data that can be accessed via the Reincubate iCloud Extractor API. This can be used to construct a bit mask which is passed to the RequestData function to specify what types of data to return
        /// </summary>
        public static Dictionary<int, string> AVAILABLE_DATA = new Dictionary<int, string>{
                {DATA_SMS              , "SMS Messages"},
                {DATA_PHOTOS           , "Photos and Videos"},
                {DATA_BROWSER_HISTORY  , "Browser History"},
                {DATA_CALL_HISTORY     , "Call History"},
                {DATA_CONTACTS         , "Contacts"},
                {DATA_INSTALLED_APPS   , "Installed Apps"},
                {DATA_WHATSAPP_MESSAGES, "WhatsApp messages"},
                {DATA_SKYPE_MESSAGES   , "Skype messages"},
                {DATA_APPOINTMENTS   , "Appointments"},
                {DATA_LINE_MESSAGES, "Line messages"},
                {DATA_KIK_MESSAGES   , "Kik messages"},
                {DATA_VIBER_MESSAGES   , "Viber messages"},

                {DATA_WEB_CONTACTS     , "Contacts (live)"},
                {DATA_WEB_LOCATION     , "Location (live)"}
        };

        protected static DateTime MIN_REQUEST_DATE = new DateTime(1900, 1, 1);

        protected ricloud mRICloud;
        /// <summary>
        /// Instance of a ricloud object containing APi session information associated with this backup client object
        /// </summary>
        public ricloud RICloud {
            get {
                return mRICloud;
            }
            set {
                mRICloud = value;
            }
        }

        /// <summary>
        /// Default constructor, not normally used
        /// </summary>
        public BackupClient() {
        }

        /// <summary>
        /// Instantiate a new instance of the BackupClient object based on an existing ricloud object
        /// </summary>
        /// <param name="objRICloud">An instance of a ricloud object representing a particular API session</param>
        public BackupClient( ricloud objRICloud ) {
            mRICloud = objRICloud;
        }

        /// <summary>
        /// Fetches specified data items via the Reincubate iCloud Extractor API - any time period
        /// </summary>
        /// <param name="mDeviceId">ID of the device to fetch data for</param>
        /// <param name="mDataMask">Data mask specifying what types of data to return</param>
        /// <returns></returns>
        public dynamic RequestData( string mDeviceId, int mDataMask ) {
            return RequestData(mDeviceId, mDataMask, MIN_REQUEST_DATE);
        }

        /// <summary>
        /// Fetches specified data items via the Reincubate iCloud Extractor API for a specified time period
        /// </summary>
        /// <param name="mDeviceId">ID of the device to fetch data for</param>
        /// <param name="mDataMask">Data mask specifying what types of data to return</param>
        /// <param name="mSince">Fetch all data since the specified date</param>
        /// <returns></returns>
        public dynamic RequestData(string mDeviceId, int mDataMask, DateTime mSince) {
            if ( RICloud == null || RICloud.SessionKey == "" )
                throw new LoginRequiredException("Invalid or missing API session key");

            // Check data mask, if 0 or -1 make it everything
            if (mDataMask == -1 || mDataMask == 0) {
                mDataMask = 0;
                foreach (int maskValue in BackupClient.AVAILABLE_DATA.Keys) {
                    mDataMask += maskValue;
                }
            }

            // Ensure mSince is at least the minimum valid request date
            if (mSince <= BackupClient.MIN_REQUEST_DATE)
                mSince = BackupClient.MIN_REQUEST_DATE;

            Dictionary<string, string> mData = new Dictionary<string, string>();
            mData.Add("key", RICloud.SessionKey);
            mData.Add("mask", mDataMask.ToString());
            mData.Add("since", mSince.ToString("yyyy-MM-dd HH:mm:ss.ff"));
            mData.Add("device", mDeviceId);

            dynamic jsonObj = RICloud.MakeRequest(ricloud.ENDPOINT_DOWNLOAD_DATA, mData);

            return jsonObj;
        }

        public byte[] DownloadFile( string mDeviceId, string mFileId ) {
            Dictionary<string, string> mData = new Dictionary<string, string>();
            mData.Add("key", RICloud.SessionKey);
            mData.Add("device", mDeviceId);
            mData.Add("file", mFileId);
            dynamic fileData = RICloud.MakeRequest(ricloud.ENDPOINT_DOWNLOAD_FILE, mData, true);
            // Check that we really do have binary data returned
            if (fileData is byte[]) {
                return fileData;
            }
            return null;
        }
    }

}
