# Bannerlord.NexusmodsUploader
Generic mod file uploader. Is mainly used for M&amp;B2:B, but can be used for any other game. Example of usage in WebSite.

### Requirements
Requires BUTR GitHub Package Registry to be added.
```shell
nuget sources add -name "BUTR" -Source https://nuget.pkg.github.com/BUTR/index.json -Username YOURGITHUBUSERNAME -Password YOURGITHUBTOKEN
```

Requires ``NEXUSMODS_COOKIES_JSON`` environment variable:
```json
[
    {
        "Id":"member_id",
        "Value":"%SET_VALUE%",
        "Domain":".nexusmods.com",
        "Path":"/",
        "Date":"%SET_VALUE%"
    },
    {
        "Id":"pass_hash",
        "Value":"%SET_VALUE%",
        "Domain":".nexusmods.com",
        "Path":"/",
        "Date":"%SET_VALUE%"
    },
    {
        "Id":"sid",
        "Value":"%SET_VALUE%",
        "Domain":".nexusmods.com",
        "Path":"/",
        "Date":"%SET_VALUE%"
    }
]
```

### Installation
```shell
dotnet tool install -g Bannerlord.NexusmodsUploader
```

### Example
When installed as a global tool:
```shell
bannerlord_nexusmods_uploader upload -g mountandblade2bannerlord -m 612 -n "Mod Configuration Menu" -v "v3.1.0" -l true -e true -d "MULTILINE\nCHANGELOG" -p "$PWD/MCM.Standalone.zip";
```
