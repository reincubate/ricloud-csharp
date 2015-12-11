using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace Reincubate.ricloud {
    public class LoginJson {
        public LoginJson() {
        }

        protected string _key;
        [JsonProperty(PropertyName="key")]
        public string Key {
            get {
                return _key;
            }
            set {
                _key = value;
            }
        }

        protected Device[] _devices;
        [JsonProperty(PropertyName="devices")]
        public Device[] Devices {
            get {
                return _devices;
            }
            set {
                _devices = value;
            }
        }
    }
}
