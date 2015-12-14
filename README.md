# ricloud.net: iCloud access made easy

This is a sample .NET library for interaction with Reincubate's iCloud API. The Reincubate iCloud API provides powerful programmatic iCloud access to investigators, application developers and integrators. It is RESTful and makes many commonly-accessed forms of data available as JSON feeds.

The API includes functionality for extraction, manipulation and recovery of many types of iOS data, and has functionality to support bulk, scheduled, and realtime data access. It fully supports iOS 9 CloudKit-based iCloud backups, and backups created with the new A9 chipsets.

## JSON feed vs raw file access

There are two core parts to the API: the JSON feed mechanism, and the raw file access mechanism. The JSON feeds come with a number of advantages:

 * Access to feed data is generally faster and scales better than raw file access
 * App data stored in databases and Plists is prone to change in format and location over time; the JSON feed abstracts away that complexity so that you needn't worry.
 * Users of the JSON feeds are able to take advantage of Reincubate's proprietary techniques in extracting app data, such that the resultant data is more accurate.

## Installation

If you are using the NuGet Package Manager you can download and install the ricloud package using the following command:

```Install-Package ricloud```

Or you can download the package directly from the [NuGet Gallery](https://www.nuget.org/packages/ricloud/)

The source code can be compiled in Visual Studio 2010 or 2015 (other versions may also work). The only third-party dependency is currently Newtonsoft.JSON which is available to download via NuGet or directly from www.json.net .

### Configuration

The API relies on a set of security credentials. Sample credentials are baked into the sample_application.cs example file.

Full access can be gained by contacting [Reincubate](mailto:enterprise@reincubate.com).

## Usage

A sample script is included which provides an example of how the API can be used to access a range of datatypes in a way that is compatible with Apple's 2FA mechanism. To use the sample script you will first need to compile the ricloud project. Once this is done, you can compile the sample_application.cs file with the following command line (ensure you either change the DLL reference paths or copy them from the output folder into the same folder as the sample script):

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

### Using the JSON feed API

The API is able to return data retrieved from a wide range of apps, and enumerations for some of these are baked into the sample API. However, we have many other types of app feeds available, including Viber, Kik, WeChat, Line, and others.

> We also have functionality such as message undeletion which can be enabled on demand against API keys.

## Troubleshooting

### The JSON feed returns a message: "Contact enterprise@reincubate.com for access to this data"

This message will be returned when the demonstration key is used. Please contact us for a trial key with access to more data. If you already have a trial key, are you correctly specifying the API credentials?

### I'm trying to pull an app's database file by `file_id` but I'm not getting any data back

`file_id`s are derived from an SHA-1 hash of the file's path and name, so they are constant for any given file. If the file's attributes or content change, it won't affect the hash.

However, sometimes app authors change the name of the file they store data in (and sometimes Apple do in new iOS releases). That's why, for instance, there several different `file_id`s to examine when getting WhatsApp data. These `file_id`s could be changed any time an app is updated.

This is one of the reasons we recommend users pull our JSON feeds instead of working with files and manipulating them directly. Using the JSON feeds, one needn't worry over the efficacy of SQL, PList parsing or undeletion, and the JSON feeds are quicker and much simpler to work with.

## Need more functionality?

Reincubate builds world class iOS and app data access and recovery technology. The company was founded in 2008 and was first to market with iOS backup extraction technology, consumer backup decryption, and more recently with enterprise iCloud support. Clients include law enforcement, government and security organisations in the US and internationally, and to corporations as large as Microsoft and IBM.

> Users with simpler needs may wish to try the [iPhone Backup Extractor](http://www.iphonebackupextractor.com), which provides a set of iCloud functionality better suited to consumers.

With six years' experience helping police forces, law firms and forensics labs access iOS data, the company can help enterprise users with:

* iCloud access and data recovery
* Recovery of data deleted from SQLite databases
* Bulk iOS data recovery
* Forensic examination of iOS data
* Passcode, password and keybag analysis
* Custom iOS app data extraction
* Advanced PList, TypedStream and Mbdb manipulation

Contact [Reincubate](mailto:enterprise@reincubate.com) for more information, or see our site at [reincubate.com](https://www.reincubate.com).

## Terms & license

Users must not use the API in any way that is unlawful, illegal, fraudulent or harmful; or in connection with any unlawful, illegal, fraudulent or harmful purpose or activity. See the `LICENSE` file. Full terms are available from [Reincubate](mailto:enterprise@reincubate.com).

Copyright &copy; Reincubate Ltd, 2011 - 2015, all rights reserved.
