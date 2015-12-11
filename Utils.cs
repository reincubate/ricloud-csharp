using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Reincubate.ricloud {
    /// <summary>
    /// Helper class for utilities relating to iCloud data extraction
    /// </summary>
    public class Utils {
        /// <summary>
        /// Utility function to convert a Dictionary of string objects into a suitably formatted HTTP POST string
        /// </summary>
        /// <param name="postVariables">Dictionary of variables to encode</param>
        /// <returns>HTTP encoded string suitable for specifying as data for an HTTP POST request</returns>
        public static string dictionaryToPostString( Dictionary<string, string> postVariables ) {
            StringBuilder postString = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in postVariables) {
                if (pair.Key != null && pair.Key != "" && pair.Value != null && pair.Value != "" )
                    postString.Append(Uri.EscapeDataString(pair.Key)).Append("=").Append(Uri.EscapeDataString(pair.Value)).Append("&");
            }
            if ( postString.Length > 0 )
                postString.Length -= 1;
            return postString.ToString();
        }
    }
}
