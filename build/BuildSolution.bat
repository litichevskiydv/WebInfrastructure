dotnet restore "src\Infrastructure\Common" --no-dependencies
dotnet restore "src\Infrastructure\CQRS.Abstractions" --no-dependencies
dotnet restore "src\Infrastructure\CQRS.Implementations" --no-dependencies
dotnet restore "src\Infrastructure\Dapper" --no-dependencies
dotnet restore "src\Infrastructure\Migrations" --no-dependencies
dotnet restore "src\Infrastructure\Web.Conventions" --no-dependencies
dotnet restore "src\Infrastructure\Web.Serialization" --no-dependencies
dotnet restore "src\Infrastructure\Web.Integration" --no-dependencies
dotnet restore "src\Infrastructure\Web.Authentication" --no-dependencies
dotnet restore "src\Infrastructure\Web" --no-dependencies
dotnet restore "src\Infrastructure\Web.Testing" --no-dependencies
dotnet restore "src\Web.Migrations" --no-dependencies
dotnet restore "src\Web.Domain" --no-dependencies
dotnet restore "src\Web.DataAccess" --no-dependencies
dotnet restore "src\Web.Application" --no-dependencies
dotnet restore "src\Web" --no-dependencies
dotnet restore "src\Web.Client" --no-dependencies
dotnet restore "test\Infrastructure\Common.Tests" --no-dependencies
dotnet restore "test\Infrastructure\CQRS.Implementations.Tests" --no-dependencies
dotnet restore "test\Infrastructure\Dapper.Tests" --no-dependencies
dotnet restore "test\Infrastructure\Web.Authentication.Tests" --no-dependencies
dotnet restore "test\Infrastructure\Web.Tests" --no-dependencies
dotnet restore "test\Web.Tests" --no-dependencies

dotnet build "src\Infrastructure\Common" -c %1 --no-dependencies
dotnet build "src\Infrastructure\CQRS.Abstractions" -c %1 --no-dependencies
dotnet build "src\Infrastructure\CQRS.Implementations" -c %1 --no-dependencies
dotnet build "src\Infrastructure\Dapper" -c %1 --no-dependencies
dotnet build "src\Infrastructure\Migrations" -c %1 --no-dependencies
dotnet build "src\Infrastructure\Web.Conventions" -c %1 --no-dependencies
dotnet build "src\Infrastructure\Web.Serialization" -c %1 --no-dependencies
dotnet build "src\Infrastructure\Web.Integration" -c %1 --no-dependencies
dotnet build "src\Infrastructure\Web.Authentication" -c %1 --no-dependencies
dotnet build "src\Infrastructure\Web" -c %1 --no-dependencies
dotnet build "src\Infrastructure\Web.Testing" -c %1 --no-dependencies
dotnet build "src\Web.Migrations" -c %1 --no-dependencies
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