powershell.exe -file build\SetFullDebugType.ps1
nuget install OpenCover -ExcludeVersion -OutputDirectory tools

tools\OpenCover\tools\OpenCover.Console.exe -target:"C:\Program Files\dotnet\dotnet.exe" -targetargs:" test ""test\Infrastructure\Common.Tests\Common.Tests.csproj"" -c Debug" -register:user -filter:"+[Skeleton*]* -[xunit*]*" -excludebyattribute:*.ExcludeFromCodeCoverage* -hideskipped:Filter;Attribute -threshold:100 -oldstyle -returntargetcode -output:coverage.xml
if %errorlevel% neq 0 exit /b %errorlevel%
tools\OpenCover\tools\OpenCover.Console.exe -target:"C:\Program Files\dotnet\dotnet.exe" -targetargs:" test ""test\Infrastructure\CQRS.Implementations.Tests\CQRS.Implementations.Tests.csproj"" -c Debug" -register:user -filter:"+[Skeleton*]* -[xunit*]*" -excludebyattribute:*.ExcludeFromCodeCoverage* -hideskipped:Filter;Attribute -threshold:100 -oldstyle -returntargetcode -mergeoutput -output:coverage.xml
if %errorlevel% neq 0 exit /b %errorlevel%
tools\OpenCover\tools\OpenCover.Console.exe -target:"C:\Program Files\dotnet\dotnet.exe" -targetargs:" test ""test\Infrastructure\Dapper.Tests\Dapper.Tests.csproj"" -c Debug" -register:user -filter:"+[Skeleton*]* -[xunit*]*" -excludebyattribute:*.ExcludeFromCodeCoverage* -hideskipped:Filter;Attribute -threshold:100 -oldstyle -returntargetcode -mergeoutput -output:coverage.xml
if %errorlevel% neq 0 exit /b %errorlevel%
tools\OpenCover\tools\OpenCover.Console.exe -target:"C:\Program Files\dotnet\dotnet.exe" -targetargs:" test ""test\Infrastructure\Web.Authentication.Tests\Web.Authentication.Tests.csproj"" -c Debug" -register:user -filter:"+[Skeleton*]* -[xunit*]*" -excludebyattribute:*.ExcludeFromCodeCoverage* -hideskipped:Filter;Attribute -threshold:100 -oldstyle -returntargetcode -mergeoutput -output:coverage.xml
if %errorlevel% neq 0 exit /b %errorlevel%
tools\OpenCover\tools\OpenCover.Console.exe -target:"C:\Program Files\dotnet\dotnet.exe" -targetargs:" test ""test\Infrastructure\Web.Tests\Web.Tests.csproj"" -c Debug" -register:user -filter:"+[Skeleton*]* -[xunit*]*" -excludebyattribute:*.ExcludeFromCodeCoverage* -hideskipped:Filter;Attribute -threshold:100 -oldstyle -returntargetcode -mergeoutput -output:coverage.xml
if %errorlevel% neq 0 exit /b %errorlevel%
tools\OpenCover\tools\OpenCover.Console.exe -target:"C:\Program Files\dotnet\dotnet.exe" -targetargs:" test ""test\Web.Tests\Web.Tests.csproj"" -c Debug" -register:user -filter:"+[Skeleton*]* -[xunit*]*" -excludebyattribute:*.ExcludeFromCodeCoverage* -hideskipped:Filter;Attribute -threshold:100 -oldstyle -returntargetcode -mergeoutput -output:coverage.xml
if %errorlevel% neq 0 exit /b %errorlevel%

SET PATH=C:\\Python34;C:\\Python34\\Scripts;%PATH%
pip install codecov
codecov -f "coverage.xml"