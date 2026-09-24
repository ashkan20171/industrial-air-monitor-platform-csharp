# AshkanAQMS 3.0 — Industrial hardening review

This revision keeps the application on WinForms / .NET Framework 4.8 and focuses on fail-safe acquisition rather than visual-only changes.

## Correctness fixes
- Fixed a TCP acquisition defect that appended every received network block twice, which could corrupt parsing and logs.
- The live dashboard now checks **accepted, currently-enabled usable readings** before proceeding. A reading that returns after an operator disables an analyzer is discarded and cannot reach AQI, alarms or archive.
- Encoding lookup now falls back safely to ASCII when a configured encoding name is invalid.
- Added optional engineering-range validation per analyzer. Out-of-range measurements are marked InvalidData before downstream processing.

## Industrial safeguards
- Real hardware remains the default data source; simulation requires an explicit setting.
- Disabled analyzers are excluded before polling and re-checked after an in-flight acquisition completes.
- Communication failures never create synthetic measurement values.
- Analyzer communication health is tracked in a thread-safe registry: last attempt, last success, response time, last quality/message and consecutive failures.
- Configuration writes remain atomic with a backup copy.
- Audit, diagnostics, alarms, data-quality scoring, local analytics and executive reporting remain separated into services.

## Deployment note
This software is an engineering/portfolio AQMS foundation, not a regulatory-certified DAHS. Before production use, add vendor-specific protocol drivers (for example Modbus register maps), calibration/zero-span workflows, traceable time synchronization, database redundancy, user authentication/roles, signed audit records, UPS/watchdog integration and validation against the jurisdiction's applicable monitoring standard.

## Build verification
The source targets .NET Framework 4.8. This Linux packaging environment does not contain the Windows .NET Framework/MSBuild toolchain, so the final compile should be run in Visual Studio on Windows with the .NET Framework 4.8 targeting pack installed.
