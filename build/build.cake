// Target - The task you want to start. Runs the Default task if not specified.
var target = Argument("Target", "Default");

// Configuration - The build configuration (Debug/Release) to use.
// 1. If command line parameter parameter passed, use that.
// 2. Otherwise if an Environment variable exists, use that.
var configuration = 
    HasArgument("Configuration") 
        ? Argument<string>("Configuration") 
        : EnvironmentVariable("Configuration") ?? "Release";

// The build number to use in the version number of the built NuGet packages.
// There are multiple ways this value can be passed, this is a common pattern.
// 1. If command line parameter parameter passed, use that.
// 2. Otherwise if running on AppVeyor, get it's build number.
// 3. Otherwise if running on Travis CI, get it's build number.
// 4. Otherwise if an Environment variable exists, use that.
// 5. Otherwise default the build number to 0.
var buildNumber =
    HasArgument("BuildNumber") ? Argument<int>("BuildNumber") :
    AppVeyor.IsRunningOnAppVeyor ? AppVeyor.Environment.Build.Number :
    TravisCI.IsRunningOnTravisCI ? TravisCI.Environment.Build.BuildNumber :
    EnvironmentVariable("BuildNumber") != null ? int.Parse(EnvironmentVariable("BuildNumber")) : 0;

// Packages version in format major.minor.patch
var version = HasArgument("ShortVersion") ? Argument<string>("ShortVersion") : EnvironmentVariable("ShortVersion");
version = !string.IsNullOrWhiteSpace(version) ? version : "1.0.0";
var assemblyVersion = $"{version}.{buildNumber}";

// Text suffix of the package version
var versionSuffix = HasArgument("VersionSuffix") ? Argument<string>("VersionSuffix") : EnvironmentVariable("VersionSuffix");
var packageVersion = version + (!string.IsNullOrWhiteSpace(versionSuffix) ? $"-{versionSuffix}-build{buildNumber}" : "");
 
// A directory path to an Artifacts directory.
var artifactsDirectory = MakeAbsolute(Directory("./artifacts"));
 
// Deletes the contents of the Artifacts folder if it should contain anything from a previous build.
Task("Clean")
    .Does(() =>
    {
        CleanDirectory(artifactsDirectory);
    });
 
// Find all csproj projects and build them using the build configuration specified as an argument.
 Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        var projects = GetFiles("../src/**/*.csproj").Concat(GetFiles("../test/**/*.csproj"));
        foreach(var project in projects)
        {
            DotNetCoreBuild(
                project.GetDirectory().FullPath,
                new DotNetCoreBuildSettings()
                {
                    Configuration = configuration,
                    ArgumentCustomization = args => args
                        .Append($"/p:Version={version}")
                        .Append($"/p:AssemblyVersion={assemblyVersion}")
                });
        }
    });

// Run dotnet pack to produce NuGet packages from our projects. Versions the package
// using the build number argument on the script which is used as the revision number 
// (Last number in 1.0.0.0). The packages are dropped in the Artifacts directory.
Task("Pack")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var projects = GetFiles("../src/Infrastructure/**/*.csproj");
        foreach (var project in projects)
        {
            DotNetCorePack(
                project.GetDirectory().FullPath,
                new DotNetCorePackSettings()
                {
                    Configuration = configuration,
                    NoRestore = true,
                    NoBuild = true,
                    OutputDirectory = artifactsDirectory,
                    IncludeSymbols = true,
                    ArgumentCustomization = args => args
                        .Append($"/p:PackageVersion={packageVersion}")
                });
        }
    });

// Look under a 'Tests' folder and run dotnet test against all of those projects.
// Then drop the XML test results file in the Artifacts folder at the root.
Task("Test")
    .IsDependentOn("Pack")
    .Does(() =>
    {
        var projects = GetFiles("../test/**/*.csproj");
        foreach(var project in projects)
        {
            DotNetCoreTool(
                project.FullPath,
                "xunit",
                new ProcessArgumentBuilder() 
                    .Append("-configuration " + configuration)
                    .Append("-nobuild")
                    .Append($"-xml {artifactsDirectory.CombineWithFilePath(project.GetFilenameWithoutExtension()).FullPath}.xml")
                );
        }
    });

// Look under a 'test' folder and calculate tests against all of those projects.
// Then drop the XML test results file in the artifacts folder at the root.
#tool "nuget:?package=OpenCover"
#tool "nuget:?package=Codecov&version=1.0.3"
#addin "nuget:?package=Cake.Codecov"
Task("CalculateCoverage")
    .IsDependentOn("Pack")
    .Does(() =>
    {
        var projects = GetFiles("../src/Infrastructure/**/*.csproj");
        foreach(var project in projects)
        {
            TransformTextFile(project.FullPath, ">", "<")
                .WithToken("portable", ">full<")
                .Save(project.FullPath);
        }

        projects = GetFiles("../test/**/*.csproj");
        var resultsFile = artifactsDirectory.CombineWithFilePath("coverage.xml");
        foreach(var project in projects)
        {
            OpenCover(
                x => x.DotNetCoreTest(
                     project.FullPath,
                     new DotNetCoreTestSettings() { Configuration = "Debug" }
                ),
                resultsFile,
                new OpenCoverSettings()
                {
                    ArgumentCustomization = args => args
                        .Append("-threshold:100")
                        .Append("-returntargetcode")
                        .Append("-hideskipped:Filter;Attribute"),
                    Register = "user",
                    OldStyle = true,
                    MergeOutput = true
                }
                    .WithFilter("+[Skeleton*]*")
                    .WithFilter("-[xunit*]*")
                    .ExcludeByAttribute("*.ExcludeFromCodeCoverage*")
            );
        }

        Codecov(resultsFile.FullPath);
    });
 
// The default task to run if none is explicitly specified. In this case, we want
// to run everything starting from Clean, all the way up to Test.
Task("Default")
    .IsDependentOn("Test");
 
// Executes the task specified in the target argument.
RunTarget(target);