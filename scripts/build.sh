#!/bin/bash
set -e

DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
echo "dir is $DIR"

echo "-------------------------------------------------------"
echo "[INFO] Cleanup previous publish"
echo "-------------------------------------------------------"
cd $DIR
rm -rf ../publish/

echo "-------------------------------------------------------"
echo "[INFO] Building solution"
echo "-------------------------------------------------------"
cd ../src
dotnet clean
dotnet build

echo "-------------------------------------------------------"
echo "[INFO] Testing solution"
echo "-------------------------------------------------------"
dotnet test --no-build

echo "-------------------------------------------------------"
echo "[INFO] Generating publish build API"
echo "-------------------------------------------------------"
cd $DIR
cd ../src/JOHNNYbeGOOD.Home.Api
dotnet publish -c release -r linux-arm --self-contained -o ../../publish/api

echo "-------------------------------------------------------"
echo "[INFO] Preparing package"
echo "-------------------------------------------------------"
cd $DIR
cd ../
chmod 777 $DIR/install.sh
cp $DIR/install.sh ./publish

echo "-------------------------------------------------------"
echo "[INFO] Compressing files"
echo "-------------------------------------------------------"
tar -czf ./publish/api.tar.gz -C ./publish/api .

echo "-------------------------------------------------------"
echo "[INFO] Cleaning up"
echo "-------------------------------------------------------"
rm -rf ./publish/api
rm -rf ./publish/client