#r @"packages/FAKE.4.3.7/tools/FakeLib.dll"
#load "build-helpers.fsx"
open Fake
open System
open System.IO
open BuildHelpers

//Environment variables
let version = "1.1.1"
let build = environVarOrDefault "BUILD_NUMBER" "1"
let versionNumber = (version + "." + build)
setEnvironVar "VERSION_NUMBER" versionNumber

//iOS stuff
let provisioningId = ""
let provisioningName = ""

Target "clean" (fun () ->
    DeleteFile "TestResults.xml"
    CleanDir "Scratch"
    CleanDir "build"
    CleanDir "bin"
    CleanDir "obj"
)

Target "android" (fun () ->
    UnityDefine ["GOOGLE_PLAY"; "BETA_BUILD"]
    Unity "-executeMethod BuildScript.Android"
    ArchiveUnityLog @"build\GooglePlay_Beta_Log.txt"
    if not(File.Exists(@"build\Epic2_GooglePlay_Beta.apk")) then raise(Exception("Epic2_GooglePlay_Beta.apk not found!"))
)

Target "ios-player" (fun () ->
    UnityDefine ["BETA_BUILD"]
    Unity "-executeMethod BuildScript.iOS"
)

Target "ios" (fun () ->
    //Xcode archive
    DeleteDir "Scratch/Xcode/Epic2.xarchive/"
    //Update Info.plist
    UpdatePlist version versionNumber "Scratch/Xcode"
    Xcode ("-project Scratch/Xcode/Unity-iPhone.xcodeproj -scheme Unity-iPhone archive -archivePath Scratch/Xcode/Epic2 COMPRESS_PNG_FILES=NO GCC_PREPROCESSOR_DEFINITIONS=\"BETA_BUILD\" CODE_SIGN_ENTITLEMENTS=Epic2Beta.entitlements PROVISIONING_PROFILE=" + provisioningId)
    //Export the archive to an ipa file
    DeleteFile "build/Epic2_Beta.ipa"
    CreateDir "build"
    Xcode ("-exportArchive -archivePath Scratch/Xcode/Epic2.xcarchive -exportPath build/Epic2_Beta.ipa -exportProvisioningProfile " + provisioningName)
)

RunTarget()
