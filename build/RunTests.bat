dotnet test "test\Infrastructure\Skeleton.Common.Tests" -c %1
if %errorlevel% neq 0 exit /b %errorlevel%
dotnet test "test\Infrastructure\Skeleton.CQRS.Implementations.Tests" -c %1
if %errorlevel% neq 0 exit /b %errorlevel%
dotnet test "test\Infrastructure\Skeleton.Dapper.Tests" -c %1
if %errorlevel% neq 0 exit /b %errorlevel%
dotnet test "test\Infrastructure\Skeleton.Web.Authentication.Tests" -c %1
if %errorlevel% neq 0 exit /b %errorlevel%
dotnet test "test\Infrastructure\Skeleton.Web.Tests" -c %1
if %errorlevel% neq 0 exit /b %errorlevel%
dotnet test "test\Web.Tests" -c %1
if %errorlevel% neq 0 exit /b %errorlevel%
