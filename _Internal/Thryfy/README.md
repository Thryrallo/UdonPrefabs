# Thryfy
Thryfy is a youtube music song database with ~200.000 songs by 10.000 artist. It allows you to search songs by name or artist & creates a playlist based on a song choice.

## Thryfy is a video player extension
The Prefab is made in way that it uses an adapter pattern to connect to video players.
Currently the only implemented (an thus the only supported videoplayer) is [UdonSharp VideoPlayer](https://github.com/MerlinVR/USharpVideo)

# Setup
1. Drag YT_DB_Manager into project
2. Drag Thryfy into project
3. On Thryfy, make sure Database manager is set to the 'YT_DB_Manager' prefab & the Adapater is set to the Adapter
4. On the Adapter Object make sure the Thryfy prefab & assign your video player object to the slot 
