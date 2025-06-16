###### Bubba
![](https://github.com/is-leeroy-jenkins/Bubba/blob/master/Resources/Assets/GitHubImages/project_bubba.png)
<div align="left">
  <p>
    <a href="https://github.com/is-leeroy-jenkins/Bubba/tree/master/Resources/Github/Users.md">Download</a> •  <a href="">Documentation</a> •<a href="https://github.com/is-leeroy-jenkins/Bubba/tree/master/Resources/Github/Compilation.md">Build</a> • <a href="#-license">License</a>
  </p>
  <p>
  </p>
</div>



### ![](https://github.com/is-leeroy-jenkins/Bubba/blob/master/Resources/Assets/GitHubImages/features.png) Features

- An open source tool for connecting and interacting through a single, windows interface!
- Connect and interact with ChatGPT through a single user interface.
- Asynchronous ChatGPT Interaction: Efficiently sent prompts to the OpenAI Completions and Assistants API.
- **Data Collection**: Collects data such as PDFs, CSVs, DOCX, XLS, PPTX, TXT, Images, Videos, JSON, DBSQL, XML, HTML, PHP, JS, Archives, and Miscellaneous files.
- **Database Integration**: Organizes scraped URLs into SQLite databases based on their file types.
- **Modern UI Design**: User-friendly WPF interface with rich text logging.
- **Multi OS Support**: Compatible with Windows x64/x86/ARM, Linux and MacOS.
___

### ![](https://github.com/is-leeroy-jenkins/Bubba/blob/master/Resources/Assets/GitHubImages/csharp.png)  Code

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
___

![](https://github.com/is-leeroy-jenkins/Bocifus/blob/main/Resources/Assets/DemoImages/OpenAIOnWPF.gif)

### ![](https://github.com/is-leeroy-jenkins/Bubba/blob/master/Resources/Assets/GitHubImages/documentation.png)  Documentation

- [User Guide](https://github.com/is-leeroy-jenkins/Bubba/tree/master/Resources/Github/Users.md) - how to use Bubba.
- [Compilation Guide](https://github.com/is-leeroy-jenkins/Bubba/tree/master/Resources/Github/Compilation.md) - instructions on how to compile Bubba.
- [Configuration Guide](https://github.com/is-leeroy-jenkins/Bubba/tree/master/Resources/Github/Configuration.md) - information for the Bubba configuration file. 
- [Distribution Guide](https://github.com/is-leeroy-jenkins/Bubba/tree/master/Resources/Github/Distribution.md) -  distributing Bubba.

___

### ![](https://github.com/is-leeroy-jenkins/Bubba/blob/master/Resources/Assets/GitHubImages/openai.png)  Generative AI

> Vectorization is the process of converting textual data into numerical vectors and is a process that is usually applied once the text is cleaned.
> It can help improve the execution speed and reduce the training time of your code. Bubba provides the following vector stores
> on the OpenAI platform to support environmental data analysis with machine-learning:


### 📦 Fine-tuning Datasets

- [Appropriations](https://huggingface.co/datasets/leeroy-jankins/Appropriations) - Enacted appropriations from 1996-2024 available for fine-tuning learning models
- [Regulations](https://huggingface.co/datasets/leeroy-jankins/Regulations/tree/main) - Collection of federal regulations on the use of appropriatied funds
- [SF-133](https://huggingface.co/datasets/leeroy-jankins/SF133) - The Report on Budget Execution and Budgetary Resources
- [Balances](https://huggingface.co/datasets/leeroy-jankins/Balances) -  U.S. federal agency Account Balances (File A) submitted as part of the DATA Act 2014.
- [Outlays](https://huggingface.co/datasets/leeroy-jankins/Outlays) -  The actual disbursements of funds by the U.S. federal government from 1962 to 2025
- [SF-133](https://huggingface.co/datasets/leeroy-jankins/SF133) The Report on Budget Execution and Budgetary Resources
- [Balances](https://huggingface.co/datasets/leeroy-jankins/Balances) - U.S. federal agency Account Balances (File A) submitted as part of the DATA Act 2014.
- [Circular A11](https://huggingface.co/datasets/leeroy-jankins/OMB-Circular-A-11) - Guidance from OMB on the preparation, submission, and execution of the federal budget
- [Fastbook](https://huggingface.co/datasets/leeroy-jankins/FastBook) - Treasury guidance on federal ledger accouts
- [Redbook](https://huggingface.co/datasets/leeroy-jankins/RedBook) - The Principles of Appropriations Law (Volumes I & II).

___


###  gpt-4o

- Bubba is prompted to review 10 years of appropriation data
- Extrapolate the appropriated amounts.
- Total the amount for a fiscal year. 
- Present the calculated data in the form of a table. 

## ![](https://github.com/is-leeroy-jenkins/Bubba/blob/master/Resources/Assets/GitHubImages/Bubba.gif) 


## ![](https://github.com/is-leeroy-jenkins/Badger/blob/main/Resources/Assets/GitHubImages/graph.png) Maching Learning

- Bubba uses `gpt-4-turbo` for Embedding & fine-tuning regulatory guidance.  
- Large-Language Models `gpt-4o-mini`, `o1`, and `davinci` are used for Budget-reasonsing
- Translations and transcriptions available via `whipser-1`, and `tts-2`
- Image geneerations provided by `dall-e-3` 
- [Outlay Projections](https://anaconda.cloud/api/nbserve/launch_notebook?nb_url=https%3A%2F%2Fanaconda.cloud%2Fapi%2Fprojects%2Ff4ad0240-eaf1-4ad1-a8b4-99e630b46cda%2Ffiles%2Foutlays.ipynb%3Fversion%3D3c5763b3-e106-4e67-b314-3207f7f4ee71) is a forecasting model that uses historical expenditure data, 
generative AI, and machine-learning to project future outlays by agency and fiscal year.

 
- [Balance Estimator](https://anaconda.cloud/api/nbserve/launch_notebook?nb_url=https%3A%2F%2Fanaconda.cloud%2Fapi%2Fprojects%2F088fd703-e3a6-426c-af6b3c80bf443a33%2Ffiles%2FBalanceEstimator.ipynb%3Fversion%3Dac106c51-bc0c-4e82-a9c0-1c59c48f4510) is a forecasting model that uses 
historical data from 2017 to 2024 to project balances at the account-level.

___

  

### ![](https://github.com/is-leeroy-jenkins/Bubba/blob/master/Resources/Assets/GitHubImages/tools.png) Build

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


### 🙏 Acknowledgements

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

### ![](https://github.com/is-leeroy-jenkins/Bubba/blob/master/Resources/Assets/GitHubImages/chrome.png) CefSharp Requirements

#### The binaries directory must contain these required dependencies:

- libcef.dll (Chromium Embedded Framework Core library)
- icudtl.dat (Unicode Support data)
- chrome_elf.dll(Crash reporting library)
- snapshot_blob.bin, v8_context_snapshot.bin (V8 snapshot data)
- locales\en-US.pak, chrome_100_percent.pak, chrome_200_percent.pak, resources.pak, 
- d3dcompiler_47.dll 
- libEGL.dll 
- libGLESv2.dll

#### Whilst these are technically listed as optional, the browser is unlikely to function without these files.

- CefSharp.Core.dll, CefSharp.dll 
- CefSharp.Core.Runtime.dll
- CefSharp.BrowserSubprocess.exe 
- CefSharp.BrowserSubProcess.Core.dll

#### These are required CefSharp binaries that are the common core logic binaries of CefSharp (only 1 required).

- CefSharp.WinForms.dll
- CefSharp.Wpf.dll
- CefSharp.OffScreen.dll

#### By default `CEF` has it's own log file, `Debug.log` which is located in your executing folder. e.g. `bin`


### ![](https://github.com/is-leeroy-jenkins/Bubba/blob/master/Resources/Assets/GitHubImages/signature.png)  Code Signing 

Bubba uses free code signing provided by [SignPath.io](https://signpath.io/) and a free code signing certificate
from [SignPath Foundation](https://signpath.org/).

The binaries and installer are built on [AppVeyor](https://ci.appveyor.com/project/is-leeroy-jenkins/networkmanager) directly from the [GitHub repository](https://github.com/is-leeroy-jenkins/Bubba/blob/main/appveyor.yml).
Build artifacts are automatically sent to [SignPath.io](https://signpath.io/) via webhook, where they are signed after manual approval by the maintainer.
The signed binaries are then uploaded to the [GitHub releases](https://github.com/is-leeroy-jenkins/Bubba/releases) page.


### ![](https://github.com/is-leeroy-jenkins/Bubba/blob/master/Resources/Assets/GitHubImages/web.png) Privacy Policy

This program will not transfer any information to other networked systems unless specifically requested by the user or the person installing or operating it.

Bubba has integrated the following services for additional functions, which can be enabled or disabled at the first start (in the welcome dialog) or at any time in the settings:

- [api.github.com](https://docs.github.com/en/site-policy/privacy-policies/github-general-privacy-statement) (Check for program updates)
- [ipify.org](https://www.ipify.org/) (Retrieve the public IP address used by the client)
- [ip-api.com](https://ip-api.com/docs/legal) (Retrieve network information such as geo location, ISP, DNS resolver used, etc. used by the client)

## 📝 License

Bubba is published under the [MIT General Public License v3](https://github.com/is-leeroy-jenkins/Bubba/blob/main/LICENSE).

The licenses of the libraries used can be found [here](https://github.com/is-leeroy-jenkins/Bubba/tree/main/Resources/Licenses).
