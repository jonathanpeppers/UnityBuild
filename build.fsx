#r @"packages/FAKE.4.3.7/tools/FakeLib.dll"
#load "build-helpers.fsx"
open Fake
open System
open BuildHelpers

//Environment variables
let version = "1.0.0"
let build = environVarOrDefault "BUILD_NUMBER" "1"
let versionNumber = (version + "." + build)
setEnvironVar "VERSION_NUMBER" versionNumber

//iOS stuff
let provisioningId = "eceff215-f35c-45a6-bed8-09bb562401e9"
let provisioningName = "GenericInHouse"

Target "clean" (fun () ->
    DeleteFile "TestResults.xml"
    CleanDir "Scratch"
    CleanDir "build"
    CleanDir "bin"
    CleanDir "obj"
)

Target "android" (fun () ->
    Unity "-executeMethod BuildScript.Android"
)

Target "ios-player" (fun () ->
    Unity "-executeMethod BuildScript.iOS"
)

Target "ios" (fun () ->
    //Xcode archive
    DeleteDir "Scratch/Xcode/UnityBuild.xarchive/"
    //Update Info.plist
    UpdatePlist version versionNumber "Scratch/Xcode"
    Xcode ("-project Scratch/Xcode/Unity-iPhone.xcodeproj -scheme Unity-iPhone archive -archivePath Scratch/Xcode/UnityBuild PROVISIONING_PROFILE=" + provisioningId)
    //Export the archive to an ipa file
    DeleteFile "build/UnityBuild.ipa"
    CreateDir "build"
    Xcode ("-exportArchive -archivePath Scratch/Xcode/UnityBuild.xcarchive -exportPath build/UnityBuild.ipa -exportProvisioningProfile " + provisioningName)
)

RunTarget()
