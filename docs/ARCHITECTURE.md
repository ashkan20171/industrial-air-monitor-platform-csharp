# Architecture

The project intentionally remains WinForms/.NET Framework 4.8 while separating UI concerns from services and domain models.

```text
Presentation (WinForms)
   |
   +-- CurrentDataControl / Forms / Theme
   |
Application Services
   +-- AQI Calculator
   +-- Alarm Engine
   +-- AI / Advanced Analytics
   +-- Data Quality
   +-- Health Monitor
   +-- Reporting
   |
Infrastructure
   +-- CSV / local archive
   +-- DbService extension point
   +-- sensor transport extension point
   |
Domain Models
   +-- AirQualityData
   +-- SensorLog
   +-- AlarmEvent
   +-- AppSettings
```

The analytics layer is intentionally local and deterministic so the demo remains usable offline. It must not be described as a regulatory-certified AI model; real deployment requires validation against the target sensor hardware, averaging periods and applicable environmental standards.

### Feature Center
`FeatureCenterControl` provides a dashboard-level capability registry over `AppSettings`. This makes optional platform services explicitly controllable by an operator and keeps feature flags out of hard-coded UI logic. The settings are serialized by `StorageService` and can be extended with additional feature flags as the product grows.

## Operational UX layer (v2.5)
The dashboard now exposes Alarm & Audit History and System Diagnostics as first-class views. This makes operational traceability and deployment-readiness visible without requiring users to inspect files manually. These views are presentation and operational tooling; they do not imply regulatory certification.
