#!/bin/bash
set -e

# --- helper functions for logs ---
info()
{
    echo '[INFO] ' "$@"
}

# --- VARIABLES ---
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
DOWNLOAD_DIR=/tmp/johnnybegoodhome
EXTRACT_DIR=${DOWNLOAD_DIR}/extract
SERVICE_NAME=johnnybegood
SERVICE_FILE=${SERVICE_NAME}.service
SYSTEMD_DIR=/etc/systemd/system
FILE_SERVICE=${SYSTEMD_DIR}/${SERVICE_FILE}
BIN_DIR=/usr/local/bin
INSTALL_DIR=${BIN_DIR}/${SERVICE_NAME}
VERSION="latest"

# --- DOWNLOAD FILES ---
info "Downloading ${VERSION}"
$SUDO rm -rf ${DOWNLOAD_DIR} || true
mkdir ${DOWNLOAD_DIR}
wget --quiet -P ${DOWNLOAD_DIR} "https://github.com/johnnybegood/johnnybegood.home/releases/download/${VERSION}/api.tar.gz"

# --- UNPACK ---
info "Unpacking JOHHNYbeGOOD.Home to ${EXTRACT_DIR}"
mkdir ${EXTRACT_DIR}
tar -xzf ${DOWNLOAD_DIR}/api.tar.gz -C ${EXTRACT_DIR}

# --- STOPPING ---
info "Stopping existing JOHHNYbeGOOD.Home"
systemctl stop ${SERVICE_NAME} >/dev/null 2>&1 || true

# --- BACKUP DB ---
info "Backup DB"
$SUDO cp ${INSTALL_DIR}/feeder-v1.db ${EXTRACT_DIR} || true
$SUDO cp ${INSTALL_DIR}/feeder-v1.db ${EXTRACT_DIR}/feeder-v1.db.bak || true

# --- CLEAR OLD INSTALL ---
info "Removing old install"
$SUDO rm -f ${FILE_SERVICE} || true
$SUDO systemctl disable ${SERVICE_NAME} >/dev/null 2>&1 || true
$SUDO rm -rf ${INSTALL_DIR}  >/dev/null 2>&1 || true

# --- SETUP PERMISSIONS ---
info "Setting premisions JOHHNYbeGOOD.Home"
$SUDO chown -R root:root ${EXTRACT_DIR}
$SUDO chmod 777 ${EXTRACT_DIR}/${SERVICE_FILE}

# --- COPY FILES ---
info "Installing JOHHNYbeGOOD.Home to ${INSTALL_DIR}"
$SUDO mv -f ${EXTRACT_DIR}/${SERVICE_FILE} ${SYSTEMD_DIR}
$SUDO mv -f ${EXTRACT_DIR} ${INSTALL_DIR}

# --- START SERVICE ---
info "Enabling ${SERVICE_NAME} unit"
$SUDO systemctl enable ${FILE_SERVICE}
$SUDO systemctl daemon-reload
$SUDO systemctl start ${SERVICE_NAME}

# --- CLEAR INSTALL ---
$SUDO rm -rf ${EXTRACT_DIR} || true