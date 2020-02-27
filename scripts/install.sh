#!/bin/bash
set -e

# --- helper functions for logs ---
info()
{
    echo '[INFO] ' "$@"
}

# --- LOCATIONS ---
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
EXTRACT_DIR=${DIR}/tmp
SOURCE_DIR=${EXTRACT_DIR}/publish/api
SERVICE_NAME=johnnybegood
SERVICE_FILE=${SERVICE_NAME}.service
SYSTEMD_DIR=/etc/systemd/system
FILE_SERVICE=${SYSTEMD_DIR}/${SERVICE_FILE}
BIN_DIR=/usr/local/bin
INSTALL_DIR=${BIN_DIR}/${SERVICE_NAME}

# --- STOPPING ---
info "Stopping existing JOHHNYbeGOOD.Home"
systemctl stop ${SERVICE_NAME} >/dev/null 2>&1 || true

# --- CLEAR OLD INSTALL ---
$SUDO rm -f ${FILE_SERVICE} || true
$SUDO systemctl disable ${SERVICE_NAME} >/dev/null 2>&1 || true
$SUDO rm -rf ${INSTALL_DIR}  >/dev/null 2>&1 || true

# --- UNPACK ---
info "Unpacking JOHHNYbeGOOD.Home to ${SOURCE_DIR}"
mkdir ${EXTRACT_DIR}
tar -xzvf api.tar.gz -C ${EXTRACT_DIR}

# --- SETUP PERMISSIONS ---
info "Setting premisions JOHHNYbeGOOD.Home"
$SUDO chown root:root ${SOURCE_DIR}
$SUDO chmod 777 ${SOURCE_DIR}/${SERVICE_FILE}

# --- COPY FILES ---
info "Installing JOHHNYbeGOOD.Home to ${INSTALL_DIR}"
$SUDO mv -f ${SOURCE_DIR}/${SERVICE_FILE} ${SYSTEMD_DIR}
$SUDO mv -f ${SOURCE_DIR} ${INSTALL_DIR}

# --- START SERVICE ---
info "Enabling ${SERVICE_NAME} unit"
$SUDO systemctl enable ${FILE_SERVICE}
$SUDO systemctl daemon-reload
$SUDO systemctl start ${SERVICE_NAME}

# --- CLEAR INSTALL ---
$SUDO rm -rf ${EXTRACT_DIR} || true