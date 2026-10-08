using System.Collections.Concurrent;
using System.Diagnostics;

// The CI gate: the one definition of "green" for TheProject.
// GitHub Actions runs this file (.github/workflows/ci.yml) and so do you:
//   dotnet run build/ci.cs              live output of every step
//   dotnet run build/ci.cs -- --quiet   only the output of the step that fails

Step[] steps =
[
    new("Restore", ["restore", "TheProject.slnx"]),
    new("Format", ["format", "TheProject.slnx", "--verify-no-changes", "--no-restore"]),
    new("Build", ["build", "TheProject.slnx", "--configuration", "Release", "--no-restore"]),
    new("Test", ["test", "--solution", "TheProject.slnx", "--configuration", "Release", "--no-build"]),
];

bool quiet = args.Contains("--quiet");
bool onGitHubActions = Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true";
string repositoryRoot = Path.GetFullPath("..", (string)AppContext.GetData("EntryPointFileDirectoryPath")!);

var results = new List<StepResult>();
foreach (Step step in steps)
{
    Console.WriteLine(onGitHubActions ? $"::group::{step.Name}" : $"==> {step.Name}: dotnet {string.Join(' ', step.Arguments)}");
    var stopwatch = Stopwatch.StartNew();
    (int exitCode, IReadOnlyCollection<string> output) = await RunAsync(step, repositoryRoot, quiet);
    results.Add(new StepResult(step.Name, exitCode == 0, stopwatch.Elapsed));
    if (onGitHubActions)
    {
        Console.WriteLine("::endgroup::");
    }

    if (exitCode != 0)
    {
        foreach (string line in output)
        {
            Console.WriteLine(line);
        }

        break;
    }
}

Console.WriteLine();
foreach (Step step in steps)
{
    StepResult? result = results.Find(r => r.Name == step.Name);
    string outcome = result switch
    {
        null => "skipped",
        { Passed: true } => "passed",
        _ => "FAILED",
    };
    string elapsed = result is null ? "" : $"{result.Elapsed.TotalSeconds,6:F1}s";
    Console.WriteLine($"{step.Name,-8} {outcome,-8} {elapsed}".TrimEnd());
}

return results.TrueForAll(r => r.Passed) ? 0 : 1;

static async Task<(int ExitCode, IReadOnlyCollection<string> Output)> RunAsync(Step step, string workingDirectory, bool captureOutput)
{
    var startInfo = new ProcessStartInfo("dotnet", step.Arguments)
    {
        WorkingDirectory = workingDirectory,
        RedirectStandardOutput = captureOutput,
        RedirectStandardError = captureOutput,
    };
    var output = new ConcurrentQueue<string>();
    using Process process = Process.Start(startInfo)!;
    if (captureOutput)
    {
        process.OutputDataReceived += (_, e) => Enqueue(output, e.Data);
        process.ErrorDataReceived += (_, e) => Enqueue(output, e.Data);
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
    }

    await process.WaitForExitAsync();
    return (process.ExitCode, output);
}

static void Enqueue(ConcurrentQueue<string> output, string? line)
{
    if (line is not null)
    {
        output.Enqueue(line);
    }
}

sealed record Step(string Name, string[] Arguments);

sealed record StepResult(string Name, bool Passed, TimeSpan Elapsed);