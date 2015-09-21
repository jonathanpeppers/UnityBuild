# UnityBuild
Example project showing automated builds with Unity3D

Note, build script assumes you have Unity installed at `/Applications/Unity/Unity.app/` on Mac and `C:\Program Files\Unity` on Windows. You can edit these paths in `build-helpers.fsx`.

# How to run a build

On Mac
```
#Exports an Xcode project to Scratch/Xcode
./build.sh ios-player
#Builds the Xcode project, and exports the ipa file to build/
./build.sh ios
#Builds an Android apk to build/
./build.sh android
```
Notes: I have the Xcode project using my provisioning profile. In `build.fsx`, you can replace the provisioning profile ID and name to use your own.

On Windows
```
#Builds an Android apk to build\
build android
```
Notes: sorry, iOS won't work on Windows. Email Tim Cook.
