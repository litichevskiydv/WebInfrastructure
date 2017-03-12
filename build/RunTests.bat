dotnet test "test\Infrastructure\Common.Tests\Common.Tests.csproj" -c %1 --no-build
if %errorlevel% neq 0 exit /b %errorlevel%
dotnet test "test\Infrastructure\CQRS.Implementations.Tests\CQRS.Implementations.Tests.csproj" -c %1 --no-build
if %errorlevel% neq 0 exit /b %errorlevel%
dotnet test "test\Infrastructure\Dapper.Tests\Dapper.Tests.csproj" -c %1 --no-build
if %errorlevel% neq 0 exit /b %errorlevel%
dotnet test "test\Infrastructure\Web.Authentication.Tests\Web.Authentication.Tests.csproj" -c %1 --no-build
if %errorlevel% neq 0 exit /b %errorlevel%
dotnet test "test\Infrastructure\Web.Tests\Web.Tests.csproj" -c %1 --no-build
if %errorlevel% neq 0 exit /b %errorlevel%
dotnet test "test\Web.Tests\Web.Tests.csproj" -c %1 --no-build
if %errorlevel% neq 0 exit /b %errorlevel%
