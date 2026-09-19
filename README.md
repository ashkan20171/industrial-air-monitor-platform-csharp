# Ashkan AQMS Enterprise — WinForms / .NET Framework 4.8

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
