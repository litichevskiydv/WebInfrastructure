powershell.exe -file build\SetFullDebugType.ps1
nuget install OpenCover -ExcludeVersion -OutputDirectory tools

tools\OpenCover\tools\OpenCover.Console.exe -target:"C:\Program Files\dotnet\dotnet.exe" -targetargs:" test ""test\Infrastructure\Skeleton.Common.Tests"" -c Debug" -register:user -filter:"+[Skeleton*]* -[xunit*]*" -excludebyattribute:*.ExcludeFromCodeCoverage* -hideskipped:Filter;Attribute -threshold:100 -oldstyle -returntargetcode -output:coverage.xml
if %errorlevel% neq 0 exit /b %errorlevel%
tools\OpenCover\tools\OpenCover.Console.exe -target:"C:\Program Files\dotnet\dotnet.exe" -targetargs:" test ""test\Infrastructure\Skeleton.CQRS.Implementations.Tests"" -c Debug" -register:user -filter:"+[Skeleton*]* -[xunit*]*" -excludebyattribute:*.ExcludeFromCodeCoverage* -hideskipped:Filter;Attribute -threshold:100 -oldstyle -returntargetcode -mergeoutput -output:coverage.xml
if %errorlevel% neq 0 exit /b %errorlevel%
tools\OpenCover\tools\OpenCover.Console.exe -target:"C:\Program Files\dotnet\dotnet.exe" -targetargs:" test ""test\Infrastructure\Skeleton.Dapper.Tests"" -c Debug" -register:user -filter:"+[Skeleton*]* -[xunit*]*" -excludebyattribute:*.ExcludeFromCodeCoverage* -hideskipped:Filter;Attribute -threshold:100 -oldstyle -returntargetcode -mergeoutput -output:coverage.xml
if %errorlevel% neq 0 exit /b %errorlevel%
tools\OpenCover\tools\OpenCover.Console.exe -target:"C:\Program Files\dotnet\dotnet.exe" -targetargs:" test ""test\Infrastructure\Skeleton.Web.Authentication.Tests"" -c Debug" -register:user -filter:"+[Skeleton*]* -[xunit*]*" -excludebyattribute:*.ExcludeFromCodeCoverage* -hideskipped:Filter;Attribute -threshold:100 -oldstyle -returntargetcode -mergeoutput -output:coverage.xml
if %errorlevel% neq 0 exit /b %errorlevel%
tools\OpenCover\tools\OpenCover.Console.exe -target:"C:\Program Files\dotnet\dotnet.exe" -targetargs:" test ""test\Infrastructure\Skeleton.Web.Tests"" -c Debug" -register:user -filter:"+[Skeleton*]* -[xunit*]*" -excludebyattribute:*.ExcludeFromCodeCoverage* -hideskipped:Filter;Attribute -threshold:100 -oldstyle -returntargetcode -mergeoutput -output:coverage.xml
if %errorlevel% neq 0 exit /b %errorlevel%
tools\OpenCover\tools\OpenCover.Console.exe -target:"C:\Program Files\dotnet\dotnet.exe" -targetargs:" test ""test\Web.Tests"" -c Debug" -register:user -filter:"+[Skeleton*]* -[xunit*]*" -excludebyattribute:*.ExcludeFromCodeCoverage* -hideskipped:Filter;Attribute -threshold:100 -oldstyle -returntargetcode -mergeoutput -output:coverage.xml
if %errorlevel% neq 0 exit /b %errorlevel%

SET PATH=C:\\Python34;C:\\Python34\\Scripts;%PATH%
pip install codecov
codecov -f "coverage.xml"