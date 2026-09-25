# Contributing

Thanks for your interest in AshkanAQMS.

## Engineering principles

1. Do not fabricate live telemetry or silently fall back to simulation.
2. Treat zero as a potentially valid measurement; use explicit quality/state information for invalid samples.
3. Keep vendor-specific protocol logic isolated from UI code.
4. Never add hard-coded production credentials.
5. Preserve cancellation, timeout and disabled-analyzer lifecycle behavior.
6. Add protocol fixtures/tests before changing framing, CRC/checksum or alarm-bit semantics.
7. Mark unverified vendor implementations as migration/reference until hardware validation is complete.
8. Log actionable failures; avoid empty `catch` blocks.

## Pull requests

Keep changes focused, describe the operational reason for the change, document any protocol assumptions, and include reproducible test steps. Hardware-facing changes should state the instrument/model/firmware and whether testing used a simulator, captured frame, or physical device.
