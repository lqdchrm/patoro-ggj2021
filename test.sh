#!/bin/bash
(cat ./FindLosty/Tests/walkthrough.txt && cat) | dotnet run -- script /p:Configuration=Debug /p:Platform="Any CPU" 2>&1 | tee ./logs/log.txt
