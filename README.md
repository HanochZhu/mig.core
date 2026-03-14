# Mig Core Framework

`com.mig.core` is the foundational package for MigSpace. It provides shared runtime and editor-side building blocks used by higher-level packages such as model loading, presentation, snapshot workflows, material editing, and task orchestration.

## Package Info

- Package name: `com.mig.core`
- Display name: `Mig Core Framework`
- Current version: `0.0.1`
- Unity version in `package.json`: `2019.4`

## What This Package Provides

This package contains the core abstractions and utilities used across the MigSpace ecosystem, including:

- Element-based data structures for recording and applying object changes
- Material wrapper utilities for runtime material manipulation
- Snapshot management for step-based presentation workflows
- Simple task-chain infrastructure for multi-step loading and processing
- FTP utilities for uploading and downloading project-related assets
- Editor extensions for inspecting and editing core data structures

## Main Modules

### `Mig.Core/Runtime/Element`

Defines the base element system used to capture and replay object state changes.

Examples include:

- `MigColorElement`
- `MigTranslateElement`
- `MigTransparencyElement`
- `MigSmoothnessElement`
- `MigNormalMapElement`

The abstract base type is `MigElement`, which exposes:

- `Record()` to capture state
- `Apply()` to restore state
- `Clone()` to duplicate element data

### `Mig.Core/Runtime/MigMaterial`

Provides wrappers around Unity materials so higher-level systems can work with a more consistent material abstraction.

Key types include:

- `MigMaterial`
- `MigMaterialLibrary`
- `MigMaterialWrapperBase`
- `MigLitMaterialWrapper`
- `MigGltfMaterialWrapper`

### `Mig.Core/Runtime/SnapshotManager`

Implements step-based snapshot management used for presentation and history-like workflows.

This module supports:

- creating and inserting snapshot steps
- deleting snapshot steps
- applying a target snapshot
- generating thumbnail images for steps

### `Mig.Core/Runtime/TaskPattern`

Contains a lightweight task-chain pattern for composing sequential operations.

Representative classes:

- `TaskHandlerBase`
- `LoadingFromCacheTask`
- `LoadingFromFTPTask`
- `UnzipProjectFileTask`

### `Mig.Core/Runtime/FTP`

Contains FTP-related helpers used by the project to exchange project files and assets with a remote server.

The main entry point is:

- `FTPClient`

If you use this package independently, make sure to update the FTP server address and credentials before runtime use.

### `Mig.Core/Editor`

Includes editor-only tooling for inspecting and maintaining core assets and wrappers inside Unity Editor.

## Folder Structure

```text
Packages/mig.core
|- package.json
|- README.md
|- Mig.Core
   |- Runtime
   |  |- Account
   |  |- Element
   |  |- Event
   |  |- FTP
   |  |- MigMaterial
   |  |- Model
   |  |- SnapshotManager
   |  |- StateMachine
   |  |- TaskPattern
   |  |- UIUtils
   |  |- Utils
   |- Editor
```

## Assemblies

This package includes two assemblies:

- Runtime assembly: `Mig.Core`
- Editor assembly: `com.mig.core.editor`

## How To Use

### Install through `manifest.json`

Add the package to your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.mig.core": "git@github.com:HanochZhu/mig.core.git"
  }
}
```

### Use in code

After Unity resolves the package, you can reference its namespaces in your scripts:

```csharp
using Mig.Core;
using Mig.Snapshot;
using Mig.Core.TaskPattern;
```

## Development Notes

- This package is designed as a shared infrastructure layer rather than a standalone app.
- Several runtime systems are intended to be used together with other Mig packages.
- Some configuration values are currently hard-coded and may need to be adapted for your environment.

## FTP Configuration

The FTP configuration currently lives in:

`Mig.Core/Runtime/FTP/FTPClient.cs`

Typical fields to review:

```csharp
private static string FTPCONSTR = "";
private static string FTPUSERNAME = "mig";
private static string FTPPASSWORD = "migassets";
```

## License

This package follows the license terms used by the main MigSpace repository.
