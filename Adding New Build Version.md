# Adding new build version for binaries

**open files in notepad or notepad++:**

- Module.sln
- JET/JustEmuTarkov.csproj

LEGEND:
VERSION - means game version like 10988 or 9767

### File: JustEmuTarkov.csproj

**adding new Property Group**
_INFO: replace "VERSION" with game version of your choose_
_INFO: a BVERSION should look like B10988_

```
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'VERSION|AnyCPU' ">
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\Release\</OutputPath>
    <DefineConstants>BVERSION;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
    <LangVersion>8.0</LangVersion>
  </PropertyGroup>
```

**if your version contains bsg.componentace.compression.libs.zlib.dll**

in line 108 add new condition at the begining following the flow.
`'$(Configuration)' == 'VERSION' Or `

**if game version is above 13074 or contains dll FilesChecker.dll**
in line 120 add new condition at the begining following the flow.
`'$(Configuration)' == 'VERSION' Or `

**if your version contains netstandard.dll**
in line 128 add new condition at the begining following the flow.
`'$(Configuration)' == 'VERSION' Or `

**if your version DOESN'T contain zlib.net.dll**
in line 182 add new condition at the begining following the flow.
`'$(Configuration)' != 'VERSION' And `

**END OF EDITING JustEmuTarkov.csproj**

### File: Module.sln

around line 16 add line looking like this with replaced VERSION to your game version number
`VERSION|Any CPU = VERSION|Any CPU`

around line 28 we are starting to add new solution parameters for each solution whic hare 3
patter looks like this:

```
		{GUID_OF_THE_PROJECT}.VERSION|Any CPU.ActiveCfg = VERSION|Any CPU
		{GUID_OF_THE_PROJECT}.VERSION|Any CPU.Build.0 = VERSION|Any CPU
```

it will be there around 3 times cause we have 3 solutions in the project JET / JET singleplayer / JET Launcher

GUID of projects:

- AC9ADC41-6007-4392-9D52-2BD850506052
- 3812C87E-F193-43B0-9FC6-3116948C023B
- 1D95F26C-E959-40C8-A73F-C9E280E87E17

**END OF EDITING Module.sln**

## Adding References to be auto loaded

- go to "Shared" folder and create folder which tempalte looks like this
  `References_VERSION` - remember to replace VERSION with your game version
- go inside of that folder
- now things can get kind of complicated if version is older its better to look at older version folders and trying to add refferences like there if version is newer make sure to add files there as the newer version has for example for version 15xxx we gonna add files like for R"eferences_14687" folder.
- after that we have done everything outside of visual studio

## inside visual studio

**try to locate places like:**

```cs
#if B13074
  // come content
#endif
```

with adding new ones above them so it will look like:

```cs
#if BVERSION
  // your new content
#elseif B13074
  // come content
#endif
```

files that contains gclasses or places that uses #if compilation statements

```
 Files To Edit GClasses:
	Utilities.EasyBundleHelper.cs
	Utilities.Config.cs
	Utilities.Player.HealthListener.cs
	Patches.ScavMode.LoadOfflineRaidScreenPatch.cs
	Patches.Quests.DogtagPatch.cs
	Patcher.Progression.WeaponDurabilityPatch.cs
	Patcher.Progression.OfflineSpawnPointPatch.cs
	Patcher.Progression.OfflineSavePatch.cs
	Patcher.Progression.OfflineLootPatch.cs (disabled after 12.11 cause we use build in one)
	Patcher.Other.HideoutRequirementIndicator.cs
	Patcher.Matchmaker.MatchMakerSelectionLocationScreenPatch.cs >> menu button
	Patcher.Matchmaker.MatchMakerAfterSelectLocation.cs >> menu button
	Patcher.Matchmaker.InsuranceScreenPatch.cs
	Patcher.Healing.MainMenuControllerPatch.cs
	Patcher.Bots.RemoveUsedBotProfilePatch.cs
	Patcher.Bots.GetNewBotTemplatesPatch.cs
	Patcher.Bots.CoreDifficultyPatch.cs
	Patcher.Bots.BotSettingsLoadPatch.cs
	Patcher.Bots.BotDifficultyPatch.cs


Edit finder so its not searching static names
	RemoveAddOfferButton.cs + BarterSchemeAutoFill.cs
	UnlockItemsIdLength.cs
```

you can also fine this list in SinglePlayer.cs file
