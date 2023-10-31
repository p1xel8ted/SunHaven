### Sun Haven Mods

![Game Logo](NexusImages/header.jpg)<br>

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/F2F2DI3WA)<br>

All except mods except for `Custom Textures Redux` are available on the [NexusMods](https://www.nexusmods.com/sunhaven/users/4404677?tab=user+files&BH=1) page.

**Custom Textures Redux**

- I noticed on my machine (i9 9900k, NVME) that there was a delay on game load where there were no textures.
- This is a custom build of [Custom Textures](https://www.nexusmods.com/sunhaven/mods/6) that:
  - Removes all use of reflection
  - Only checks for texture replacements based on the textures in the CustomTextures folder (instead of all game textures)
  - Changed some loops and dictionaries to their multithreaded counterparts where safe to do so.
  - Remove case sensitivity from texture replacement.
  - No more `Material doesn't have a texture property '_MainTex'` spam.
- With the changes above, for around 150 textures shaved off between 100 and 200ms on my machine. 
- Only available on my GitHub for now as it's more of a personal project.

[Download Here](https://github.com/p1xel8ted/SunHaven/releases)

![GitHub release (by tag)](https://img.shields.io/github/downloads/p1xel8ted/SunHaven/CustomTexturesRedux/total?label=downloads&style=for-the-badge)