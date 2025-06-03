# WideEye
[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/B0B01CJHZ5)
> **Note:** These modifications apply only to the *spectator view* and do not affect the VR headset display.

**WideEye** is a BONELAB mod built to give creators full control over the spectator camera. From precise FOV tuning to post-processing effects, smoothing, and a powerful handheld camera—WideEye enhances content creation with intuitive tools and customization options.

## ✨ What’s New in 3.0.0

- Complete code structure overhaul with improved logic, readability, and maintainability.
- New **Handheld Camera** with built-in lighting, zoom controls, and a preview screen.
- **Presets System**: Save and load camera settings; share them easily via `.json` files.
- removed legacy "Pinned"/"Head" camera modes.

---  

## 🎮 Features

- **FOV Control**

- **Preferences Saving**

- **Auto-Apply & Auto-Save** (experimental)

- **Offset Adjustments**
- Position
	- Rotation

- **Smoothing** _(Stabilization)_
- Position
	- Rotation

- **Avatar Mesh Toggle**
- Hair Meshes
	- Head Meshes

- **Post-Processing Adjustments**
- Auto Exposure
	- Chromatic Aberration
	- Lens Distortion

- **Handheld Camera**
- Zoom In/Out
	- Built-in Lighting
	- Preview Screen
- **Presets System**
- Save/Load settings
	- JSON file sharing

---  

## 🎮 Handheld Camera Controls

| Control                       | Action                          |  
|-------------------------------|---------------------------------|  
| Left Handle - Menu Button     | Pin camera in place             |  
| Left Handle - Trigger         | Zoom out                        |  
| Right Handle - Menu Button    | Toggle preview screen           |  
| Right Handle - Trigger        | Zoom in                         |  
  
---  


## 📤 Sharing Presets

Sharing your camera presets is easy—here’s how:

### Exporting

1. Open the **Presets** page and click **View Path**.

2. Your preset file (`.json`) will be highlighted in the file explorer.

3. Copy that file and send it to a friend.


> As they say: _sharing is caring_.

### Importing

1. Have your friend send you their `.json` preset file.

2. Paste the file into this folder: `[GameRootDir]\UserData\WideEye Resources\Presets`
- the `[GameRootDir]` For Steam is  : `C:\Program Files (x86)\Steam\steamapps\common\BONELAB`
- For Meta Link : `C:\Program Files\Meta\Software\BONELAB`
3. Reopen the **Presets** page and you should see the imported preset.

---  

## 📦 Installation

1. Download the latest release from [**Thunderstore**](https://thunderstore.io/c/bonelab/p/HL2H0/WideEye/).
2. Extract the contents of the ZIP.
3. Copy the included `Mods` and `UserData` folders into your BONELAB **root directory**.
	- Typically found at: `C:\Program Files (x86)\Steam\steamapps\common\BONELAB\`
- or `C:\Program Files\Meta\Software\BONELAB\`

Then you are ready to go!  
Enjoy WideEye!
  
---  

## 💬 GitHub Discussions

WideEye now has a dedicated [Discussions](https://github.com/HL2H0/WideEye/discussions) page! Whether you want to ask a question, share an idea, or show off your setup — there’s a place for you.

### Categories:

-   📢 **Announcements** — Official updates and news.

-   💬 **General** — Open talk about anything WideEye-related.

-   💡 **Ideas** — Suggest features or improvements.

-   📤 **Presets** — Share your custom camera presets.

-   ❓ **Q&A** — Ask questions or get help using the mod.

-   🎥 **Show and Tell** — Post your screenshots, videos, or creative setups using WideEye!


Join the conversation here:  
👉 [github.com/HL2H0/WideEye/discussions](https://github.com/HL2H0/WideEye/discussions)

---

## 🛠 Requirements

- PC
- BONELAB Patch 5/6
- MelonLoader 0.6
- [BoneLib 3.1.1](https://thunderstore.io/c/bonelab/p/gnonme/BoneLib/)
- [FieldInjector 2.1.1](https://thunderstore.io/c/bonelab/p/WNP78/FieldInjector/)

---  

## 📖 Story Behind WideEye

### How It All Started

WideEye actually started from a mix of frustration and curiosity. I was waiting for _someone_ to make a mod that let you change the FOV of BONELAB’s spectator camera—but no one did. I barely knew anything about C# back then, but I had UnityExplorer installed, so I used it to manually change the FOV. It worked... but it was super annoying to do every time.

So I thought, “Screw it, I’ll just make a simple mod that changes the FOV when I press a key.” I gave it a try—and it _didn’t_ work. I spent days trying to fix it, nearly gave up, but then noticed something dumb: there was a space at the end of the camera path. That tiny thing was throwing a `NullReferenceException`.

I removed the space, launched the game, pressed “F,” and boom—it worked.

I was so hyped I made a GitHub repo, a quick icon, a menu, and even a README all in one night. And just like that, WideEye was born.
  
---  
## 💖 Support

WideEye started as a random “what if?” moment—and now it’s an actual tool people use, which is wild.

If you like what I’m doing and want to help fuel future updates (or just keep me stocked with snacks and caffeine), you can toss a coin my way here:

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/B0B01CJHZ5)

No pressure—WideEye will always be free. But every bit of support helps keep the keyboard clacking and the ideas flowing 🧠✨

  
---  

## 🧠 Contributing & Feedback

WideEye is open-source and welcomes suggestions, bug reports, and contributions! You can:

- [Open an issue](https://github.com/HL2H0/WideEye/issues) for bugs or suggestions.
- Fork the repo and submit pull requests.
- Leave feedback to help improve future releases.

---  

## 🙌 Credits

**Contributors**
- [WaveSmash](https://github.com/WaveSmashies) — Smoothing & Rotation Offset *(Suggestion)*
- [xDraxxis](https://github.com/xDraxxis) — Position Offset & Preferences *(Suggestions)*
- [ThomasSteve83](https://github.com/ThomasSteve83) — Notification Customization *(Suggestion)*

**Tools**
- Aiden – [BONELAB Icon Template](https://www.figma.com/community/file/1218386424917309834)
- TrevTV – [MelonLoader Mod Template](https://github.com/TrevTV/MelonLoader.VSWizard)

---  

## 💡 Future Plans

- MKGlow support
- Custom UI

---  

## 📜 Changelog

See the [**Releases**](https://github.com/HL2H0/WideEye/releases) page for detailed changelogs.
