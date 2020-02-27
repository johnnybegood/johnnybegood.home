#!/bin/bash

LOCATION=$1
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"

if [ -z "${LOCATION}" ]; then
    echo '[ERROR] Location is required, run publish.sh user@host' >&2
    exit 1
fi

echo "- BUILDING"
cd $DIR
./build.sh

echo "- COPY TO ${LOCATION}"
cd $DIR
scp -r ../publish/ $LOCATION:~/feeding