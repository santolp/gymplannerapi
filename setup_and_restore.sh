#!/usr/bin/env bash
set -e
echo 'Restoring .NET packages...'
dotnet restore
echo 'Building backend...'
dotnet build
echo 'Backend ready.'
