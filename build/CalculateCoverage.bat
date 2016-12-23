powershell.exe -file build\SetFullDebugType.ps1

nuget install OpenCover -ExcludeVersion -OutputDirectory tools
tools\OpenCover\tools\OpenCover.Console.exe -target:"C:\Program Files\dotnet\dotnet.exe" -targetargs:" test ""test\Infrastructure\Skeleton.Common.Tests"" -c Debug" -register:user -filter:"+[Skeleton*]* -[xunit*]*" -excludebyattribute:*.ExcludeFromCodeCoverage* -hideskipped:Filter;Attribute -threshold:100 -oldstyle -returntargetcode -output:coverage.xml
tools\OpenCover\tools\OpenCover.Console.exe -target:"C:\Program Files\dotnet\dotnet.exe" -targetargs:" test ""test\Infrastructure\Skeleton.CQRS.Implementations.Tests"" -c Debug" -register:user -filter:"+[Skeleton*]* -[xunit*]*" -excludebyattribute:*.ExcludeFromCodeCoverage* -hideskipped:Filter;Attribute -threshold:100 -oldstyle -returntargetcode -mergeoutput -output:coverage.xml
tools\OpenCover\tools\OpenCover.Console.exe -target:"C:\Program Files\dotnet\dotnet.exe" -targetargs:" test ""test\Infrastructure\Skeleton.Dapper.Tests"" -c Debug" -register:user -filter:"+[Skeleton*]* -[xunit*]*" -excludebyattribute:*.ExcludeFromCodeCoverage* -hideskipped:Filter;Attribute -threshold:100 -oldstyle -returntargetcode -mergeoutput -output:coverage.xml
tools\OpenCover\tools\OpenCover.Console.exe -target:"C:\Program Files\dotnet\dotnet.exe" -targetargs:" test ""test\Infrastructure\Skeleton.Web.Tests"" -c Debug" -register:user -filter:"+[Skeleton*]* -[xunit*]*" -excludebyattribute:*.ExcludeFromCodeCoverage* -hideskipped:Filter;Attribute -threshold:100 -oldstyle -returntargetcode -mergeoutput -output:coverage.xml
cd "test\Web.Tests"
..\..\tools\OpenCover\tools\OpenCover.Console.exe -target:"C:\Program Files\dotnet\dotnet.exe" -targetargs:" test -c Debug" -register:user -filter:"+[Skeleton*]* -[xunit*]*" -excludebyattribute:*.ExcludeFromCodeCoverage* -hideskipped:Filter;Attribute -threshold:100 -oldstyle -returntargetcode -mergeoutput -output:..\..\coverage.xml
cd "..\.."

SET PATH=C:\\Python34;C:\\Python34\\Scripts;%PATH%
pip install codecov
codecov -f "coverage.xml"