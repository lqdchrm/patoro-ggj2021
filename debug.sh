#!/bin/bash
dotnet run -- script ./FindLosty/Tests/walkthrough.txt 2>&1 | tee ./logs/log.txt
