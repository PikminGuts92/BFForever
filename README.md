# BFForever [![Build status](https://ci.appveyor.com/api/projects/status/yt8eu333kn91fv2y?svg=true)](https://ci.appveyor.com/project/PikminGuts92/bfforever)
BFForever is an open-source library for managing and creating game files for the short-lived BandFuse video game which released for PS3 and Xbox 360 consoles in November 2013. [Preview](https://www.youtube.com/watch?v=NHvFKonTGR0)

The latest build can be found on [AppVeyor](https://ci.appveyor.com/project/PikminGuts92/bfforever/branch/master/artifacts).

# System Requirements
You will need at least [.NET 5](https://dotnet.microsoft.com/download/dotnet-core) runtime installed and be using an x64 operating system.

# Planned Features
* Full support for creating/managing RIFF files
* XPR2/GTF texture conversion
* CLT (OPUS) audio conversion
* MIDI <-> RIFF conversion

# Custom Song Pipeline (WIP)
Input will be a package that contains a json file which lists song metadata (title, artist, etc.) and instrument specific information such as tuning. It will also contain pre-converted non-RIFF song files like chart, audio stems, album art, and music video.

From that the song manager will import the package and interact with a game archive to generate the necessary RIFF files required for the song.

# Currently Implemented
* RIFF
  * Full serialization of select zobjects
* CELT
  * Encoding/decoding
