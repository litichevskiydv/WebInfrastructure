dotnet pack "src\Infrastructure\Skeleton.Domain" -c %1 --version-suffix %2
dotnet pack "src\Infrastructure\Skeleton.Web.Conventions" -c %1 --version-suffix %2
dotnet pack "src\Infrastructure\Skeleton.Web.Serialization" -c %1 --version-suffix %2
dotnet pack "src\Infrastructure\Skeleton.Integrations" -c %1 --version-suffix %2
dotnet pack "src\Infrastructure\Skeleton.Web" -c %1 --version-suffix %2
dotnet pack "src\Infrastructure\Skeleton.Web.Testing" -c %1 --version-suffix %2