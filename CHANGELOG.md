# Changelog
## v1.1.0
* Refactoring of the code thanks to MarkdeGraaff in [\#1](https://github.com/vasanex/ItemQuickSwitchMod/pull/1).
* Fixed an edge case where client could potentially get out of sync with server if more than four item slots are accessible 
_(NOTE: This mod still officially only supports 4 slots like the vanilla game)_
* Config file will now be generated under `BepInEx/config/de.vasanex.ItemQuickSwitchMod.cfg` due to plugin GUID change.
Existing overrides need to be recreated in this file.
* Added BepInEx-Pack dependency for Thunderstore release.

## v1.0.3
* Removed Debug logs that happen everytime you switched slots
* Added MIT License to the GitHub repository

## v1.0.2
* Updated ReadMe file. 
* No other changes have been made. If you already are using version `v1.0.1` you don't need to update.

## v1.0.1
* Now properly cancels build mode on item switch.
* Should now properly play item SFX on slot change.

## v1.0.0
* initial release