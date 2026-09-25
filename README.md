# AshkanAQMS

**Industrial Air Quality Monitoring & Analyzer Operations Platform — C# / .NET Framework / Windows Forms**

AshkanAQMS is a portfolio-scale industrial air-quality monitoring system designed around the workflows of environmental monitoring stations. It combines real-time analyzer acquisition, AQI and pollutant telemetry, analyzer fleet health, alarm operations, calibration evidence, data-quality workflows, diagnostics, reporting, configuration governance, and distributed-station operations in a Windows desktop application.

> **Engineering status:** the application defaults to **RealHardware** mode and does not fabricate measurements when a configured instrument is unavailable. Vendor-specific sources under `LegacyAnalyzerSources/` are retained as migration references and are excluded from compilation until adapted and validated against the corresponding physical hardware and protocol documentation.

## Why this project exists

Environmental monitoring software is more than a chart of sensor values. A station operator needs to understand whether an instrument is online, whether a sample is trustworthy, what changed in configuration, whether calibration evidence exists, which alarms need action, and whether data can be traced from field device to historian. AshkanAQMS explores those concerns as one cohesive industrial operations product.

## Highlights

- Real-hardware-first analyzer acquisition with explicit simulation opt-in
- Immediate polling exclusion for disabled analyzers, including post-request acceptance checks
- Multi-analyzer configuration and per-analyzer health/latency tracking
- AQI, pollutant telemetry, trend visualization and deterministic local analytics
- Analyzer Fleet Intelligence and Station Digital Twin views
- Alarm history plus acknowledge/shelve/close operations console
- Zero/Span calibration workflow and calibration evidence center
- Data Quality / QA, commissioning, maintenance and operations planning workspaces
- Driver Catalog, Communication Lab, Driver Test Bench and packet inspection tooling
- Serial, TCP/IP and configurable ASCII acquisition paths
- Legacy protocol migration references for multiple environmental-instrument families
- Historical exploration, reporting and executive snapshots
- Configuration backup, recovery, versioning and change-control workflows
- Operations Logbook, incident/shift handover and compliance/governance views
- Multi-Station Command Center and notification/escalation design surfaces
- RBAC / least-privilege governance blueprint
- SCADA/NOC-inspired Obsidian operations dashboard

## System architecture

```text
┌──────────────────────────────── Field / Station Layer ────────────────────────────────┐
│ Gas analyzers │ PM analyzers │ Weather │ DAQ / Analog │ Serial │ TCP/IP │ HTTP/CGI │
└───────────────────────────────────────┬───────────────────────────────────────────────┘
                                        │
                         Acquisition + Driver Boundary
                                        │
                 ┌──────────────────────┴──────────────────────┐
                 │ RealAnalyzerReader / AnalyzerAcquisition     │
                 │ Driver Catalog / Health Registry / Validation│
                 └──────────────────────┬──────────────────────┘
                                        │
                  Quality / Operations / Intelligence Services
                                        │
       ┌──────────────┬──────────────┬───┴────────┬──────────────┬───────────────┐
       │ AQI / Trends │ Alarm Engine │ Data QA    │ Calibration  │ Audit/Reports │
       └──────────────┴──────────────┴────┬───────┴──────────────┴───────────────┘
                                         │
                            Storage / Historian Boundary
                                         │
                  ┌──────────────────────┴──────────────────────┐
                  │ WinForms Operations / Engineering Workspaces│
                  │ Dashboard • Fleet • Digital Twin • Evidence │
                  └─────────────────────────────────────────────┘
```

More detail: [`docs/ARCHITECTURE.md`](docs/ARCHITECTURE.md)

## Technology stack

| Area | Technology / approach |
|---|---|
| Desktop | C#, Windows Forms, .NET Framework 4.8 |
| UI | Custom industrial controls, WinForms charting, SCADA/NOC-inspired dashboard |
| Acquisition | Serial and TCP/IP request/response; configurable text parsing |
| Security | Windows DPAPI support for protected analyzer credentials |
| Analytics | AQI, data-quality scoring, deterministic trend/anomaly services |
| Persistence | Local storage/archive with database service extension boundary |
| Engineering | Driver catalog, diagnostics, packet test bench, configuration validation |

## Analyzer / protocol engineering

The catalog models instrument families found in the supplied legacy AQMS source, including HORIBA, Ecotech/Serinus, Thermo Scientific, Teledyne, ESA, Met One BAM/E-BAM, GRIMM, Palas, FAI SWAM, Unitec, Vaisala, Davis, Delta OHM, Aeroqual, Synspec and other integration families.

| Status | Meaning |
|---|---|
| **Production** | Implemented by the current application path; still requires deployment-specific commissioning |
| **Migration reference** | Legacy source/catalog mapping retained for engineering migration; **not claimed as hardware-validated** |
| **Hardware validation required** | Physical-device/protocol verification is required before production deployment |

See [`docs/DRIVER_SUPPORT_MATRIX.md`](docs/DRIVER_SUPPORT_MATRIX.md) and [`docs/REAL_HARDWARE_SETUP.md`](docs/REAL_HARDWARE_SETUP.md).

## Major workspaces

The application includes Operations Dashboard, Live Data, Analyzer Fleet, Station Digital Twin, Analyzer Configuration, Driver Catalog, Calibration Center, Calibration Evidence, Maintenance, Quality Assurance, Commissioning, Alarm Console, Historical/Data Explorer, Reports, Remote Operations, Industrial I/O, Communication Lab, Driver Test Bench, Diagnostics, Configuration Governance, Configuration Versioning, Backup & Recovery, Operations Logbook, Incident Center, Operations Planner, Compliance Center, Access Governance, Multi-Station Command Center, Notification/Escalation Center and Integration Hub.

## Safety and data-integrity behavior

AshkanAQMS intentionally separates demonstration UX from real telemetry. `RealHardware` is the default acquisition mode. If no enabled analyzer returns a usable sample, the UI reports no live data rather than substituting a simulated value. Disabling an analyzer prevents new polling and the acquisition pipeline re-checks its enabled state before accepting an in-flight result.

This repository is an engineering/portfolio project. It does **not** claim regulatory certification, metrological certification, cybersecurity certification, or validation for every retained vendor protocol. Deployment requires verification against the actual instruments, applicable environmental standards, site procedures and infrastructure.

## Build

Requirements:

- Windows 10/11
- Visual Studio 2022 with **.NET desktop development** workload
- .NET Framework 4.8 Developer Pack

```text
1. Clone the repository.
2. Open AshkanAQMS.sln in Visual Studio.
3. Restore NuGet packages.
4. Build the solution (Debug or Release).
5. Configure analyzers before enabling real acquisition.
```

For hardware setup and parsing options, read [`docs/REAL_HARDWARE_SETUP.md`](docs/REAL_HARDWARE_SETUP.md).

## Repository map

```text
Controls/               WinForms operational and engineering workspaces
Drivers/                Current analyzer-driver catalog
Interfaces/             Infrastructure abstractions
Models/                 Domain/configuration models
Services/               Acquisition, AQI, QA, alarms, storage and reporting
LegacyAnalyzerSources/  Supplied legacy protocol sources; excluded from compilation
docs/                   Architecture, hardware setup and engineering documentation
```

## Portfolio / showcase mode

The application contains an explicitly labelled showcase path for UI demonstrations. Showcase values must not be interpreted as station measurements and are not a substitute for real-hardware validation.

## Roadmap

The strongest future engineering steps are protocol-by-protocol hardware validation, production authentication/authorization enforcement, a durable SQL historian, automated test coverage, resource-based localization, signed deployment packages, and validated OPC UA/MQTT adapters.

## Documentation

- [`Architecture`](docs/ARCHITECTURE.md)
- [`Feature Matrix`](docs/FEATURES.md)
- [`Driver Support Matrix`](docs/DRIVER_SUPPORT_MATRIX.md)
- [`Real Hardware Setup`](docs/REAL_HARDWARE_SETUP.md)
- [`Portfolio Showcase`](docs/PORTFOLIO_SHOWCASE.md)
- [`Legacy Driver Migration`](docs/LEGACY_DRIVER_MIGRATION.md)
- [`Security Policy`](SECURITY.md)
- [`Contributing`](CONTRIBUTING.md)
- [`Changelog`](CHANGELOG.md)

## Author

**Ashkan Motaei** — Software Engineer / Front-End & Industrial Software Developer

GitHub: `ashkan20171`

---

If you are reviewing this repository as an engineering portfolio, the most representative areas are the acquisition lifecycle, analyzer health registry, real-hardware safety behavior, Analyzer Driver Catalog, Operations Dashboard, Driver Test Bench, Calibration/QA workflows, and configuration governance surfaces.
