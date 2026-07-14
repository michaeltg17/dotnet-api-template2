#!/bin/bash
set -e

echo "========================================="
echo "  Running ci"
echo "========================================="

# Step 1: Restore
echo ""
echo "[1/3] Restoring packages..."
dotnet restore Template.slnx
echo "Restore successful"

# Step 2: Build
echo ""
echo "[2/3] Building..."
dotnet build Template.slnx
echo "Build successful"

# Step 3: Tests
echo ""
echo "[3/3] Running tests..."
mkdir -p test-results
dotnet test Template.slnx --no-build
echo "Tests passed"

echo ""
echo "========================================="
echo "  All ci checks passed!"
echo "========================================="