# CLAUDE.md

TheProject: a learning-driven, production-style .NET 10 system (Products, Customers, Orders,
Shipping) built as vertical-slice services. Portfolio code: no scratch files, no dead code.

## Commands
- Build: `dotnet build TheProject.slnx`
- Test: `dotnet test --solution TheProject.slnx` (Microsoft.Testing.Platform mode; `--project <path>` for one project)
- Full CI gate: `dotnet run build/ci.cs` (`-- --quiet` prints only the failing step), or the
    `ci-local` skill. GitHub Actions runs the same file, so the gate's steps change only in `build/ci.cs`.

## Conventions Claude would otherwise get wrong
- **Packages:** Central Package Management. Never put `Version=` on a `PackageReference`;
  add or bump a `PackageVersion` in `Directory.Packages.props`.
- **Build rules live in `Directory.Build.props`** (root, and `tests/` which imports root).
  Don't repeat `TargetFramework`, `Nullable`, `ImplicitUsings` in `.csproj` files.
- **Warnings are errors.** Fix the code. If a rule is genuinely wrong for a spot, lower that
  rule's severity in `.editorconfig` for the narrowest path, with a comment. Never `NoWarn`.
- **Errors:** handlers return `Result`/`Result<T>` (TheProject.BuildingBlocks.Results) for
  expected failures only. Handlers never `catch (Exception)`; unexpected failures propagate.
- **HTTP status for failures comes only from `ErrorType` via `error.ToProblem()`.**
- **Slices:** endpoints implement `IEndpoint`, handlers implement `IHandler<TRequest>` or
  `IHandler<TRequest, TResponse>`. Registration is automatic
  (`AddSlices(typeof(Program).Assembly)` + `app.MapEndpoints()`); never register them by hand.
- **Slice type names:** `<UseCase>Request` / `<UseCase>Response`, one pair per slice
  (`GetProductRequest`, `GetProductResponse`). Never share a response type across slices.
- Extension members use C# 14 `extension(...)` blocks, not `this`-parameter methods. One
  static class per extended type, named `<Purpose>Extensions` (two receivers in one class
  trip CA1708).
- Tests: xUnit v3 + AwesomeAssertions (not FluentAssertions, licence).

## Workflow
- One branch + PR per change; CI must be green; Conventional Commit messages.
- The user writes the code in this repo as part of a course. When asked to review or explain,
  do that. Don't rewrite their code unless asked.