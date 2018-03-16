# Module Rotator Module for DNN (DotNetNuke)

Module Rotator is a DNN module that allows you to rotate the content of multiple modules in the space of a single module.

## Minimum DNN Version

Current releases support DNN 7.2.0 and later

## Releases

[Releases](https://github.com/redtempo/dnnstuff.modulerotator/releases)

## Documentation

[Documentation](https://redtempo.github.io/dnnstuff.modulerotator/)

## Building Extension Package

To build a package for installing with the DNN extension installer do the following:

Drop to a command line and go into the build folder

Run `build.bat [version] [configuration]`

where:

* version is the version formatted as major.minor.patch (ie. 01.04.05)
* configuration is the build configuration to use (Debug or Release)

Example:

* `build.bat 01.04.05 Release` will created a release build with version 01.04.05

All builds are written into the Build\Deploy folder
