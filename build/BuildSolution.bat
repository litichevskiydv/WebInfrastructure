dotnet restore "src\Infrastructure\Common"
dotnet restore "src\Infrastructure\CQRS.Abstractions"
dotnet restore "src\Infrastructure\CQRS.Implementations"
dotnet restore "src\Infrastructure\Dapper"
dotnet restore "src\Infrastructure\Web.Conventions"
dotnet restore "src\Infrastructure\Web.Serialization"
dotnet restore "src\Infrastructure\Web.Integration"
dotnet restore "src\Infrastructure\Web.Authentication"
dotnet restore "src\Infrastructure\Web"
dotnet restore "src\Infrastructure\Web.Testing"
dotnet restore "src\Web.Domain"
dotnet restore "src\Web.DataAccess"
dotnet restore "src\Web.Application"
dotnet restore "src\Web"
dotnet restore "src\Web.Client"
dotnet restore "test\Infrastructure\Common.Tests"
dotnet restore "test\Infrastructure\CQRS.Implementations.Tests"
dotnet restore "test\Infrastructure\Dapper.Tests"
dotnet restore "test\Infrastructure\Web.Authentication.Tests"
dotnet restore "test\Infrastructure\Web.Tests"
dotnet restore "test\Web.Tests"

dotnet build "src\Infrastructure\Common" -c %1 --no-dependencies
dotnet build "src\Infrastructure\CQRS.Abstractions" -c %1 --no-dependencies
dotnet build "src\Infrastructure\CQRS.Implementations" -c %1 --no-dependencies
dotnet build "src\Infrastructure\Dapper" -c %1 --no-dependencies
dotnet build "src\Infrastructure\Web.Conventions" -c %1 --no-dependencies
dotnet build "src\Infrastructure\Web.Serialization" -c %1 --no-dependencies
dotnet build "src\Infrastructure\Web.Integration" -c %1 --no-dependencies
dotnet build "src\Infrastructure\Web.Authentication" -c %1 --no-dependencies
dotnet build "src\Infrastructure\Web" -c %1 --no-dependencies
dotnet build "src\Infrastructure\Web.Testing" -c %1 --no-dependencies
dotnet build "src\Web.Domain" -c %1 --no-dependencies
dotnet build "src\Web.DataAccess" -c %1 --no-dependencies
dotnet build "src\Web.Application" -c %1 --no-dependencies
dotnet build "src\Web" -c %1 --no-dependencies
dotnet build "src\Web.Client" -c %1 --no-dependencies
dotnet build "test\Infrastructure\Common.Tests" -c %1 --no-dependencies
dotnet build "test\Infrastructure\CQRS.Implementations.Tests" -c %1 --no-dependencies
dotnet build "test\Infrastructure\Dapper.Tests" -c %1 --no-dependencies
dotnet build "test\Infrastructure\Web.Authentication.Tests" -c %1 --no-dependencies
dotnet build "test\Infrastructure\Web.Tests" -c %1 --no-dependencies
dotnet build "test\Web.Tests" -c %1 --no-dependencies