#!/bin/bash

mono --runtime=v4.0 .nuget/nuget.exe install FAKE -Version 4.3.7
mono --runtime=v4.0 packages/FAKE.4.3.7/tools/FAKE.exe build.fsx $@
