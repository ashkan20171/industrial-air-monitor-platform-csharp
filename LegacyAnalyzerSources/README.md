# Legacy analyzer source archive

This folder contains the original SadraAQMS analyzer/driver source files supplied for migration into AshkanAQMS.

They are deliberately **included in the final project but excluded from compilation** because they reference the old SadraAQMS runtime, global state, device base classes and vendor libraries. Keeping them here preserves the real protocol logic and makes migration auditable without pretending that every protocol has already been validated against physical hardware.

The production AshkanAQMS layer uses the catalog and hardened acquisition boundary. Each vendor driver should move from `Migration reference` to a validated adapter only after protocol review and hardware testing.

Important migration rules:
- Never poll a disabled analyzer.
- Re-check enabled state before committing an in-flight reading.
- Do not hard-code credentials.
- Replace empty catches with structured logging.
- Replace sentinel measurements such as `-10000` with explicit data-quality states.
- Separate transport, framing/parser, acquisition scheduling, calibration, remote control and persistence.
