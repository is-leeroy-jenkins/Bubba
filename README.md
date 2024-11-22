<div align="left">
  <p>
    <a href="https://github.com/is-leeroy-jenkins/Bubba/tree/master/Resources/Github/Users.md">Download</a> •  <a href="">Documentation</a> •<a href="https://github.com/is-leeroy-jenkins/Bubba/tree/master/Resources/Github/Compilation.md">Build</a> • <a href="#-license">License</a>
  </p>
  <p>
    
  </p>
</div>

![](https://github.com/is-leeroy-jenkins/Bubba/blob/master/Resources/Assets/GitHubImages/project_bubba.png)



## ![](https://github.com/is-leeroy-jenkins/Bubba/blob/master/Resources/Assets/GitHubImages/features.png) Features

- An open source tool for connecting and interacting through a single, windows interface!
- Connect and interact with ChatGPT through a single user interface.
- Asynchronous ChatGPT Interaction: Efficiently sent prompts to the OpenAI Completions and Assistants API.
- Data Collection**: Collects data such as PDFs, CSVs, DOCX, XLS, PPTX, TXT, Images, Videos, JSON, DBSQL, XML, HTML, PHP, JS, Archives, and Miscellaneous files.
- Database Integration**: Organizes scraped URLs into SQLite databases based on their file types.
- Modern UI Design**: User-friendly WPF interface with rich text logging.

- Multi OS Support**: Compatible with Windows x64/x86/ARM, Linux and MacOS.

## ![](https://github.com/is-leeroy-jenkins/Bubba/blob/master/Resources/Assets/GitHubImages/csharp.png)  Code

- Bubba supports x64 specific builds
- [Enumerations](https://github.com/is-leeroy-jenkins/Bubba/tree/master/Enumerations) - enumerations used in the applications.
- [Extensions](https://github.com/is-leeroy-jenkins/Bubba/tree/master/Extensions) - extension classes for Collections, DataTables, DataRows, LINQ, etc.
- [IO](https://github.com/is-leeroy-jenkins/Bubba/tree/master/IO)- classes providing input/output and networking functionality.
- [Resources](https://github.com/is-leeroy-jenkins/Bubba/tree/master/Resources) - images, documentation, and other assets used in the applications
- [Controls](https://github.com/is-leeroy-jenkins/Bubba/tree/master/UI/Controls) - classes used for interacting with the user interface. 
- [Themes](https://github.com/is-leeroy-jenkins/Bubba/tree/master/UI/Themes) - classes defining styles & themes for the user interface. 
- [Views](https://github.com/is-leeroy-jenkins/Bubba/tree/master/UI/Views) - windows used by Bubba.
- [Callbacks](https://github.com/is-leeroy-jenkins/Bubba/tree/master/Callbacks) - events, delegates, and event-handlers used in the application
- [Data](https://github.com/is-leeroy-jenkins/Bubba/tree/master/Data) - data classes used by Bubba


## ![](https://github.com/is-leeroy-jenkins/Bubba/blob/master/Resources/Assets/GitHubImages/documentation.png)  Documentation

- [User Guide](https://github.com/is-leeroy-jenkins/Bubba/tree/master/Resources/Github/Users.md) - how to use Bubba.
- [Compilation Guide](https://github.com/is-leeroy-jenkins/Bubba/tree/master/Resources/Github/Compilation.md) - instructions on how to compile Bubba.
- [Configuration Guide](https://github.com/is-leeroy-jenkins/Bubba/tree/master/Resources/Github/Configuration.md) - information for the Bubba configuration file. 
- [Distribution Guide](https://github.com/is-leeroy-jenkins/Bubba/tree/master/Resources/Github/Distribution.md) -  distributing Bubba.


## 📦 Download

Pre-built and binaries (setup, portable and archive) are available on the with install instructions (e.g. silent install). 




## ![](https://github.com/is-leeroy-jenkins/Bubba/blob/master/Resources/Assets/GitHubImages/tools.png) Build

- [x] VisualStudio 2022
- [x] Based on .NET8 and WPF


```bash
$ git clone https://github.com/is-leeroy-jenkins/Bubba.git
$ cd Bubba
```
Run `Bubba.sln`


You can build the application like any other .NET / WPF application on Windows.

1. Make sure that the following requirements are installed:

   - [.NET 8.x - SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
   - Visual Studio 2022 with `.NET desktop development` and `Universal Windows Platform development`

2. Clone the repository with all submodules:

   ```PowerShell
   # Clone the repository
   git clone https://github.com/is-leeroy-jenkins/Bubba

   # Navigate to the repository
   cd Bubba

   # Clone the submodules
   git submodule update --init
   ```

3. Open the project file `.\Source\Bubba.sln` with Visual Studio or JetBrains Rider to build (or debug)
   the solution.

   > **ALTERNATIVE**
   >
   > With the following commands you can directly build the binaries from the command line:
   >
   > ```PowerShell
   > dotnet restore .\Source\Bubba.sln
   >
   > dotnet build .\Source\Bubba.sln --configuration Release --no-restore
   > ```

# Bubba's AI layer provides access to the OpenAI VectorStores and Files

## Federal Appropriations

|               Public Law                 |           Explanatory Statement       |	  	Appropriation Title			                                                                            |	    
| ---------------------------------------- | --------------------------------------|----------------------------------------------------------------------------------------------------------|
		P.L. 104-134			   H. Rpt. 104-537				 Omnibus Consolidated Rescissions and Appropriations Act
		P.L. 104-208			   H. Rpt. 104-863 			         Omnibus Consolidated Appropriations Act, 1997
		P.L. 105-277			   H. Rpt. 105-825				 Omnibus Consolidated and Emergency Supplemental Appropriations Act
		P.L. 105-277 			   H. Rpt. 106-110 			         Omnibus Consolidated and Emergency Supplemental Appropriations Act
		P.L. 106-113			   H. Rpt. 106-479 			         Consolidated Appropriations Act, 2000
		P.L. 106-79			   H. Rpt. 106-371				 Department of Defense Appropriations Act, 2000
		P.L. 106-113			   H. Rpt. 106-479				 Consolidated Appropriations Act, 2000
		P.L. 106-79			   H. Rpt. 106-371				 Department of Defense Appropriations Act, 2000
		P.L. 106-554			   H. Rpt. 106-1033			         Consolidated Appropriations Act, 2001
		P.L. 106-259			   S. Rpt. 106-298				 Department of Defense Appropriations Act, 2001
		P.L. 107-117			   H. Rpt. 107-350				 Department of Defense and Emergency Supplemental Appropriations
		P.L. 107-206			   H. Rpt. 107-593				 Supplemental Appropriations Act, 2002
		P.L. 108-7			   H. Rpt. 108-10				 Consolidated Appropriations Resolution, 2003
		P.L. 108-199			   H. Rpt. 108-401				 Consolidated Appropriations Act, 2004
		P.L. 108-11			   H. Rpt. 108-55				 Emergency Supplemental Appropriations Act for Defense
		P.L. 108-447			   H. Rpt. 108-792				 Consolidated Appropriations Act, 2005
		P.L. 109-13			   H. Rpt. 109-72				 Emergency Supplemental Appropriations Act for Defense, the Global War on Terror, and Tsunami Relief
		P.L. 109-108			   H. Rpt. 109-272				 Science, State, Justice, Commerce Appropriations Act
		P.L. 109-148			   S. Rpt. 109-141				 Department of Defense Appropriations Act, 2006
		P.L. 110-5			   H. Rpt. 110-5				 Revised Continuing Appropriations Resolution, 2007
		P.L. 110-161			   H. Rpt. 110-497				 Consolidated Appropriations Act, 2008
		P.L. 110-252			   H. Rpt. 110-656				 Supplemental Appropriations Act, 2008
		P.L. 111-8			   H. Rpt. 111-8				 Omnibus Appropriations Act, 2009
		P.L. 111-32			   H. Rpt. 111-105				 Supplemental Appropriations Act, 2009
		P.L. 111-117			   H. Rpt. 111-366				 Consolidated Appropriations Act, 2010
		P.L. 112-10			   H. Rpt. 112-331				 Department of Defense and Full-Year Continuing Appropriations Act, 2011
		P.L. 112-74			   H. Rpt. 112-331				 Consolidated Appropriations Act, 2012
		P.L. 113-6			   H. Rpt. 113-6				 Consolidated and Further Continuing Appropriations Act, 2013
		P.L. 113-76			   H. Rpt. 113-76				 Consolidated Appropriations Act, 2014
		P.L. 113-235		           H. Rpt. 113-235				 Consolidated and Further Continuing Appropriations Act, 2015
		P.L. 114-113		           H. Rpt. 114-113				 Consolidated Appropriations Act, 2016
		P.L. 115-31			   H. Rpt. 115-31				 Consolidated Appropriations Act, 2017
		P.L. 115-141		           H. Rpt. 115-141				 Consolidated Appropriations Act, 2018
		P.L. 116-6			   H. Rpt. 116-6				 Consolidated Appropriations Act, 2019
		P.L. 116-93			   H. Rpt. 116-93				 Further Consolidated Appropriations Act, 2020
		P.L. 116-260		           H. Rpt. 116-260				 Consolidated Appropriations Act, 2021
		P.L. 117-103		           H. Rpt. 117-103				 Consolidated Appropriations Act, 2022
		P.L. 117-328		           H. Rpt. 117-328				 Consolidated Appropriations Act, 2023
		P.L. 118-42			   H. Rpt. 118-42				 Continuing Appropriations Act, 2024
		P.L. 118-83			   H. Rpt. 118-83				 Continuing Appropriations Act, 2025


## 🙏 Acknowledgements

Bubba uses the following projects and libraries. Please consider supporting them as well (e.g., by starring their repositories):

|                                 Library                                       |             Description                                                |
| ----------------------------------------------------------------------------- | ---------------------------------------------------------------------- |
| [CefSharp.WPF.Core](https://github.com/cefsharp)                              | .NET (WPF/Windows Forms) bindings for the Chromium Embedded Framework  |
| [Epplus](https://github.com/EPPlusSoftware/EPPlus)                  		    | EPPlus-Excel spreadsheets for .NET      								 |
| [Google.Api.CustomSearchAPI.v1](https://developers.google.com/custom-search)  | Google APIs Client Library for working with Customsearch v1            |
| [LinqStatistics](https://www.nuget.org/packages/LinqStatistics)                     | Linq extensions to calculate basic statistics.                                                 |
| [System.Data.SqlServerCe](https://www.nuget.org/packages/System.Data.SqlServerCe_unofficial)                  | Unofficial package of System.Data.SqlServerCe.dll if you need it as dependency.      |
| [Microsoft.Office.Interop.Outlook 15.0.4797.1004](https://www.nuget.org/packages/Microsoft.Office.Interop.Outlook)                        | This an assembly you can use for Outlook 2013/2016/2019 COM interop, generated and signed by Microsoft. This is entirely unsupported and there is no license since it is a repackaging of Office assemblies.                                       |
| [ModernWpfUI 0.9.6](https://www.nuget.org/packages/ModernWpfUI/0.9.7-preview.2)                     | Modern styles and controls for your WPF applications.         |
| [Newtonsoft.Json 13.0.3](https://www.nuget.org/packages/Newtonsoft.Json)                                          | Json.NET is a popular high-performance JSON framework for .NET                  |
| [RestoreWindowPlace 2.1.0](https://www.nuget.org/packages/RestoreWindowPlace)                                             | Save and restore the place of the WPF window                                           |
| [SkiaSharp 2.88.9](https://github.com/mono/SkiaSharp)   | SkiaSharp is a cross-platform 2D graphics API for .NET platforms based on Google's Skia Graphics Library.                           |
| [Syncfusion.Licensing 24.1.41](https://www.nuget.org/packages/Syncfusion.Licensing)                           | Syncfusion licensing is a .NET library for validating the registered Syncfusion license in an application at runtime.         |
| [System.Data.OleDb 9.0.0](https://www.nuget.org/packages/System.Data.OleDb)  | This package implements a data provider for OLE DB data sources.                            |
| [OpenAI API](https://github.com/openai/openai-dotnet)                       | The OpenAI .NET library provides convenient access to the OpenAI REST API from .NET applications.  |
| [System.Data.SqlClient 4.9.0](https://www.nuget.org/packages/System.Data.SqlClient) | The legacy .NET Data Provider for SQL Server.                       |
| [MahApps.Metro](https://mahapps.com/)                                         | UI toolkit for WPF applications                                        |
| [System.Data.SQLite.Core](https://www.nuget.org/packages/System.Data.SQLite.Core)                        | The official SQLite database engine for both x86 and x64 along with the ADO.NET provider. |
| [System.Speech 9.0.0](https://www.nuget.org/packages/System.Speech)          | Provides APIs for speech recognition and synthesis built on the Microsoft Speech API in Windows.                               |
| [ToastNotifications.Messages.Net6 1.0.4](https://github.com/rafallopatka/ToastNotifications)          | Toast notifications for WPF allows you to create and display rich notifications in WPF applications.                              |                           |
| [Syncfusion.SfSkinManager.WPF 24.1.41](https://www.nuget.org/packages/Syncfusion.SfSkinManager.WPF)          | The Syncfusion WPF Skin Manageris a .NET UI library that contains the SfSkinManager class, which helps apply the built-in themes to the Syncfusion UI controls for WPF.                               |
| [Syncfusion.Shared.Base 24.1.41](https://www.nuget.org/packages/Syncfusion.Shared.Base)          | Syncfusion WinForms Shared Components                               |
| [Syncfusion.Shared.WPF 24.1.41](https://www.nuget.org/packages/Syncfusion.Shared.WPF)          | Syncfusion WPF components                               |
| [Syncfusion.Themes.FluentDark.WPF 24.1.41](https://www.nuget.org/packages/Syncfusion.Themes.FluentDark.WPF)          | The Syncfusion WPF Fluent Dark Theme for WPF contains the style resources to change the look and feel of a WPF application to be similar to the modern Windows UI style introduced in Windows 8 apps.                               |
| [Syncfusion.Tools.WPF 24.1.41](https://www.nuget.org/packages/Syncfusion.Tools.WPF)          | This package contains WPF AutoComplete, WPF DockingManager, WPF Navigation Pane, WPF Hierarchy Navigator, WPF Range Slider, WPF Ribbon, WPF TabControl, WPF Wizard, and WPF Badge components for WPF application.                               |
| [Syncfusion.UI.WPF.NET 24.1.41](https://www.nuget.org/packages/Syncfusion.UI.WPF.NET)          | Syncfusion WPF Controls is a library of 100+ WPF UI components and file formats to build modern WPF applications. 


## ![](https://github.com/is-leeroy-jenkins/Bubba/blob/master/Resources/Assets/GitHubImages/signature.png)  Code Signing 

Bubba uses free code signing provided by [SignPath.io](https://signpath.io/) and a free code signing certificate
from [SignPath Foundation](https://signpath.org/).

The binaries and installer are built on [AppVeyor](https://ci.appveyor.com/project/is-leeroy-jenkins/networkmanager) directly from the [GitHub repository](https://github.com/is-leeroy-jenkins/Bubba/blob/main/appveyor.yml).
Build artifacts are automatically sent to [SignPath.io](https://signpath.io/) via webhook, where they are signed after manual approval by the maintainer.
The signed binaries are then uploaded to the [GitHub releases](https://github.com/is-leeroy-jenkins/Bubba/releases) page.


## ![](https://github.com/is-leeroy-jenkins/Bubba/blob/master/Resources/Assets/GitHubImages/web.png) Privacy Policy

This program will not transfer any information to other networked systems unless specifically requested by the user or the person installing or operating it.

Bubba has integrated the following services for additional functions, which can be enabled or disabled at the first start (in the welcome dialog) or at any time in the settings:

- [api.github.com](https://docs.github.com/en/site-policy/privacy-policies/github-general-privacy-statement) (Check for program updates)
- [ipify.org](https://www.ipify.org/) (Retrieve the public IP address used by the client)
- [ip-api.com](https://ip-api.com/docs/legal) (Retrieve network information such as geo location, ISP, DNS resolver used, etc. used by the client)

## 📝 License

Bubba is published under the [MIT General Public License v3](https://github.com/is-leeroy-jenkins/Bubba/blob/main/LICENSE).

The licenses of the libraries used can be found [here](https://github.com/is-leeroy-jenkins/Bubba/tree/main/Resources/Licenses).
