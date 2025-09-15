#!/bin/bash
echo "Starting Popsicle Factory API..."
# Check if running in development
if [[ "$1" == "dev" ]]; then
    echo "Running in development mode with hot reload..."
    dotnet watch run --project API
else
    echo "Running in normal mode..."
    dotnet run --project API
fi