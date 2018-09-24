## CoreAPIDirect

### REST API library for ASP.NET Core

[![license](https://img.shields.io/github/license/pavlospap/coreapidirect.svg)](https://opensource.org/licenses/MIT)

[![GitHub forks](https://img.shields.io/github/forks/pavlospap/coreapidirect.svg?style=social&label=Fork)](https://github.com/pavlospap/coreapidirect/fork)
[![GitHub stars](https://img.shields.io/github/stars/pavlospap/coreapidirect.svg?style=social&label=Star)](https://github.com/pavlospap/coreapidirect)

### Table of Contents

* [Installation](#installation)
* [Usage](#usage)
* [Documentation](#documentation)
* [License](#license)

## Installation

```bash
Install-Package CoreAPIDirect
```

## Usage

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Add with default options
    services.AddCoreApiDirect();

    // Add and configure options at once
    services.AddCoreApiDirect(options =>
    {
        options.MaxPageSize = 25;
        options.PageSize = 15;
    });

    // Configure options first and then add
    services.Configure<CoreOptions>(options =>
    {
        options.MaxPageSize = 25;
        options.PageSize = 15;
    });
    services.AddCoreApiDirect();
}
```

## Documentation

The documentation for the CoreAPIDirect library is located in this repo's [wiki](https://github.com/pavlospap/coreapidirect/wiki).

## License

[MIT](https://github.com/pavlospap/CoreAPIDirect/blob/master/LICENSE)