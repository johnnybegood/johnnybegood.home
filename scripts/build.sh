#!/bin/bash

DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
echo "dir is $DIR"

cd $DIR
rm -rf ../publish/api
rm -rf ../publish/client

cd ../src
dotnet clean
dotnet build
dotnet test

cd ../src/JOHNNYbeGOOD.Home.Api
dotnet publish -c release -r linux-arm -o ../../publish/api

cd ../JOHNNYbeGOOD.Home.Client
dotnet publish -c release -r linux-arm -o ../../publish/client