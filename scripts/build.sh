#!/bin/bash

DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
echo "dir is $DIR"

cd $DIR
rm -rf ../publish/

cd ../src
dotnet clean
dotnet build
dotnet test

cd $DIR
cd ../src/JOHNNYbeGOOD.Home.Api
dotnet publish -c release -r linux-arm -o ../../publish/api

cd $DIR
cd ../src/JOHNNYbeGOOD.Home.Client
dotnet publish -c release -r linux-arm -o ../../publish/client

cd $DIR
cd ../
echo "copying additional files"
chmod 777 $DIR/install.sh
cp $DIR/install.sh ./publish

echo "Compressing files"
tar -czf ./publish/api.tar.gz ./publish/api
tar -czf ./publish/client.tar.gz ./publish/client

echo "Remove uncompressed files"
rm -rf ./publish/api
rm -rf ./publish/client