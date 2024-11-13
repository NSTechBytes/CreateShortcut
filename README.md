

# CreateShortcut Rainmeter Plugin

The **CreateShortcut** plugin allows you to create shortcuts on your desktop and check if they exist using Rainmeter. This plugin can be useful for creating custom shortcuts for your Rainmeter skins and easily managing them.

## Features

- **Create Shortcuts**: Create shortcuts on your desktop with a custom name, icon, and target location.
- **Check Shortcut**: Check if a specific shortcut exists on your desktop.
- **Customizable**: Fully customizable in Rainmeter skins.

## Installation

1. **Download the plugin**: Download the `CreateShortcut.dll` plugin.
2. **Place the DLL in the Plugins folder**:
   - Navigate to your Rainmeter installation directory (`C:\Users\<YourUserName>\Documents\Rainmeter\Plugins\`).
   - Place the `CreateShortcut.dll` file in the Plugins folder.
3. **Set up a skin**:
   - Create a new `.ini` file or modify an existing skin to include the `CreateShortcut` plugin.

## Usage

To use the `CreateShortcut` plugin in your Rainmeter skin, you can create a measure to create a shortcut or check if it exists, and link it to an action or button.

### Example Skin

```ini
[Metadata]
Name=CreateShortcut
Author=NS Tech Bytes
Version=1.0
Description=This skin allows you to create a shortcut on your desktop and check its existence using the CreateShortcut plugin.
Tags=shortcut, create, check, plugin
License=Apache 2.0

[Rainmeter]
Update=1000
BackgroundMode=2
SolidColor=000000

[CreateShortcut]
Measure=Plugin
Plugin=CreateShortcut.dll
Type=Create
ShortcutName=MyShortcut
ShortcutIcon=C:\Path\To\Icon.ico
Target=C:\Path\To\TargetFile.exe
SaveLocation=C:\Users\YourUserName\Desktop
OnCompleteAction=[!Log "Shortcut created!"]

[ShortcutCheck]
Measure=Plugin
Plugin=CreateShortcut.dll
Type=ShortcutFound
Location=C:\Users\YourUserName\Desktop\MyShortcut.lnk

[TextOutput]
Meter=STRING
MeasureName=ShortcutCheck
X=10
Y=10
W=300
H=30
FontColor=FFFFFF
FontSize=12
Text="Shortcut Status: %1"

[ButtonCreateShortcut]
Meter=Button
X=10
Y=50
W=200
H=40
Text="Create Shortcut"
LeftMouseUpAction=[!RainmeterPluginBang "CreateShortcut ExecuteBatch 1"]

[ButtonCheckShortcut]
Meter=Button
X=10
Y=100
W=200
H=40
Text="Check Shortcut"
LeftMouseUpAction=[!RainmeterPluginBang "ShortcutCheck ExecuteBatch 1"]
```

### Parameters

- **Type**:
  - `Create`: Used to create a shortcut.
  - `ShortcutFound`: Used to check if a shortcut exists.
- **ShortcutName**: The name of the shortcut you want to create.
- **ShortcutIcon**: The path to the icon for the shortcut (optional).
- **Target**: The path to the application or file the shortcut will point to.
- **SaveLocation**: The location where the shortcut will be saved (e.g., `C:\Users\YourUserName\Desktop`).
- **Location**: The location of the shortcut to check.
- **OnCompleteAction**: Optional action to take after creating the shortcut (e.g., log a message).

### Actions

- **CreateShortcut ExecuteBatch 1**: Used to trigger the creation of the shortcut.
- **ShortcutCheck ExecuteBatch 1**: Used to check if the shortcut exists.

## License

This project is licensed under the Apache License - see the [LICENSE](LICENSE) file for details.

## Support

If you run into any issues or have any questions, feel free to open an issue on the GitHub repository.

