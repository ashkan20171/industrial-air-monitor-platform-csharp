# Ashkan AQMS — Enterprise Feature Matrix

## Monitoring
- Real-time AQI and pollutant dashboard
- PM2.5, PM10, NO2, CO2, temperature and humidity
- Station identity and last-update timestamp
- Pause/resume monitoring
- 240-point in-memory trend window

## Local Intelligence
- EMA smoothing
- Linear-regression short-horizon forecasting
- Z-score anomaly detection
- Multi-pollutant trend/risk aggregation
- Data quality scoring
- Explainable operator recommendations
- AI anomaly alarms
- Fully offline/deterministic analytics path

## Industrial Operations
- Configurable warning/critical thresholds
- Alarm latching and clear events
- Sound notification
- Audit logging
- System CPU/RAM/uptime telemetry
- Automatic measurement archive

## Reporting
- CSV measurement export
- Executive HTML snapshot
- Current KPI, AI risk, data quality and forecasts in the executive report

## UX / Presentation
- Dark industrial theme
- Clear severity colors
- Compact operator dashboard
- Executive Snapshot action for demonstrations and stakeholder reviews
- .NET Framework 4.8 / WinForms retained for Windows industrial deployment

## Roadmap-ready extension points
- Modbus RTU/TCP, OPC UA and REST transports
- SQL Server/SQLite historian
- German/English resource-based localization
- Calibration workflow
- Role-based access control
- Signed installer and update channel

## Feature Center (v2.3)
The main dashboard now includes a dedicated **Feature Center** where operators can enable or disable optional capabilities without editing configuration files.

Configurable capabilities include:
- AI Insights
- Advanced multi-pollutant analytics
- AI anomaly alarms
- Sound alerts
- Automatic data archiving
- Audit logging
- System health telemetry
- Executive HTML reports
- Presentation / portfolio demo mode

The feature center also provides **Enable All**, **Safe Demo Mode**, and **Reset Defaults** actions. Changes are persisted to the application's settings store and are reflected the next time the monitoring view is opened.

## Portfolio & Product Identity
- Dashboard Feature Center for runtime capability toggles
- Safe Demo Mode and Portfolio Mode
- Dedicated About screen identifying **Ashkan Motaei** as the developer
- Product/version/technology stack presentation for recruiter demonstrations

## v2.5 additions
- Dedicated **Alarm & Audit History** screen with search, refresh, CSV export and log-folder access.
- Dedicated **System Diagnostics** screen covering runtime, storage writability, analyzer configuration, polling configuration and feature state.
- Diagnostics snapshot export for technical handover and recruiter demonstrations.
- Dashboard navigation now exposes previously hidden operational forms.
- Analyzer configuration validation for measurement type, network address/port and serial connection.

## Remote Operations & Calibration

The dashboard now includes a dependency-safe modernization of the legacy `RemoteForm` concept. It provides analyzer selection, connection lifecycle, remote command controls, a terminal-style remote screen, operation logging and explicit safety interlocks. Hardware-specific protocols are intentionally isolated for future Modbus/serial/OPC UA transport adapters; the built-in simulation mode is suitable for portfolio demonstrations without claiming hardware connectivity.
