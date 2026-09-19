# Real Hardware Analyzer Setup

AshkanAQMS now defaults to **RealHardware** acquisition. The application never creates demo COM/IP analyzers automatically and never substitutes simulated values when a real analyzer is unavailable.

## Analyzer lifecycle

- **Enabled**: the acquisition engine polls the analyzer.
- **Disabled**: no new request is sent, no new sample is archived, and the analyzer is excluded from AQI/alarm evaluation.
- **Re-enabled**: polling resumes on the next acquisition cycle.
- Missing ports/endpoints are reported as Offline/Communication Error instead of crashing the application.

## Serial analyzer

Configure:

- COM port (select from the ports detected by Windows)
- Baud rate
- Data bits / parity / stop bits
- Request interval and timeout
- Optional request command. Escape sequences `\\r`, `\\n`, and `\\t` are supported.
- Response delimiter, for example `\\r\\n`
- Response field index for delimited responses
- Response key for responses such as `PM25=12.4`
- Optional regular expression. A named group `value` is recommended, for example `PM25\\s*[:=]\\s*(?<value>[-+]?\\d+(?:\\.\\d+)?)`
- Gain / offset

## TCP analyzer

Configure IP address and TCP port plus the same request/response parsing options. The generic reader is intended for ASCII/text request-response protocols.

## Important protocol limitation

A generic ASCII reader cannot safely decode proprietary binary, Modbus register maps, vendor checksum frames, or instrument-specific protocols without their protocol specification. For those devices, add a dedicated driver using the same `AnalyzerReading` contract rather than guessing a value.

## Connection test

The Analyzer Configuration dialog now performs a real acquisition test, not just a port-open test. A successful test displays the parsed live value and response time.

## No live data behavior

If no enabled analyzer returns a valid sample, the dashboard shows `NO LIVE DATA` and displays `--` instead of stale or fabricated measurements.

## Simulation

Simulation remains available only as an explicit test mode when `AppSettings.AllowSimulationMode` is enabled and `DataSourceMode` is set to `Simulation`. Production/default mode is `RealHardware`.
