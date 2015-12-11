using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace Reincubate.ricloud {
    /// <summary>
    /// Class to encapsulate a specific iDevice. NB - can be serialised/deserialised using JSON.NET
    /// </summary>
    public class Device {
        public Device() {
        }

        public Device( string mId, string mModel, string mDeviceName, string mColour, string mName, DateTime mLatestBackup ) {
            _id = mId;
            _model = mModel;
            _deviceName = mDeviceName;
            _colour = mColour;
            _name = mName;
            _latestBackup = mLatestBackup;
        }

        protected string _id;
        /// <summary>
        /// Unique Device ID (UDID)
        /// </summary>
        [JsonProperty(PropertyName = "")]
        public string Id {
            get {
                return _id;
            }
            set {
                _id = value;
            }
        }

        protected string _model;
        /// <summary>
        /// Apple Model ID for device (e.g J98aAP is an iPad Pro)
        /// </summary>
        [JsonProperty(PropertyName = "model")]
        public string Model {
            get {
                return _model;
            }
            set {
                _model = value;
            }
        }

        protected string _deviceName;
        /// <summary>
        /// Friendly name of device
        /// </summary>
        [JsonProperty(PropertyName = "device_name")]
        public string DeviceName {
            get {
                return _deviceName;
            }
            set {
                _deviceName = value;
            }
        }

        protected string _colour;
        /// <summary>
        /// Colour of device
        /// </summary>
        [JsonProperty(PropertyName = "colour")]
        public string Colour {
            get {
                return _colour;
            }
            set {
                _colour = value;
            }
        }

        protected string _name;
        /// <summary>
        /// Name of device model (e.g iPhone 6S or iPad Pro)
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name {
            get {
                return _name;
            }
            set {
                _name = value;
            }
        }

        protected DateTime _latestBackup;
        /// <summary>
        /// Date and time of the most recent backup for this device
        /// </summary>
        [JsonProperty(PropertyName = "latest-backup")]
        public DateTime LatestBackup {
            get {
                return _latestBackup;
            }
            set {
                _latestBackup = value;
            }
        }

    }
}
