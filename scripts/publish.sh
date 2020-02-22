#!/bin/bash

LOCATION=$1
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"

cd $DIR
./build.sh
scp -r ../publish/ $LOCATION:~/feeding/