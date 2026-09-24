# AshkanAQMS Enterprise v10.4 — Dashboard Visual Polish

This revision focuses on the station dashboard shown at startup.

- Menu-only navigation: no duplicate dashboard navigation buttons or toolbar.
- KPI cards use a stable three-row layout so long values such as `RealHardware` are not clipped.
- Dashboard status text now correctly refers only to the permanent menu.
- Larger operational panels for AQI, trend and summary.
- Analyzer fleet state is visually emphasized while preserving the existing industrial layout.
- Existing analyzer studio, acquisition, calibration, alarms, reports, diagnostics and legacy analyzer source references are preserved.

Real analyzer protocols still require validation against the actual analyzer model/firmware before production deployment.

# AshkanAQMS Enterprise v10.3.1 — Menu-Only Navigation

## Dashboard navigation correction
- Removed the entire quick-action ToolStrip from the main shell.
- Current Data, Historical, Export/Trends, Analyzer Parameters, Calibration, Remote, DAQ/Weather, Alarms, Diagnostics, Readiness and Dashboard are no longer repeated as toolbar buttons.
- Navigation remains available from the permanent top MenuStrip.
- The Dashboard is reserved for operational KPIs, AQI, 24-hour trend, alarm/weather/station summary and analyzer fleet status.

# AshkanAQMS Enterprise v10.2 — Dashboard Navigation Edition

This edition starts on a dedicated station Dashboard, inspired by the workflow of the legacy SadraAQMS application while retaining the modern enterprise UI. Every top-menu item, toolbar command and dashboard quick action opens its related detailed workspace inside the main application content area.

## v10 navigation changes
- Dashboard is the default startup workspace.
- Dashboard shows configured/enabled/disabled analyzers and acquisition mode.
- Quick actions open Live Data, Analyzer Parameters, Alarms and Historical/Reports.
- Top menus and the large legacy-inspired toolbar remain available at all times.
- Detailed workspaces replace the central content area without opening a confusing collection of unrelated main windows.
- Existing Analyzer Studio, driver catalog, calibration, fleet health, maintenance, DAQ/weather/UPS, remote operations, diagnostics, QA, commissioning, governance, digital twin, incidents, planner, integrations and analytics remain available.

# Ashkan AQMS Enterprise v8.0 — Legacy-Inspired Modern Operations UI

## v8 visual direction
The main shell deliberately preserves the proven SadraAQMS operator workflow — **File / Analyzers / View / Tools / Operations / Help**, a large quick-access toolbar, persistent station identity, and a bottom equipment-status strip — while modernizing typography, spacing, colors, hierarchy, responsive docking and enterprise navigation. The goal is familiarity for AQMS operators without carrying forward the dated fixed-size UI.

### Quick-access workflow
Current Data, Historical/Reports, Export/Trends, Analyzer Parameters, Calibration, Remote, DAQ/Weather, Alarms, Diagnostics and Command Center are one click away from the top toolbar. The richer v7 Analyzer Studio remains available under **Analyzers → Analyzer Parameters**, including driver-family and measurement/gas selection.

# Ashkan AQMS Enterprise v7.0

Version 6 expands the portfolio/enterprise surface beyond the supplied legacy system while retaining all 35 supplied analyzer/reference files under `LegacyAnalyzerSources/`.

## v6 additions
- Station Digital Twin with synchronized analyzer/edge/storage/alarm topology.
- Operations Planner & Work Orders with generated preventive-maintenance and calibration-readiness queues.
- Incident & Shift Handover Center with ownership, acknowledgement and resolution workflow.
- Data Explorer & Analytics Workbench for quality-aware investigation and cross-pollutant analysis design.
- Integration Hub covering OPC UA, MQTT, REST, SQL Historian, SignalR, Modbus, Serial, TCP/UDP, HTTP/CGI and DAQ integration boundaries.
- Expanded Command Center capability cards and navigation.
- Existing Fleet Health, Maintenance, Calibration, Remote Operations, Communication Lab, Reports, Alarm/Audit, DAQ/Weather/UPS, Diagnostics and Analyzer Management retained.

> Engineering note: screens that describe an adapter as architecture-ready are integration surfaces, not claims that a production OPC UA/MQTT/REST server is already field-certified. Legacy vendor protocols remain migration references until adapted and hardware-validated.


A portfolio-grade industrial air-quality operations platform for WinForms / .NET Framework 4.8. v5 focuses on a premium command-center experience, a broader operational surface, and an auditable migration path for the real analyzer protocols supplied from SadraAQMS.

## v5 highlights
- Redesigned enterprise command-center shell and visual system.
- New Fleet Health and Maintenance & Reliability surfaces.
- Expanded driver catalog and transport coverage.
- Original supplied analyzer source files retained under `LegacyAnalyzerSources/` (excluded from compilation until adapted and hardware-tested).
- RealHardware remains the safe default; simulation requires explicit opt-in.
- Analyzer disable safety is enforced before polling and before accepting in-flight readings.
- Portfolio showcase documentation under `docs/PORTFOLIO_SHOWCASE.md`.


A portfolio-grade industrial Air Quality Monitoring System designed to remain compatible with Windows desktop/industrial environments while providing a modern operator experience.

## Highlights
- Modern dark industrial WinForms dashboard
- Real-time monitoring simulator
- AQI and dominant pollutant calculation
- PM2.5 trend visualization
- Local-first AI analytics: EMA, linear forecast, anomaly detection and confidence
- Measurement Data Quality Score
- Latched Warning/Critical alarm engine with clear events
- Optional AI anomaly alarms
- Operator sound alerts
- CSV historical archive and reporting
- Analyzer configuration
- Application audit log
- Basic CPU/RAM health telemetry
- DPI-aware foundation and defensive exception handling

## Build
Open `AshkanAQMS.sln` in Visual Studio on Windows with .NET Framework 4.8 developer/targeting tools installed.

## Important
The bundled simulator and local analytics are demonstration/engineering features. They are not a regulatory certification. For production AQMS use, validate sensors, calibration, averaging periods, AQI methodology, traceability and applicable local/EU requirements.

See `docs/ARCHITECTURE.md` and `docs/FEATURES.md` for the engineering design and extension points.


### v2.3 highlights
- Dashboard Feature Center for live capability toggles
- Enable All / Safe Demo Mode / Reset Defaults
- Presentation Mode for portfolio demonstrations
- Feature flags persisted through the existing settings store

### v2.5 — Operations & Technical Review
- Alarm & Audit History is now a first-class dashboard view.
- System Diagnostics provides a quick deployment-readiness check and exportable diagnostic snapshot.
- Analyzer configuration includes input validation for network and serial settings.
- Existing WinForms forms remain part of the application and are being progressively upgraded rather than replaced.

## Real Hardware Acquisition (v2.8.4)

The application now uses a real-hardware acquisition pipeline by default. See `docs/REAL_HARDWARE_SETUP.md` for analyzer configuration, serial/TCP parsing, disable/enable behavior, and protocol-driver guidance.


### v3.0 — Industrial hardening
- Fixed duplicate TCP receive-buffer processing.
- Added engineering-range rejection before AQI/alarm/archive processing.
- Added per-analyzer communication health registry.
- Hardened disable-during-read behavior so late samples are discarded.
- Added safe encoding fallback and technical hardening review.

See `docs/REVIEW_AND_HARDENING.md`.

## Ashkan AQMS Industrial 4.0

Version 4.0 introduces a top-navigation NOC/SCADA-inspired shell, station operations overview, analyzer driver catalog, calibration center, industrial I/O/weather/UPS workspace and communication diagnostics workspace.

### Acquisition safety boundary

* Analyzer enabled state is checked when a cycle is created **and again immediately before each physical request**.
* The live enabled set is re-read after acquisition; a response from an analyzer disabled while a request was in flight is discarded before dashboard, AQI, alarms or archive processing.
* Simulation is never used as an implicit fallback for failed hardware. It only runs when `DataSourceMode=Simulation` and simulation is explicitly allowed.
* Vendor entries marked **Migration reference** were reconstructed from the supplied legacy SadraAQMS source. They are compatibility references, not claims of laboratory or field validation.

### Migrated protocol families catalogued

Aeroqual, AIO2 9800, AMA, API/Enviro, BAM/E-BAM, AE/AE33, Ecotech/Serinus, ESA/ESA Modbus, GC995/GC Alpha, GRIMM, HORIBA serial/LAN, Delta OHM LEQ/Weather, TM1240, Palas, Serinus S50, SWAM, Teledyne, Thermo Scientific, TP Analog, Unitec, Davis Vantage, Vaisala WXT510 and DAQ/analog sources.


## v7.0 Analyzer Parameter Center
- Analyzer/driver picker built from the migrated hardware catalog.
- Gas/measurement list changes dynamically for the selected analyzer family.
- Multi-measurement selection is persisted per analyzer while retaining a primary measurement for the existing acquisition pipeline.
- Manufacturer, model, transport and protocol metadata are auto-populated from the selected driver profile.
- Includes gas, particulate, black-carbon, LEQ, analog and weather measurement profiles derived from the supplied legacy sources.
- Legacy analyzer source files remain in `LegacyAnalyzerSources` for auditable migration; unvalidated hardware protocols are not falsely marked production-ready.

## v9.0 Operations & QA expansion
- Added Data Quality & QA Center for analyzer/measurement/range governance.
- Added Commissioning & Readiness workflow with configuration-derived checks and explicit operator sign-off items.
- Added Configuration Governance & Recovery center with analyzer configuration backup inventory and controlled recovery messaging.
- Expanded the legacy-inspired Operations menu and quick toolbar without removing v8 functionality.
- Preserved Analyzer Studio, multi-measurement/gas mapping, real-hardware acquisition guardrails, all retained legacy analyzer sources, driver catalog, calibration, maintenance, digital twin, incidents, data explorer and integration hub.


## v10.2 Operational Command Dashboard
- Removed duplicate Current Data / Analyzer Parameters / Alarms / Historical quick-action buttons from the Dashboard.
- Navigation remains available through the persistent top menu and toolbar.
- Dashboard now focuses on station KPIs, operational status, and analyzer fleet summary.


## v10.2 Operational Command Dashboard
- Dashboard is now information-only: no duplicate navigation buttons.
- Adds current AQI from the latest stored live measurement, 24-hour PM2.5/PM10 trend, threshold-condition summary, weather snapshot, station uptime, analyzer counts and fleet snapshot.
- Values are sourced from stored acquisition data; unavailable data is shown as unavailable rather than fabricated.
