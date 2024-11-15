![](https://github.com/is-leeroy-jenkins/Bubba/tree/master/Resources/Assets/GitHubImages/ProjectTemplate.png)

<div align="left">
  <p>
    <a href="https://github.com/is-leeroy-jenkins/Bubba/tree/master/Resources/Github/Users.md">Download</a> •  <a href="">Documentation</a> •<a href="https://github.com/is-leeroy-jenkins/Bubba/tree/master/Resources/Github/Compilation.md">Build</a> • <a href="#-license">License</a>
  </p>
  <p>
    <b>An open source tool for connecting and interacting through a single, windows interface!</b>
  </p>
  <p align="left">
    Connect and interact with ChatGPT through a single user interface.

</div>


## ![](https://github.com/is-leeroy-jenkins/Bubba/blob/master/Resources/Assets/GitHubImages/csharp.png)  Code

- Bubba supports x64 specific builds
- [Enumerations](https://github.com/is-leeroy-jenkins/Bubba/tree/master/Enumerations) - enumerations used in the applications.
- [Extensions](https://github.com/is-leeroy-jenkins/Bubba/tree/master/Extensions) - extension classes for Collections, DataTables, DataRows, LINQ, etc.
- [IO](https://github.com/is-leeroy-jenkins/Bubba/tree/master/IO)- classes providing input/output and networking functionality.
- [Resources](https://github.com/is-leeroy-jenkins/Bubba/tree/master/Resources) - images, documentation, and other assets used in the applications
- [Controls](https://github.com/is-leeroy-jenkins/Bubba/tree/master/Controls) - classes used for interacting with the user interface. 
- [Themes](https://github.com/is-leeroy-jenkins/Bubba/tree/master/Themes) - classes defining styles & themes for the user interface. 
- [Views](https://github.com/is-leeroy-jenkins/Bubba/tree/master/Views) - windows used by Bubba.
- [Events](https://github.com/is-leeroy-jenkins/Bubba/tree/master/Events) - callbacks used in the application
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



## 🙏 Acknowledgements

Bubba uses the following projects and libraries. Please consider supporting them as well (e.g., by starring their repositories):

|                                                                               |                                                                        |
| ----------------------------------------------------------------------------- | ---------------------------------------------------------------------- |
| [#SNMP Library](https://github.com/lextudio/sharpsnmplib)                     | SNMP library for .NET                                                  |
| [AirspaceFixer](https://github.com/chris84948/AirspaceFixer)                  | AirspacePanel fixes all Airspace issues with WPF-hosted Winforms.      |
| [ControlzEx](https://github.com/ControlzEx/ControlzEx)                        | Shared Controlz for WPF and more                                       |
| [DnsClient.NET](https://github.com/MichaCo/DnsClient.NET)                     | Powerful, high-performance open-source library for DNS lookups         |
| [Docusaurus](https://docusaurus.io/)                                          | Easy to maintain open source documentation websites.                   |
| [Dragablz](https://dragablz.net/)                                             | Tearable TabControl for WPF                                            |
| [GongSolutions.Wpf.DragDrop](https://github.com/punker76/gong-wpf-dragdrop)   | An easy to use drag'n'drop framework for WPF                           |
| [IPNetwork](https://github.com/lduchosal/ipnetwork)                           | .NET library for complex network, IP, and subnet calculations          |
| [LoadingIndicators.WPF](https://github.com/zeluisping/LoadingIndicators.WPF)  | A collection of loading indicators for WPF                             |
| [MahApps.Metro.IconPacks](https://github.com/MahApps/MahApps.Metro.IconPacks) | Awesome icon packs for WPF and UWP in one library                      |
| [MahApps.Metro](https://mahapps.com/)                                         | UI toolkit for WPF applications                                        |
| [NetBeauty2](https://github.com/nulastudio/NetBeauty2)                        | Move .NET app runtime components and dependencies into a sub-directory |
| [PSDiscoveryProtocol](https://github.com/lahell/PSDiscoveryProtocol)          | PowerShell module for LLDP/CDP discovery                               |

## ![](https://github.com/is-leeroy-jenkins/Bubba/blob/master/Resources/Assets/GitHubImages/signature.png)  Code Signing 

Bubba uses free code signing provided by [SignPath.io](https://signpath.io/) and a free code signing certificate
from [SignPath Foundation](https://signpath.org/).

The binaries and installer are built on [AppVeyor](https://ci.appveyor.com/project/is-leeroy-jenkins/networkmanager) directly from the [GitHub repository](https://github.com/is-leeroy-jenkins/Bubba/blob/main/appveyor.yml).
Build artifacts are automatically sent to [SignPath.io](https://signpath.io/) via webhook, where they are signed after manual approval by the maintainer.
The signed binaries are then uploaded to the [GitHub releases](https://github.com/is-leeroy-jenkins/Bubba/releases) page.


## ![](https://github.com/is-leeroy-jenkins/Bubba/blob/master/Resources/Assets/GitHubImages/training.png) Privacy Policy

This program will not transfer any information to other networked systems unless specifically requested by the user or the person installing or operating it.

Bubba has integrated the following services for additional functions, which can be enabled or disabled at the first start (in the welcome dialog) or at any time in the settings:

- [api.github.com](https://docs.github.com/en/site-policy/privacy-policies/github-general-privacy-statement) (Check for program updates)
- [ipify.org](https://www.ipify.org/) (Retrieve the public IP address used by the client)
- [ip-api.com](https://ip-api.com/docs/legal) (Retrieve network information such as geo location, ISP, DNS resolver used, etc. used by the client)

## 📝 License

Bubba is published under the [GNU General Public License v3](https://github.com/is-leeroy-jenkins/Bubba/blob/main/LICENSE).

The licenses of the libraries used can be found [here](https://github.com/is-leeroy-jenkins/Bubba/tree/main/Resources/Licenses).
