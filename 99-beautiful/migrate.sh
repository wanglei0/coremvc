#!/bin/bash
# Execute migration script against src/WebApp

SLN_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
DB_FILE_PATH="$SLN_DIR/src/WebApp/Data/sampledb.db"
TOOLS_PATH="$SLN_DIR/tools"

dotnet tool install -v d --tool-path "${TOOLS_PATH}" FluentMigrator.DotNet.Cli
${TOOLS_PATH}/dotnet-fm migrate -p sqlite -c "Data Source=${DB_FILE_PATH}" -a "${SLN_DIR}/src/Migration/bin/Debug/netcoreapp2.2/Migration.dll"