![Status](https://github.com/xeniorn/SimpleXmpLib/actions/workflows/dotnet.yml/badge.svg?branch=develop)
![NuGet Version](https://img.shields.io/nuget/v/SimpleXmpLib)

SimpleXmpLib provides a convenient way to work with XMP metadata containers, and embedding such containers in supported files.

It uses a simplified interface based on the C++-based [Adobe XMP Toolkit](https://github.com/adobe/XMP-Toolkit-SDK).

Implementation v1 re-wraps an existing .NET based wrapper [xmp-sharp](https://github.com/xeniorn/xmp-sharp) and uses a precompiled XMP Toolkit v2025.03 for the x64 platform only.

## License
The original code in this project has no licensing restrictions and uses the "Unlicense" license. See [LICENSE](LICENSE) for more information.

Note that you still need to comply with the licensing restrictions of third-party dependencies this code is using.

### Third-Party Licenses

This project uses the following third-party libraries:

- **xmp-sharp** - [Source](https://github.com/xeniorn/xmp-sharp) | [BSD 3-Clause License](extern_dependencies/licensing/xmp-sharp/LICENSE.md)
- **Adobe XMP Toolkit** - [Source](https://github.com/adobe/XMP-Toolkit-SDK) | [BSD 3-Clause License](extern_dependencies/licensing/adobe-xmp-toolkit/LICENSE.md)
- **zlib** - [Source](https://github.com/madler/zlib) | [License](extern_dependencies/licensing/zlib/LICENSE.md)
- **libexpat** - [Source](https://github.com/libexpat/libexpat) | [License](extern_dependencies/licensing/libexpat/LICENSE.md)
	
Find all licenses under [extern_dependencies/licensing](extern_dependencies/licensing)