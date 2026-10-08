---
name: ci-local
description: Run TheProject's CI gate locally (restore, format check, Release build, tests) and report the first failing step. Use before saying a change is done, before committing, or when the user asks whether the build is green.
allowed-tools: Bash(dotnet restore *) Bash(dotnet format *) Bash(dotnet build *) Bash(dotnet test *)
---

Run the gate defined in `.github/workflows/ci.yml` from the repository root, in the same
order, stopping at the first failing step. Read the workflow first: it is the source of
truth. At the time of writing its steps are restore, `dotnet format --verify-no-changes`,
Release build, and `dotnet test --solution TheProject.slnx --no-build`. If the workflow
differs from this description, follow the workflow and tell the user this skill needs
updating.

## Reporting
- All green: one line per step.
- A failure: the step, the first relevant errors as `file:line` with the diagnostic id,
  and the most likely fix.

## Never
- Never make the gate pass by adding `NoWarn`, lowering an analyzer severity, deleting or
  skipping a test, or editing the workflow. Those are the user's decisions: report and stop.
- Don't change code at all unless the user asked for fixes.