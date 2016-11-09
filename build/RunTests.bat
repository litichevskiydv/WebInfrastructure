dotnet test "test\Infrastructure\Skeleton.Dapper.Tests" -c %1

cd "test\Web.Tests"
dotnet test -c %1