---
  name: ci-local
  description: Run TheProject's CI gate locally (restore, format check, Release build, tests) and report the first failing step. Use before saying a change is done, before committing, or when the user asks whether the build is green.
  allowed-tools: Bash(dotnet run build/ci.cs *)
  ---

  Run `dotnet run build/ci.cs -- --quiet` from the repository root. `build/ci.cs` is the gate:
  GitHub Actions runs the same file, so never run its steps one by one. In quiet mode it
  prints only the output of the step that fails, then a summary table; the exit code is 0
  only when every step passed.

  ## Reporting
  - All green: the summary table.
  - A failure: the step, the first relevant errors as `file:line` with the diagnostic id,
    and the most likely fix.

  ## Never
  - Never make the gate pass by adding `NoWarn`, lowering an analyzer severity, deleting or
    skipping a test, or editing `build/ci.cs` or the workflow. Those are the user's decisions:
    report and stop.
  - Don't change code at all unless the user asked for fixes.