Overview

This project addresses two core problems:

Delivery cost calculation, including discount logic implemented using the Strategy pattern.

Delivery time estimation, handled through efficient vehicle scheduling using a Priority Queue.

Design & Approach

Began by clarifying requirements and identifying constraints.

Modeled the core domain entities such as Package and Vehicle.

Used Clean Architecture.

Followed SOLID principles to keep the design modular and maintainable.

Applied the Strategy Pattern to handle different discount rules cleanly.

Used a Priority Queue to manage vehicle availability and scheduling efficiently.

Development Process

Started with pseudo-code and basic flow diagrams to validate the approach.

Implemented the core business logic incrementally.

Refactored the solution into a cleaner, layered architecture.

Added unit tests to validate:

Discount application logic

Maximum weight constraints

Delivery time calculations

Multi-vehicle scheduling scenarios

Error Handling

Invalid inputs are handled gracefully with clear validation.

Vehicle weight limits are strictly enforced.

Explicit exceptions are used for unexpected or invalid states.

Production Readiness

Designed to be easily extensible.

Clear separation of concerns across components.

Scales well to support additional offers, vehicles, or scheduling rules in the future.
