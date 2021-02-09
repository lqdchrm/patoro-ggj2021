#!/bin/bash
PROJECT="./FindLosty/FindLosty.csproj"
CONFIG="Debug"
PLATFORM="Any CPU"
LOGPATH=./logs
SCRIPT="./FindLosty/Tests/walkthrough.txt"

mkdir -p $LOGPATH
(cat $SCRIPT && cat) | dotnet run -p $PROJECT -- script /p:Configuration=$CONFIG /p:Platform=$PLATFORM 2>&1 | tee "$LOGPATH/log.txt"
