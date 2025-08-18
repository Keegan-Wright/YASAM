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

```bash
# Clone the repository
git clone https://github.com/Keegan-Wright/YASAM.git
cd YASAM

# Further expansion...
```

## Usage

1. Make sure Steam is running and you're logged in
2. Launch YASAM
3. Use the interface to idle games or manage achievements

## Platform Compatibility

**Note:** This application has been developed on **Linux**. **Windows** has had limited testing but everything is
functional. **Not yet tested on macOS**. While the tooling used within YASAM is cross-platform there are extra factors
at play with how processes are invoked on a platform-by-platform basis so expect some things not to work until future
updates when running on a non linux system.

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

