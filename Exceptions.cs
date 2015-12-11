using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reincubate.ricloud {
    /// <summary>
    /// Exception thrown when a login attempt is made on an Apple ID where two-factor authentication (2FA) is enabled
    /// </summary>
    public class TwoFactorAuthenticationRequiredException: Exception {
        public TwoFactorAuthenticationRequiredException() {
        }

        public TwoFactorAuthenticationRequiredException( string message ) : base(message) {
        }

        public TwoFactorAuthenticationRequiredException( string message, Exception inner )
            : base(message, inner) {
        }

    }

    /// <summary>
    /// Exception thrown when an API request is made for an Apple ID that has not been successfully logged in
    /// </summary>
    public class LoginRequiredException: Exception {
        public LoginRequiredException() {}
        public LoginRequiredException( string message ) : base(message) {}
        public LoginRequiredException( string message, Exception inner ) : base(message, inner) {
        }
    }
}
