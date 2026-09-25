# Changelog

All notable portfolio releases of AshkanAQMS are documented here.

## [17.0.0] - 2026-09

### Added
- Multi-Station Command Center for distributed-station operational views.
- Notification & Escalation Center design surface.
- Configuration Versioning & Change Control workspace.
- Calibration Evidence / certificate workflow.
- Alarm Command Console with acknowledge/shelve/close workflow.
- Driver Test Bench and offline packet inspection workspace.
- Access Governance / RBAC blueprint.
- Backup & Recovery Center.
- Operations Logbook and compliance/data-governance workspaces.
- Station Digital Twin and expanded Analyzer Fleet Intelligence.
- Obsidian SCADA/NOC-inspired operations dashboard.

### Engineering safeguards
- RealHardware remains the safe default.
- Disabled analyzers are excluded from polling and checked again before in-flight results are accepted.
- Legacy vendor sources remain excluded from compilation until migrated and validated.
- Demo/showcase telemetry remains explicitly separated from real measurements.

## Earlier portfolio evolution

Earlier iterations introduced analyzer configuration hardening, engineering-range validation, analyzer health tracking, calibration/maintenance/QA workspaces, driver cataloguing, diagnostics, reporting, governance and the modern operations shell. The Git history is the preferred source for fine-grained changes.
