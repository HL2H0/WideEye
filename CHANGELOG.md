# WideEye ChangeLog

## [3.1.0] **Regular Update**
### Added
* Free Camera Mode
* Added a startup delay slider [Changing it below 2 seconds is not recommended]

### Changed
* Some code improvements
* UI Layout 
	* Moved the Support Page to the bottom of the main menu
* Removed some unnecessary notifications
  * Replaced all the notifications with a single one that shows when everything is loaded properly.


### Fixed
* Fixed Ko-fi Button in the README file
* Fixed `ViewPath` not opening the file explorer properly. (Again lol)
* Removed leftover logging 
* Version number not being right

### Known Issues
* After zooming with the free cam and stopping quickly, the FOV may display as a long decimal value

---

## [3.0.1] **Hotfix**	

### Added
* Now the mod will check for the resources file, if it's not found,it notifies the user.

### Fixed
* Fixed an issue where when you click on `ViewPath` it opens the file instead of showing it in the explorer.
* Fixed the cameras not syncing FOVs properly.
* Fixed the view mode not being changed properly.

---


## [3.0.0] **Major Update**

### Important: New dependency is required [FieldInjector](https://thunderstore.io/c/bonelab/p/WNP78/FieldInjector/)

### Added
* Handheld Camera, with :
	* Built-in Lighting
	* Zoom in/out
	* Cool Design
* Presets System
* Complete code overhaul

### Changed
* UI Layout
* Removed camera modes (Replaced by the Handheld Camera)

---

## [2.1.0] **Regular Update**

### Added
* Notification Customizability
* Auto Save ( Experimental )


### Changed
* Reduced the waiting time form 5 seconds to 3 seconds

### Fixed
* A few typos lol
* When preferences is loaded, rotation smoothing loads as position smoothing

---

## [2.0.0] **Major Update**

### Added
* Preferences Saving
* Camera Auto-Finding
* Position Offset
* Post-Processing Control :
	* Auto Exposure
	* Chromatic Aberration
	* Lens Distortion
* Camera Modes :
	* Pinned
	* Head
* Avatar Meshes Control (On | Off) :
	* Hair Meshes
	* Head Meshes

### Changed
* UI Improvements

### Fixed
* Levels with scene chunks no longer un-finds the camera

---

## [1.2.0] **Regular Update**

### Added
* Smoothing
* Rotation Offset
* Support Page
* Reset All Button
* Notifications

### Known Issues
* Position smoothing may cause the spectator camera to clip through head meshes.
* Levels with scene chunks may un-find the camera, but it won't affect the modifications the mod made

---

## [1.1.0] **Regular Update**

### Added
* Added ability to toggle post-processing.
* Added Colors to the mod's menu

### Changed
* Changing values won't be accessible when the camera is not found, reducing confusion.

### Known Issues
* Levels with scene chunks may un-find the cam, but it won't affect the modifications the mod made

---
## [1.0.0]

* Initial Release
