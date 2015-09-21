@echo off
".nuget/NuGet.exe" install FAKE -Version 4.3.7
"packages/FAKE.4.3.7/tools/FAKE.exe" build.fsx %1%
