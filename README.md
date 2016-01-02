# ricloud-csharp: iCloud access made easy

This is the sample .NET / C# client for Reincubate's [iCloud API](https://www.reincubate.com/labs/icloud-api/?utm_source=github&utm_medium=ricloud-csharp&utm_campaign=ricloud).

> Refer to the comprehensive [iCloud API documentation](https://www.reincubate.com/contact/support/icloud-api/?utm_source=github&utm_medium=ricloud-csharp&utm_campaign=ricloud) for a fuller picture of the API's capabilities, specifications, and benefits.

## Installation

Users with the NuGet Package Manager  can download and install this package using the following command:

```bash
nuget install ricloud
```

Or the package can be downloaded directly from the [NuGet Gallery](https://www.nuget.org/packages/ricloud/).

The source code can be compiled in Visual Studio 2010 or 2015 (other versions may also work). The only third-party dependency is currently `Newtonsoft.JSON` which is available to download via NuGet or directly from [json.net](http://www.json.net).

### Configuration

The API relies on a set of security credentials. Sample credentials are baked into the `sample_application.cs` example file.

## Usage

A sample script is included which provides an example of how the API can be used to access a range of datatypes in a way that is compatible with Apple's 2FA mechanism. To use the sample script you will first need to compile the ricloud project. Once this is done, you can compile the `sample_application.cs` file with the following command line (ensure you either change the DLL reference paths or copy them from the output folder into the same folder as the sample script):

```
csc sample_application.cs /reference:ricloud.dll,Newtonsoft.JSON.dll
```

This will create a console application called '''sample_application.exe'''

Here's what the output from the sample script looks like:

```
Please enter your Apple ID: renate@reincubate.com
Please enter your password:

2FA has been enabled, choose a trusted device:
0 - ********16
1 - Renate's iPad - iPad Pro
2 - Renate's iPhone - iPhone 6s

Choose a device by specifying its index (e.g. 0): 2

A code has been sent to your device.
Code:  4967

Your devices:
0 - Renate's iPad [model: J71AP, colour: #3b3b3c, latest-backup: 2015-06-23 07:00:00.000000]
1 - Renate's iPhone [model: N71mAP, colour: #e4e7e8, latest-backup: 2015-10-13 19:07:48.000000]
2 - Renate's iPad [model: J98aAP, colour: #e1e4e3, latest-backup: 2015-11-15 20:36:48.000000]
3 - Renate's iPhone [model: N71AP, colour: #e4e7e8, latest-backup: 2015-11-17 20:51:36.000000]
4 - Renate's US iPhone [model: N49AP, colour: #3b3b3c, latest-backup: 2015-05-06 07:00:00.000000]
5 - Renate's iPhone [model: N61AP, colour: #e1e4e3, latest-backup: 2015-10-21 15:53:26.000000]

Choose a device by specifying its index (e.g. 0): 3

What would you like to download?

1     Messages
2     Photos and Videos
4     Browser History
8     Call History
16    Contacts
32    Installed Apps
512   WhatsApp Messages
1024  Skype Messages
2048  Appointments
4096  Line Messages
8192  Kik Messages
16384 Viber Messages
64    Contacts (live)
256   Location (live)

Mask (0) for all:  0
Complete! All data is in the directory "out".
```

## Troubleshooting

See the iCloud API [support page](https://www.reincubate.com/contact/support/icloud-api/?utm_source=github&utm_medium=ricloud-csharp&utm_campaign=ricloud).

## <a name="more"></a>Need more functionality?

Reincubate's vision is to provide data access, extraction and recovery technology for all app platforms, be they mobile, desktop, web, appliance or in-vehicle.

The company was founded in 2008 and was first to market with both iOS and iCloud data extraction technology. With over half a decade's experience helping law enforcement and security organisations access iOS data, Reincubate has licensed software to government, child protection and corporate clients around the world.

The company can help users with:

* iCloud access and data recovery
* Recovery of data deleted from SQLite databases
* Bulk iOS data recovery
* Forensic examination of iOS data
* Passcode, password, keybag and keychain analysis
* Custom iOS app data extraction
* Advanced PList, TypedStream and Mbdb manipulation

Contact [Reincubate](https://www.reincubate.com/?utm_source=github&utm_medium=ricloud-csharp&utm_campaign=ricloud) for more information.

## Terms & license

See the `LICENSE.md` file for details on this implentation's license. Users must not use the API in any way that is unlawful, illegal, fraudulent or harmful; or in connection with any unlawful, illegal, fraudulent or harmful purpose or activity.
