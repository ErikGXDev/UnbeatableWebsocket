# Unbeatable Websocket

This mod for UNBEATABLE adds support for WebSocket communication with the game.

This is useful if other programs want to control the game, for example to load and play custom maps programmatically.

## Current features

- Loading custom maps from a specified directory
- (Planned) Loading custom Beatball maps

## Installation

1. Download and install BepInEx 5 into your UNBEATABLE game directory. (If you have this already, skip to step 3)
2. Run the game, then close it
3. [Download this mod](https://github.com/ErikGXDev/UnbeatableWebsocket/releases)
4. Merge the BepInEx folder from this mod with the BepInEx folder in your game directory
5. Run the game
6. Use the WebSocket server at `ws://localhost:5080` to communicate with the game

## Documentation

Send the following text messages to the WebSocket server to interact with the game:

`play <file path>`: Load and play a custom map from the specified file path.
`play_beatball <file path>`: **Not fully implemented yet** - Load and play a custom Beatball map from the specified file path.
