# Version Tracking - Legend of Mir Crystal

This document tracks all framework versions, language versions, NuGet packages, and external dependencies used across the Legend of Mir Crystal codebase.

## Core Framework & Language Versions

### .NET SDK
- **Version**: 8.0
- **Download**: https://dotnet.microsoft.com/en-us/download/dotnet/8.0

### C# Language
- **Version**: Implicit from .NET 8.0 (C# 12)
- **Nullable Reference Types**: Disabled (most projects) / Enabled (AutoPatcherAdmin, CustomFormControl)
- **Implicit Usings**: Enabled (all projects)

### Visual Studio
- **Minimum Version**: 17.5.33424.131
- **Recommended Version**: 17.8+
- **Format Version**: 12.00

## Project-Specific Target Frameworks

| Project | Target Framework | Output Type | Notes |
|---------|------------------|-------------|-------|
| Client | `net8.0-windows7.0` | WinExe | Windows Forms application |
| Server.Library | `net8.0` | Library | Core server library |
| Server (Server.MirForms) | `net8.0-windows7.0` | WinExe | Windows Forms application |
| Shared | `net8.0` | Library | Shared code library |
| AutoPatcherAdmin | `net8.0-windows7.0` | WinExe | Windows Forms application |
| LibraryEditor | `net8.0-windows7.0` | WinExe | Windows Forms application, x64 platform |
| LibraryViewer | `net8.0-windows7.0` | WinExe | Windows Forms application |
| CustomFormControl | `net8.0-windows7.0` | Library | Windows Forms control library |
| PatcherWebSite.Host | `net8.0` | Web | ASP.NET Core host serving `PatcherWebSite/mir2-patchsite` |

## NuGet Package Dependencies

### Client Project
- **Microsoft.AspNet.WebApi.Client**: 6.0.0
- **Microsoft.Web.WebView2**: 1.0.2903.40
- **NAudio**: 2.2.1

### Server Projects
- **log4net**: 3.0.3 (Server.Library, Server.MirForms)

### AutoPatcherAdmin Project
- **WinSCP**: 6.3.6

## External DLL Dependencies

All external DLLs are located in the `Components/` directory:

### Graphics & Rendering
- **SlimDX.dll** - DirectX wrapper library
  - Used by: Client
  - Location: `Components/SlimDX.dll`
  - Also includes: `SlimDX.pdb`, `SlimDX.xml`

### Compression Libraries
- **Ionic.Zlib.dll** - .NET compression library
  - Used by: LibraryEditor
  - Location: `Components/dotnet zlib/Ionic.Zlib.dll`

- **ManagedSquish.dll** - Texture compression library
  - Used by: LibraryEditor, LibraryViewer
  - Location: `Components/Squish 2.0/ManagedSquish.dll`
  - Also includes: `ManagedSquish.pdb`, `ManagedSquish.XML`

- **NativeSquish_x64.dll** - Native x64 texture compression
  - Location: `Components/Squish 2.0/NativeSquish_x64.dll`
  - Also includes: `NativeSquish_x64.pdb`

- **NativeSquish_x86.dll** - Native x86 texture compression
  - Location: `Components/Squish 2.0/NativeSquish_x86.dll`
  - Also includes: `NativeSquish_x86.pdb`

### Windows Forms Controls
- **CustomFormControl.dll** - Custom form control library
  - Used by: Server.MirForms, LibraryEditor, LibraryViewer
  - Location: `Components/CustomFormControl.dll`
  - Also includes: `CustomFormControl.pdb`
  - Source: `Controls/FixedListViewControl/CustomFormControl.csproj`

- **Microsoft.VisualBasic.PowerPacks.Vs.dll** - Visual Basic Power Packs
  - Used by: Server.MirForms
  - Location: `Components/Microsoft.VisualBasic.PowerPacks.Vs.dll`
  - Also includes: `Microsoft.VisualBasic.PowerPacks.Vs.xml`

## Build Requirements

### Development Environment
- **Visual Studio 2022** (v17.8+ recommended, v17.5.33424.131 minimum)
- **.NET 8 SDK** - Required for building all projects
- **.NET 8 Runtime** - Required for running Client and `PatcherWebSite.Host`

### Platform Targets
- **Client**: x64
- **LibraryEditor**: x64
- **Other Projects**: AnyCPU

### Build Configuration
- **Configurations**: Debug, Release
- **Platform**: Any CPU (most projects), x64 (Client, LibraryEditor)

## Project Compilation Settings

### Common Settings
- **Implicit Usings**: Enabled (all projects)
- **Nullable**: Disabled (most projects), Enabled (AutoPatcherAdmin, CustomFormControl)
- **AllowUnsafeBlocks**: True (Client, LibraryEditor, LibraryViewer)

### Output Paths
- **Client**: `Build/Client/`
- **Server**: `Build/Server/`
- **LibraryEditor**: `Build/Server Tools/LibraryEditor/`
- **LibraryViewer**: `Build/Server Tools/LibraryViewer/`
- **CustomFormControl**: `Components/`

## Solution Information

- **Solution File**: `Legend of Mir.sln`
- **Visual Studio Version**: 17.5.33424.131
- **Minimum Visual Studio Version**: 10.0.40219.1
- **Solution Format Version**: 12.00

## Additional Notes

- The project was upgraded to .NET 8.0 (documented in `dotnet-upgrade.txt`)
- Most projects use Windows Forms (`UseWindowsForms: true`)
- The patch site is hosted via `PatcherWebSite.Host` (ASP.NET Core on .NET 8)
- All .NET 8 projects use the modern SDK-style project format

## PatcherWebSite.Host publish notes

- Publish (example): `dotnet publish PatcherWebSite.Host/PatcherWebSite.Host.csproj -c Release`
- The publish output includes `patchsite/` content copied from `PatcherWebSite/mir2-patchsite` (no dependency on repo layout at deploy time).

---

**Last Updated**: Based on project files as of repository indexing
**Maintained By**: Development Team
