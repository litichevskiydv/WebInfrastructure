dotnet restore "src\Infrastructure\Skeleton.Common"
dotnet restore "src\Infrastructure\Skeleton.CQRS.Abstractions"
dotnet restore "src\Infrastructure\Skeleton.CQRS.Implementations"
dotnet restore "src\Infrastructure\Skeleton.Dapper"
dotnet restore "src\Infrastructure\Skeleton.Web.Conventions"
dotnet restore "src\Infrastructure\Skeleton.Web.Serialization"
dotnet restore "src\Infrastructure\Skeleton.Web.Integration"
dotnet restore "src\Infrastructure\Skeleton.Web.Authentication"
dotnet restore "src\Infrastructure\Skeleton.Web"
dotnet restore "src\Infrastructure\Skeleton.Web.Testing"
dotnet restore "src\Web.Domain"
dotnet restore "src\Web.DataAccess"
dotnet restore "src\Web.Application"
dotnet restore "src\Web"
dotnet restore "src\Web.Client"
dotnet restore "test\Infrastructure\Skeleton.Common.Tests"
dotnet restore "test\Infrastructure\Skeleton.CQRS.Implementations.Tests"
dotnet restore "test\Infrastructure\Skeleton.Dapper.Tests"
dotnet restore "test\Infrastructure\Skeleton.Web.Tests"
dotnet restore "test\Web.Tests"

dotnet build "src\Infrastructure\Skeleton.Common" -c %1 --no-dependencies
dotnet build "src\Infrastructure\Skeleton.CQRS.Abstractions" -c %1 --no-dependencies
dotnet build "src\Infrastructure\Skeleton.CQRS.Implementations" -c %1 --no-dependencies
dotnet build "src\Infrastructure\Skeleton.Dapper" -c %1 --no-dependencies
dotnet build "src\Infrastructure\Skeleton.Web.Conventions" -c %1 --no-dependencies
dotnet build "src\Infrastructure\Skeleton.Web.Serialization" -c %1 --no-dependencies
dotnet build "src\Infrastructure\Skeleton.Web.Integration" -c %1 --no-dependencies
dotnet build "src\Infrastructure\Skeleton.Web.Authentication" -c %1 --no-dependencies
dotnet build "src\Infrastructure\Skeleton.Web" -c %1 --no-dependencies
dotnet build "src\Infrastructure\Skeleton.Web.Testing" -c %1 --no-dependencies
dotnet build "src\Web.Domain" -c %1 --no-dependencies
dotnet build "src\Web.DataAccess" -c %1 --no-dependencies
dotnet build "src\Web.Application" -c %1 --no-dependencies
dotnet build "src\Web" -c %1 --no-dependencies
dotnet build "src\Web.Client" -c %1 --no-dependencies
dotnet build "test\Infrastructure\Skeleton.Common.Tests" -c %1 --no-dependencies
dotnet build "test\Infrastructure\Skeleton.CQRS.Implementations.Tests" -c %1 --no-dependencies
dotnet build "test\Infrastructure\Skeleton.Dapper.Tests" -c %1 --no-dependencies
dotnet build "test\Infrastructure\Skeleton.Web.Tests" -c %1 --no-dependencies
dotnet build "test\Web.Tests" -c %1 --no-dependencies