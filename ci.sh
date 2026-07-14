#!/usr/bin/env bash
set -euo pipefail

SOLUTION="Template.slnx"

echo "========================================="
echo "  Running CI"
echo "========================================="

echo
echo "[1/3] Restoring packages..."
dotnet restore "$SOLUTION"
echo "Restore successful"

echo
echo "[2/3] Building..."
dotnet build "$SOLUTION" --configuration Release --no-restore
echo "Build successful"

echo
echo "[3/3] Running tests..."
dotnet test "$SOLUTION" --configuration Release --no-build
echo "Tests passed"

echo
echo "========================================="
echo "  All CI checks passed!"
echo "========================================="