# JustEmuTarkov - DLL Mod
Client-side runtime patches to alter the client's behaviour.

## Modules
- JET.Common: utilities used across modules.
- JET.Core: required patches for the game to run.
- JET.SinglePlayer: simulating online game offline.
- JET.Launcher: a custom game launcher allows the game to be launched offline.
- JET.RuntimeBundles: reponsible for loading custom asset bundles.

## Requirements
- Visual Studio 2017 (.NET desktop workload) or newer
- .NET Framework 4.6.1
- E.F.T. 0.12.7.8694

## Setup
All dependencies are provided, no additional setup required.

## Build
1. Visual Studio -> menubar -> rebuild solution.
2. Copy-paste all files inside `Build` into `game root directory`, overwrite when prompted.

## Credits
- TheMaoci
- Apofis (MultiPlayer)

## Disclaimer
This repository is for research purposes only, the use of this code is your responsibility.  
I take NO responsibility and/or liability for how you choose to use any of the source code available here. By using any of the files available in this repository, you understand that you are AGREEING TO USE AT YOUR OWN RISK. Once again, ALL files available here are for EDUCATION and/or RESEARCH purposes ONLY.  
