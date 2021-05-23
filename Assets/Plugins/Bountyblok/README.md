**This library allows you to easily use the bountyblok.io APIs via C#**

bountyblok.io is a gamification engine powered by the EOS blockchain.

This library is a quick and easy way to Log tasks for challenges as well as retrieve progresses/badges/achievements and more. We will keep updating this library to expose our API calls in the future!

Thanks for your support! 

# Installation

## Prerequisites

- .NET version 4.5.2 and higher
- .NET Core 1.0 and higher
- .NET Standard 1.3 support
- A bountyblok.io account

## Obtain an API Key

Grab your API Key from the [bountyblok.io UI](https://app.bountyblok.io/settings/api_keys).

## Install Package

To use bountyblok in your C# project, you can either <a href="https://github.com/bountyblok/bountyblok-csharp.git">download the bountyblok C# .NET libraries directly from our Github repository</a> or if you have the NuGet package manager installed, you can grab them automatically:

```
PM> Install-Package Bountyblok
```

<a name="general"></a>
## Examples

```csharp
using bountyblok.client;
using System.Threading.Tasks;
```

## /v1/log_app

```csharp
BountyblokClient client = new BountyblokClient("<< API_KEY >>");

var response = await client.LogAppAsync(new LogAppRequest
{
    AccountName = "user123",
    AppID = Guid.Parse("<< app_id >>"),
    Quantity = 1,
    Param = param
});
```

## /v1/get_challenge_progress

```csharp
BountyblokClient bbClient = new BountyblokClient("<< API_KEY >>");

GetChallengeResponse response = await bbClient.GetChallengeAsync(new GetChallengeRequest
{
    AccountName = accountname,
    ChallengeID = Guid.Parse("<< challenge_id >>")
});
```

<a name="license"></a>
# License
[The MIT License (MIT)](https://github.com/bountyblok/bountyblok-csharp/blob/master/LICENSE.md)
