# Legacy analyzer protocol migration register

The legacy SadraAQMS source supplied for the migration was used as a behavioral reference. The new application deliberately separates polling lifecycle, transport, parsing, health and UI concerns.

## Safety rules

1. Disabled devices are checked before every request.
2. Responses are revalidated against the current enabled set before acceptance.
3. No failed real-hardware read silently becomes simulated data.
4. Credentials must not be hard-coded in driver source.
5. Empty catch blocks and sentinel-only error signaling are not accepted in new production code.
6. A migrated vendor protocol remains `Migration reference` until tested with the physical analyzer/model and its manual.

## Known legacy issues intentionally not copied

- hard-coded passwords/credentials;
- empty catch blocks;
- fixed pollutant indexes where mapping can be configured;
- `-10000` as the only error representation;
- unmanaged background-thread lifetime patterns;
- tester-only protocol bytes/comments without hardware validation;
- fixed historical date filters and other UI-era defects.
