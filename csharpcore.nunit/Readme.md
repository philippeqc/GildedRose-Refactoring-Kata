# CLI
All the following commands presume `csharpcore.nunit` as the current working directory.
## Build
`dotnet build`
## Run the GildedRoseTests project
This has the effect of running the `TextTestFixture` application.
`dotnet run --project .\GildedRoseTests\GildedRoseTests.csproj`
## Run all tests
`dotnet test`
### NUnit
#### Run all nunit test
`nunit3-console .\GildedRoseTests\bin\Debug\net7.0\GildedRoseTests.dll`
Nota: This generate the file `TestResult.xml`. See nunit3-console's documentation for more information.
#### Run a named test
To run the test `AgedBrieRaiseQualityWithTime`. See documentation for more information.
`nunit3-console .\GildedRoseTests\bin\Debug\net7.0\GildedRoseTests.dll --where:method==AgedBrieRaiseQualityWithTime`
### Approval Tests


### Verify