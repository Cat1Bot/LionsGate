# LionsGate
Small C# tool that Grants early access to Riot's upcoming fighter game 2XKO (codenamed Lion). Simply run the executable, and it will automatically inject the 2XKO patchline into the Riot Client. No need to manually open or close the Riot Client; the app handles everything for you.
![image](https://github.com/user-attachments/assets/d7325594-c500-45e5-8714-3674c461aee4)
### Usage
Before running the application, ensure that you have the [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download) installed.

You can either use the precompiled executable available in the [Releases section](https://github.com/Cat1Bot/LionsGate/releases) or clone this repository and build it yourself using Visual Studio 2022. To build the project, run the following command in the terminal:
```bash   
dotnet publish "C:\path\to\LionsGate.csproj" -c Release -r win-x64 --self-contained false /p:PublishSingleFile=true
```
### TODO | Help Needed
The version of 2XKO that the Riot Client installs is outdated because I don’t have access to the latest manifest URL. Since I don't have beta access, I'm currently using the last publicly known URL, which causes authentication issues as it cannot properly connect to the RCS API.

**If you have beta access**, please send me your `ClientConfiguration.json` file located in `C:\Users\[YourUsername]\AppData\Local\Riot Games\Riot Client\Config\`. You can either send me the entire file or the JSON key starting with keystone.products.lion.patchlines.live (it may also say beta or similar instead of live). You can open issue here or contact me on Discord: c4t_bot
