# Lethal Company ItemQuickSwitch

Simple Hotbar/Item quick switch mod for Lethal Company. Allows you to use your Keyboard to access item slots directly.

## Installation

Your game needs to be modded with the latest version of [BepInEx 5](https://github.com/BepInEx/BepInEx).

### Thunderstore users:

Install the package using the Thunderstore App or any other mod manager that is compatible with Thunderstore.
I have not tested this method of installing the mod, so it might not be working correctly.

If you have troubles installing the mod please join the
unofficial [Lethal Company Modding Discord server](https://discord.gg/GVVFX2cd)
and leave a comment under
this [thread](https://discord.com/channels/1168655651455639582/1174185250646143066/1174185250646143066).
Or alternatively, raise an issue on my [GitHub repository](https://github.com/vasanex/ItemQuickSwitchMod) (GitHub
account needed).

### Manual install:

Download the latest release from Thunderstore or the [Github repository](https://github.com/vasanex/ItemQuickSwitchMod/releases) and
put `ItemQuickSwitchMod.dll` into your `BepInEx\plugins` folder.

## Usage

This mod lets you access item slots directly instead of having to scroll through them.

### Default Keybinds

* Press 1 to access item slot 1
* Press 2 to access item slot 2
* Press 3 to access item slot 3
* Press 4 to access item slot 4
* Press F1 to perform emote 1 (dance)
* Press F2 to perform emote 2 (point)

### Custom Configuration

Upon first startup of the game with this mod active, a config file will be generated
under `BepInEx/config/ItemQuickSwitchMod.cfg`. Close the game and edit the config file to your liking.

## Known Issues

This mod is currently hardcoded to support 4 inventory slots. If use any other mods which increases the amount of slots
you have available, the quick switch will only work with slots 1 through 4.

## Uninstall

Remove the `ItemQuickSwitchMod.dll` file from your plugins folder.