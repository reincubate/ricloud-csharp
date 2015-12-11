using System;
using System.Collections.Generic;
using System.IO;

using Reincubate.ricloud;

namespace ricloudDemo {
    class Program {

        static string API_KEY_USERNAME = "120455";
        static string API_KEY_PASSWORD = "1413537101";

        /// <summary>
        /// Main program execution function. Call with "/?" argument to see help
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args) {
            switch (args.Length) {
                case 0:
                    doProgram();
                    break;
                case 1:
                    if (args[0] == "/?") {
                        doHelp();
                    }
                    break;
                case 2:
                    doProgram(args[0], args[1]);
                    break;
            }
        }

        /// <summary>
        /// Display help to console
        /// </summary>
        static void doHelp() {
            Console.WriteLine("ricloudDemo demonstration application");
            Console.WriteLine("Usage:\n");
            Console.WriteLine("riclouddemo <email> <password>\n");
            Console.WriteLine("<email> is your iCloud account email address");
            Console.WriteLine("<password> is your iCloud account password");
        }

        /// <summary>
        /// Function to log into the ricloud API after asking for an Apple ID and password
        /// </summary>
        static void doProgram() {
            Console.WriteLine("Enter Apple ID:");
            string mUsername = Console.ReadLine();
            Console.WriteLine("Enter password for Apple ID");
            string mPassword = Console.ReadLine();
            doProgram(mUsername, mPassword);
        }

        /// <summary>
        /// Function to log into the ricloud API for a specified Apple ID and password, fetch a list of devices and display data for the selected device
        /// </summary>
        /// <param name="mEmail">Email address of the Apple ID to access</param>
        /// <param name="mPassword">Password for the aformentioned Apple ID</param>
        static void doProgram(string mEmail, string mPassword) {
            // Instantiate ricloud object
            ricloud mClient = new ricloud(API_KEY_USERNAME, API_KEY_PASSWORD);

            // Log in with specified Apple ID username and password
            try {
                mClient.Login(mEmail, mPassword);
            } catch (TwoFactorAuthenticationRequiredException te) {
                // The Apple ID has two-factor authentication enabled. Handle this by specifying a trusted device to send a 2FA code to and then passing that code back to the API
                dynamic mTrustedDevice = ChooseTrustedDevice(mClient.TrustedDevices);
                mClient.Request2FAChallenge(mTrustedDevice);
                string mCode = Get2FACode();
                mClient.Submit2FAChallenge(mCode);
                mClient.Login(mEmail, mPassword);
            }

            string mDeviceId = ChooseDevice(mClient.Devices);

            // Choose the data to extract from the specified device. Use the bitwise flags in the BackupClient class to define this.
            Console.WriteLine("Data to extract:");
            foreach (KeyValuePair<int,string> kvpData in BackupClient.AVAILABLE_DATA) {
                Console.WriteLine("{0} - {1}", kvpData.Key, kvpData.Value);
            }
            Console.WriteLine("Choose the data mask you want (use 0 for everything):");
            string mMaskString = Console.ReadLine();
            int mMask = -1;
            int.TryParse(mMaskString, out mMask);

            // Instantiate backup client
            BackupClient mBackupClient = new BackupClient(mClient);

            // Use backup client to fetch specified data for chosen device
            dynamic json = mBackupClient.RequestData(mDeviceId, mMask);

            // Create folder to dump results into
            DirectoryInfo newDir = Directory.CreateDirectory(Path.Combine(System.Environment.CurrentDirectory, "out"));

            // Dump JSON
            File.WriteAllText(Path.Combine(newDir.FullName, "data.json"), json.ToString());

            // If we are getting photos
            if ((mMask & BackupClient.DATA_PHOTOS) == BackupClient.DATA_PHOTOS) {
                // Loop through photos
                string picfilePath;
                byte[] picfileData;
                foreach (dynamic photo in json.photos) {
                    picfilePath = Path.Combine(newDir.FullName, photo.filename.ToString());
                    picfileData = mBackupClient.DownloadFile(mDeviceId, photo.file_id.ToString());
                    if (picfileData != null) {
                        File.WriteAllBytes(picfilePath, picfileData);
                    }

                }
            }

            Console.WriteLine("Complete! All data written to the directory " + newDir.FullName);

        }

        /// <summary>
        /// Helper function to display a list of trusted devices (for two-factor authentication) and allow the user to choose a device to send the 2FA code to
        /// </summary>
        /// <param name="mTrustedDevices">JSON object containing the list of trusted devices</param>
        /// <returns>The trusted device chosen by the user to which the 2FA code will be sent</returns>
        static dynamic ChooseTrustedDevice( dynamic mTrustedDevices ) {
            Console.WriteLine("2FA has been enabled. Please choose a trusted device from the list below");
            int mDeviceIndex = 0;
            Dictionary<string, dynamic> mDevices = new Dictionary<string, dynamic>();
            foreach (dynamic trustedDevice in mTrustedDevices) {
                mDevices.Add(mDeviceIndex.ToString(), trustedDevice);
                Console.WriteLine("{0} - {1}", mDeviceIndex, trustedDevice.Value);
                mDeviceIndex++;
            }
            Console.WriteLine("Choose a device by entering it's index (e.g 0)");
            string mIndexString = Console.ReadLine();
            return mDevices[mIndexString];
        }

        /// <summary>
        /// Helper function to display a list of devices associated with an Apple ID and allow the user to choose one of the devices
        /// </summary>
        /// <param name="mDevices">JSON object containing the list of associated devices</param>
        /// <returns>The specific device chosen by the user</returns>
        static string ChooseDevice(dynamic mDevices) {
            // List devices
            Console.WriteLine("Your devices:");
            int mDeviceIndex = 0;
            Dictionary<string, string> mDeviceIDs = new Dictionary<string, string>();
            foreach (Device mDevice in mDevices) {
                mDeviceIDs.Add(mDeviceIndex.ToString(), mDevice.Id);
                Console.WriteLine("({0}) - {1} (model: {2}, colour: {3}, latest-backup: {4})", mDeviceIndex, mDevice.Name, mDevice.Model, mDevice.Colour, mDevice.LatestBackup);
                mDeviceIndex++;
            }

            // Choose a device
            Console.WriteLine("Choose a device by entering it's index");
            string mIndexString = Console.ReadLine();
            string mDeviceId = "";
            mDeviceId = mDeviceIDs[mIndexString];
            return mDeviceId;
        }

        /// <summary>
        /// Helper function to allow the user to enter a 2FA code on the command line
        /// </summary>
        /// <returns>2FA code entered by the user</returns>
        static string Get2FACode() {
            Console.WriteLine("A 2FA code has been sent to your device. Please enter it below.");
            Console.Write("Code: ");
            string mCode = Console.ReadLine();
            return mCode;
        }
    }
}
