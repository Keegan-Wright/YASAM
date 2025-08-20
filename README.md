# YASAM

YASAM (Yet Another Steam Application Manager) is a Linux-compatible alternative to popular Steam platform tools, built
with Avalonia on .NET.

## Features

### Current

- Game idling (accumulate playtime in multiple games)
- Achievement management:
    - Unlock/lock individual achievements
    - Bulk unlock/lock selected achievements
    - Unlock/lock all achievements for a game

### Planned

- Automatic trading card farming
- Automatic card trading
- Free game alerts
- Task automation and scheduling via cron jobs

## Requirements

- .NET 9.0 runtime
- Steam client installed and running

## Installation

### Pre-built Binaries

Pre-built binaries are available on the [Releases](https://github.com/Keegan-Wright/YASAM/releases) page. Download the appropriate package for your platform.

### Package Managers

*Coming soon* - Installation via package managers like apt, dnf, pacman, etc.

## Building and Running

### Linux
```bash
# Clone the repository
git clone https://github.com/Keegan-Wright/YASAM.git
cd YASAM

# Build the project
dotnet build -c Release

# Navigate to the output directory
cd YASAM/bin/Release/net9.0

# Run the application
./YASAM
```


### Windows
```shell
# Clone the repository
git clone https://github.com/Keegan-Wright/YASAM.git
cd YASAM

# Build the project
dotnet build -c Release

# Navigate to the output directory
cd YASAM\bin\Release\net9.0

# Run the application
YASAM.exe
```


## Usage

1. Make sure Steam is running and you're logged in
2. Launch YASAM
3. Get your Steam64Id from [Steamid.io](https://steamid.io/)
4. Ensure you have a steam API key [Steam](https://steamcommunity.com/dev)
5. Add your user as a tracked user
6. Use the interface to idle games or manage achievements

## Screenshots

*Coming soon* - Screenshots of the application interface will be added to showcase the main features.

## Platform Compatibility

**Note:** This application has been developed on **Linux**. **Windows** has had limited testing but everything is
functional. **Not yet tested on macOS**. While the tooling used within YASAM is cross-platform there are extra factors
at play with how processes are invoked on a platform-by-platform basis so expect some things not to work until future
updates when running on a non linux system.

## Troubleshooting

### Common Issues

- **Steam not detected**: Ensure Steam is running before launching YASAM
- **API Key Issues**: Verify your Steam API key is valid and has the correct permissions
- **Game not showing**: Refresh your game library or check privacy settings in your Steam profile
- **Unable to idle games**: Verify your Steam account has no restrictions and games are properly installed

For more assistance, please [open an issue](https://github.com/Keegan-Wright/YASAM/issues) on the GitHub repository.

## Contributing

Contributions, suggestions, and feedback are warmly welcomed! If you'd like to contribute:

- Fork the repository
- Create a feature branch (`git checkout -b feature/amazing-feature`)
- Commit your changes (`git commit -m 'Add some amazing feature'`)
- Push to the branch (`git push origin feature/amazing-feature`)
- Open a Pull Request

For suggestions or feature requests, please open an issue with the tag "enhancement".
Bug reports should include as much detail as possible about how to reproduce the issue.

## Special Thanks

This project wouldn't be possible without these amazing open-source tools and libraries:

- [Avalonia UI](https://avaloniaui.net/) - Cross-platform .NET UI framework
- [Spectre Console](https://spectreconsole.net/) - A library that makes it easier to create beautiful console
  applications
- [.NET Community Toolkit](https://github.com/CommunityToolkit) - Collection of helpers and APIs for building .NET
  applications
- [SukiUI](https://github.com/kikipoulet/SukiUI) - Extensive UI component library
- [Facepunch.Steamworks](https://github.com/Facepunch/Facepunch.Steamworks) - A wrapper around Steams API

## Inspiration

This project aims to bring functionality similar to these Windows tools to Linux:

- [Steam Achievement Manager](https://github.com/gibbed/SteamAchievementManager)
- [Steam Game Idler](https://github.com/zevnda/steam-game-idler)

## License

[MIT](LICENSE)

## Disclaimer

This application is not affiliated with or endorsed by Valve Corporation. Use at your own risk as modification of
achievements may violate Steam's Terms of Service.

## FAQ

### Is it safe to use YASAM?

YASAM interacts with the Steam API in a similar way to other achievement managers. While many users have used similar tools without issues, modifying achievements or artificially accumulating playtime could potentially violate Steam's Terms of Service.

### Will YASAM get my Steam account banned?

There are no known cases of accounts being banned solely for using achievement managers or idlers. However, we cannot guarantee this will always be the case. Use at your own risk.

### Does YASAM work with all Steam games?

YASAM should work with most Steam games, but some games with special anti-cheat mechanisms or server-side achievement validation might not be compatible.

### Is my Steam API key secure?

Your Steam API key is stored locally on your machine. YASAM does not transmit your key to any external servers.

### How do I report a bug or request a feature?

Please use the [GitHub Issues](https://github.com/Keegan-Wright/YASAM/issues) page to report bugs or request features.

