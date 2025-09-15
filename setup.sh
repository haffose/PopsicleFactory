#!/bin/bash
# setup.sh - Setup script for Popsicle Factory API
echo "Setting up Popsicle Factory API..."
# Check if .NET 8 is installed
if ! command -v dotnet &> /dev/null; then
    echo ".NET 8 SDK is not installed. Please install it from https://dotnet.microsoft.com/download"
    exit 1
fi
# Check .NET version
DOTNET_VERSION=$(dotnet --version)
echo ".NET version: $DOTNET_VERSION"
# Restore packages
echo "Restoring NuGet packages..."
dotnet restore
# Build the solution
echo "Building the solution..."
dotnet build --configuration Release
# Run tests
echo "Running tests..."
dotnet test --configuration Release --no-build
echo "Setup complete!"
echo ""
echo "To run the API:"
echo "  dotnet run --project API"
echo ""
echo "To run with Docker:"
echo "  docker-compose up --build"
echo ""
echo "API will be available at:"
echo "  - https://localhost:7212 (with Swagger UI)"
echo "  - http://localhost:5122"