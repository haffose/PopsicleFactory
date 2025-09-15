#!/bin/bash
echo "Running Popsicle Factory API Tests..."
# Run tests with coverage
dotnet test --configuration Release --collect:"XPlat Code Coverage" --results-directory ./TestResults
# Generate coverage report if reportgenerator is installed
if command -v reportgenerator &> /dev/null; then
    echo "Generating coverage report..."
    reportgenerator -reports:"./TestResults/*/coverage.cobertura.xml" -targetdir:"./TestResults/CoverageReport" -reporttypes:Html
    echo "Coverage report available at: ./TestResults/CoverageReport/index.html"
else
    echo "Install reportgenerator for HTML coverage reports:"
    echo "   dotnet tool install -g dotnet-reportgenerator-globaltool"
fi
echo "Tests completed!"