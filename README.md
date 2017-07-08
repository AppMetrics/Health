# App Metrics Health <img src="http://app-metrics.io/logo.png" alt="App Metrics" width="50px"/> 
[![Official Site](https://img.shields.io/badge/site-appmetrics-blue.svg?style=flat-square)](http://app-metrics.io/getting-started/intro.html) [![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg?style=flat-square)](https://opensource.org/licenses/Apache-2.0) [![Gitter](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/app-metrics/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

## What is App Metrics Health?

App Metrics Health is an open-source and cross-platform .NET library used define health checks within an application, see the [Getting Started Guide](http://app-metrics.io/getting-started/health-checks/index.html).

## Latest Builds, Packages & Repo Stats

|Branch|AppVeyor|Travis|Coverage|
|------|:--------:|:--------:|:--------:|
|dev|[![AppVeyor](https://img.shields.io/appveyor/ci/alhardy/health/dev.svg?style=flat-square&label=appveyor%20build)](https://ci.appveyor.com/project/alhardy/health/branch/dev)|[![Travis](https://img.shields.io/travis/alhardy/health/dev.svg?style=flat-square&label=travis%20build)](https://travis-ci.org/alhardy/health)|[![Coveralls](https://img.shields.io/coveralls/alhardy/health/dev.svg?style=flat-square)](https://coveralls.io/github/alhardy/health?branch=dev)
|master|[![AppVeyor](https://img.shields.io/appveyor/ci/alhardy/health/master.svg?style=flat-square&label=appveyor%20build)](https://ci.appveyor.com/project/alhardy/health/branch/master)| [![Travis](https://img.shields.io/travis/alhardy/health/master.svg?style=flat-square&label=travis%20build)](https://travis-ci.org/alhardy/health)| [![Coveralls](https://img.shields.io/coveralls/alhardy/health/master.svg?style=flat-square)](https://coveralls.io/github/alhardy/health?branch=master)|

|Package|Dev Release|Pre-Release|Release|
|------|:--------:|:--------:|:--------:|
|App.Metrics.Health|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.Health.svg?style=flat-square)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.Health)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.Health.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Health/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.Health.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Health/)
|App.Metrics.Health.Abstractions|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.Health.Abstractions.svg?style=flat-square0)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.Health.Abstractions)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.Health.Abstractions.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Health.Abstractions/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.Health.Abstractions.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Health.Abstractions/)
|App.Metrics.Health.Formatters.Ascii|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.Health.Formatters.Ascii.svg?style=flat-square&maxAge=7200)](https://www.myget.org/feed/appmetrics/package/nuget/AApp.Metrics.Health.Formatters.Ascii)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.Health.Formatters.Ascii.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Health.Formatters.Ascii/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.Health.Formatters.Ascii.svg)](https://www.nuget.org/packages/App.Metrics.Health.Formatters.Ascii/)
|App.Metrics.Health.Formatters.Json|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.Health.Formatters.Json.svg?style=flat-square)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.Health.Formatters.Json)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.Health.Formatters.Json.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Health.Formatters.Json/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.Health.Formatters.Json.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Health.Formatters.Json/)|
----------

## How to build

[AppVeyor](https://ci.appveyor.com/project/alhardy/health/branch/master) and [Travis CI](https://travis-ci.org/alhardy/health) builds are triggered on commits and PRs to `dev` and `master` branches.

See the following for build arguments and running locally.

|Configuration|Description|Default|Environment|Required|
|------|--------|:--------:|:--------:|:--------:|
|BuildConfiguration|The configuration to run the build, **Debug** or **Release** |*Release*|All|Optional|
|PreReleaseSuffix|The pre-release suffix for versioning nuget package artifacts e.g. `beta`|*ci*|All|Optional|
|CoverWith|**DotCover** or **OpenCover** to calculate and report code coverage, **None** to skip. When not **None**, a coverage file and html report will be generated at `./artifacts/coverage`|*OpenCover*|Windows Only|Optional|
|SkipCodeInspect|**false** to run ReSharper code inspect and report results, **true** to skip. When **true**, the code inspection html report and xml output will be generated at `./artifacts/resharper-reports`|*false*|Windows Only|Optional|
|BuildNumber|The build number to use for pre-release versions|*0*|All|Optional|


### Windows

Run `build.ps1` from the repositories root directory.

```
	.\build.ps1
```

**With Arguments**

```
	.\build.ps1 --ScriptArgs '-BuildConfiguration=Release -PreReleaseSuffix=beta -CoverWith=OpenCover -SkipCodeInspect=false -BuildNumber=1'
```

### Linux & OSX

Run `build.sh` from the repositories root directory. Code Coverage reports are now supported on Linux and OSX, it will be skipped running in these environments.

```
	.\build.sh
```

**With Arguments**


```
	.\build.sh --ScriptArgs '-BuildConfiguration=Release -PreReleaseSuffix=beta -BuildNumber=1'
```

## Contributing

See the [contribution guidlines](CONTRIBUTING.md) for details.

## Acknowledgements

* [ASP.NET Core](https://github.com/aspnet)
* [Grafana](https://grafana.com/)
* [DocFX](https://dotnet.github.io/docfx/)
* [Fluent Assertions](http://www.fluentassertions.com/)
* [xUnit.net](https://xunit.github.io/)
* [StyleCopAnalyzers](https://github.com/DotNetAnalyzers/StyleCopAnalyzers)
* [Cake](https://github.com/cake-build/cake)
* [OpenCover](https://github.com/OpenCover/opencover)

***Thanks for providing free open source licensing***

* [NDepend](http://www.ndepend.com/) 
* [Jetbrains](https://www.jetbrains.com/dotnet/) 
* [AppVeyor](https://www.appveyor.com/)
* [Travis CI](https://travis-ci.org/)
* [Coveralls](https://coveralls.io/)

## License

This library is release under Apache 2.0 License ( see LICENSE ) Copyright (c) 2016 Allan Hardy

See [LICENSE](https://github.com/alhardy/AppMetrics/blob/dev/LICENSE)

----------

App Metrics is based on the [Metrics.NET](https://github.com/etishor/Metrics.NET) library, and at the moment uses the same reservoir sampling code from the original library which is a port of the Java [Dropwizard Metrics](https://github.com/dropwizard/metrics) library. 

*Metrics.NET Licensed under these terms*:
"Metrics.NET is release under Apache 2.0 License Copyright (c) 2014 Iulian Margarintescu" see [LICENSE](https://github.com/etishor/Metrics.NET/blob/master/LICENSE)

*Dropwizard Metrics* Licensed under these terms*:
"Copyright (c) 2010-2013 Coda Hale, Yammer.com Published under Apache Software License 2.0, see [LICENSE](https://github.com/dropwizard/metrics/blob/3.2-development/LICENSE)"

----------
[![Powered By NDepend](https://github.com/alhardy/AppMetrics.DocFx/blob/master/images/PoweredByNDepend.png)](http://www.ndepend.com/)

----------
