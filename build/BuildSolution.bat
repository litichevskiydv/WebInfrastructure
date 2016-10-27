dotnet restore "src\Infrastructure\Skeleton.Domain"
dotnet restore "src\Infrastructure\Skeleton.Web.Conventions"
dotnet restore "src\Infrastructure\Skeleton.Web.Serialization"
dotnet restore "src\Infrastructure\Skeleton.Integrations"
dotnet restore "src\Infrastructure\Skeleton.Web"
dotnet restore "src\Infrastructure\Skeleton.Web.Testing"
dotnet restore "src\Web.Application"
dotnet restore "src\Web"
dotnet restore "src\Web.Client"
dotnet restore "test\Web.Tests"

dotnet build "src\Infrastructure\Skeleton.Domain" -c %1 --no-dependencies
dotnet build "src\Infrastructure\Skeleton.Web.Conventions" -c %1 --no-dependencies
dotnet build "src\Infrastructure\Skeleton.Web.Serialization" -c %1 --no-dependencies
dotnet build "src\Infrastructure\Skeleton.Integrations" -c %1 --no-dependencies
dotnet build "src\Infrastructure\Skeleton.Web" -c %1 --no-dependencies
dotnet build "src\Infrastructure\Skeleton.Web.Testing" -c %1 --no-dependencies
dotnet build "src\Web.Application" -c %1 --no-dependencies
dotnet build "src\Web" -c %1 --no-dependencies
dotnet build "src\Web.Client" -c %1 --no-dependencies
dotnet build "test\Web.Tests" -c %1 --no-dependencies