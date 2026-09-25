# Analyzer Driver Support Matrix

This matrix is generated from the current `AnalyzerDriverCatalog`. **Migration reference does not mean hardware validated.** Physical-device testing and protocol-document verification are required before production use.

| Family / model | Transport | Current status | Measurements / role |
|---|---|---|---|
| Generic ASCII / Numeric | Serial / TCP | Production path | Configurable gases, PM and weather |
| Aeroqual | HTTP | Migration reference | PM |
| AIO2 9800 | Serial | Migration reference | Gas / PM |
| AMA Binary | Serial | Migration reference | Gas |
| API / Enviro | Serial | Migration reference | Gas / PM |
| Met One BAM / E-BAM | Serial | Migration reference | PM2.5 / PM10 |
| Magee / Met One Aethalometer | Serial | Migration reference | Black carbon |
| Ecotech / Serinus | Serial / USB | Migration reference | Gas |
| Serinus S50 | Serial / TCP | Migration reference | SO2 |
| ESA Analyzer | Serial / UDP | Migration reference | Gas / PM, calibration/remote concepts |
| ESA Modbus | Modbus RTU | Migration reference | Gas / PM |
| Synspec GC995 / GC Alpha | Serial | Migration reference | Gas |
| GRIMM Dust | Serial | Migration reference | PM |
| HORIBA 3XX / 370 | Serial | Migration reference | Gas |
| HORIBA LAN | HTTP / CGI | Migration reference | Gas / PM / history recovery concepts |
| HORIBA Logger | Serial | Migration reference | Mapper-based channels |
| Delta OHM HD2110L | Serial | Migration reference | LEQ |
| TM1240 | Modbus RTU | Migration reference | Gas |
| Palas Particle Analyzer | Serial | Migration reference | PM |
| FAI SWAM | Serial | Migration reference | PM |
| Teledyne Gas Analyzer | Serial | Migration reference | Gas |
| Thermo Scientific Gas Analyzer | Serial / TCP | Migration reference | Gas |
| TP Analog | Serial | Migration reference | Analog channels |
| Unitec Analyzer | TCP | Migration reference | Gas / PM / weather mappings |
| Davis Vantage Weather | Serial | Migration reference | Weather |
| Vaisala WXT510 | Serial | Migration reference | Weather |
| Delta OHM HD52 Weather | Serial / Modbus-like | Migration reference | Weather |
| Delta OHM Weather | Serial | Migration reference | Weather |
| ESA / Advantech Analog | DAQ Analog | Migration reference | Analog input |
| Enviro Relative Communication | Serial | Migration reference | Gas / PM |

## Validation policy

A driver should only be promoted from migration/reference status after its transport settings, framing, checksums/CRC, register or field mapping, engineering units, alarm semantics, timeout/retry behavior, calibration/remote commands and failure modes have been checked against authoritative protocol documentation and the target physical instrument.
