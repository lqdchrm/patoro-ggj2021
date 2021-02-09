#!/bin/bash
PROJECT="./FindLosty/FindLosty.csproj"
LOGPATH=./logs
SCRIPT="./FindLosty/Tests/walkthrough.txt"

mkdir -p $LOGPATH
dotnet run -p $PROJECT -- script $SCRIPT 2>&1 | tee "$LOGPATH/log.txt"
