#tool "nuget:?package=OpenCover"
#tool "nuget:?package=Codecov&version=1.0.4"
#addin "nuget:?package=Cake.Codecov"
using System.Text.RegularExpressions;

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

// The branch name use in version suffix
var branch = 
    AppVeyor.IsRunningOnAppVeyor ? AppVeyor.Environment.Repository.Branch :
    TravisCI.IsRunningOnTravisCI ? TravisCI.Environment.Build.Branch : (string)null;

// Text suffix of the package version
string versionSuffix = null;
if(string.IsNullOrWhiteSpace(branch) == false && branch != "master")
{
    versionSuffix = $"-dev-build{buildNumber:00000}";

    var match = Regex.Match(branch, "release\\/\\d+\\.\\d+\\.\\d+\\-?(.*)", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
    if(match.Success)
        versionSuffix = string.IsNullOrWhiteSpace(match.Groups[1].Value) == false
            ? $"{match.Groups[1].Value}-build{buildNumber:00000}"
            : $"build{buildNumber:00000}";
    Information($"Version suffix:{versionSuffix}");
}
Information($"Branch name:{branch}");
 
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
        var settings = new DotNetCoreBuildSettings
                {
                    Configuration = configuration,
                    VersionSuffix = versionSuffix
                };
        if(buildNumber != 0)
            settings.ArgumentCustomization = x => x.Append($"/p:Build={buildNumber}");

        foreach(var project in projects)
            DotNetCoreBuild(project.GetDirectory().FullPath, settings);
    });

// Run dotnet pack to produce NuGet packages from our projects. Versions the package
// using the build number argument on the script which is used as the revision number 
// (Last number in 1.0.0.0). The packages are dropped in the Artifacts directory.
Task("Pack")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var projects = GetFiles("../src/Infrastructure/**/*.csproj");
        var settings = new DotNetCorePackSettings
                {
                    Configuration = configuration,
                    NoRestore = true,
                    NoBuild = true,
                    OutputDirectory = artifactsDirectory,
                    IncludeSymbols = true,
                    VersionSuffix = versionSuffix
                };

        foreach (var project in projects)
            DotNetCorePack(project.GetDirectory().FullPath, settings);
    });

// Look under a 'Tests' folder and run dotnet test against all of those projects.
// Then drop the XML test results file in the Artifacts folder at the root.
Task("Test")
    .IsDependentOn("Pack")
    .Does(() =>
    {
        var projects = GetFiles("../test/**/*.csproj");
        var settings = new DotNetCoreTestSettings
        {
            Configuration = configuration,
            NoRestore = true,
            NoBuild = true,
        };

        foreach(var project in projects)
            DotNetCoreTest(project.FullPath, settings);
    });

// Look under a 'test' folder and calculate tests against all of those projects.
// Then drop the XML test results file in the artifacts folder at the root.
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
        var settings = new OpenCoverSettings
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
                .ExcludeByAttribute("*.ExcludeFromCodeCoverage*");

        foreach(var project in projects)
            OpenCover(
                x => x.DotNetCoreTest(
                     project.FullPath,
                     new DotNetCoreTestSettings { Configuration = "Debug" }
                ),
                resultsFile,
                settings
            );

        Codecov(resultsFile.FullPath);
    });
 
// The default task to run if none is explicitly specified. In this case, we want
// to run everything starting from Clean, all the way up to Test.
Task("Default")
    .IsDependentOn("Test");
 
// Executes the task specified in the target argument.
RunTarget(target);