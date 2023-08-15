#!/bin/bash

# This is the combined script for your app

# Check if Node.js is installed
if ! command -v node &> /dev/null; then
    echo "Node.js is required but not installed. Please install Node.js to continue."
    exit 1
fi

# Install app dependencies using npm
echo "Installing app dependencies..."
npm install

# Display setup complete message
echo "Setup complete! You can now run the app using 'node app.js'."

# Start the app
node app.js &
echo "App started. You can access the Socket.io client by opening 'index.html' in your browser."

# Wait for user to press Enter to exit the app
read -p "Press Enter to stop the app..."
kill %1