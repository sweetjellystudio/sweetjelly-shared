#   Shared package instructions

This repository owns common outcome and policy value shapes. Package-specific error catalogs and policy meanings belong to consumers.

*   Before changing public types, read [architecture][architecture] and the relevant [usage guide][usage]. Before compatibility or support changes, read the [compatibility decision][compatibility].
*   Preserve factory validation, property behavior, default struct behavior, and deprecated API compatibility unless a breaking change is explicitly agreed. Changes to payload access and exceptions are public contract changes.
*   Keep runtime assemblies independent of Unity and other packages. Validate both Result and Policy consumers; do not add consumer-specific meanings or UI types here.
*   Correctness tests belong in `Tests/Editor`. Run `SWeetJelly.Shared.Tests.EditMode` on Unity 6.3 LTS and record the actual Editor patch, target, executed/passed/failed/skipped counts and report path. Development-host success and isolated-consumer success are separate evidence.
*   Before release, validate manifest name/version/Unity floor and exact dependency declarations against runtime asmdefs and source. Treat root LICENSE.md as authoritative. Inspect the actual UPM tarball for required files, Unity metadata and unwanted development files, then install that tarball with only declared dependencies and test tools in a separate consumer. Record package resolution, compilation and tests separately from local-source checks; report unsigned packaging and untested platforms explicitly.
*   Maintain architecture and usage documentation with contract changes. Record significant agreed trade-offs in ADRs. Use the host's installed `unity-package` coordinator for the shared workflow and local work record; this package must remain understandable without the host's ignored notes.

[architecture]: ARCHITECTURE.md
[usage]: README.md
[compatibility]: docs/adr/0001-support-and-api-compatibility.md
