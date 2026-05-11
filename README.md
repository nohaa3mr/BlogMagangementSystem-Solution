Advanced Blog Management System (Vertical Slice Architecture)
This project is a sophisticated implementation of a Blog Management System, designed to demonstrate mastery over Enterprise Patterns, Asynchronous Messaging, and System Security.

Unlike traditional N-Tier architectures, this system uses Vertical Slice Architecture, keeping all code related to a specific feature together, which maximizes cohesion and simplifies scaling.

🏗️ Architectural Excellence
🍰 Vertical Slice Architecture
Instead of separating the code by technical layers (Controllers, Services, Repositories), the project is organized by Features. Each slice contains its own logic, data access, and validation, ensuring that a change in "Post Creation" doesn't break "User Authentication".

📦 Advanced Tooling & Patterns
Autofac (IoC): Used for advanced dependency injection and modularity, allowing for cleaner registration of vertical slices.

CAP Library & RabbitMQ: Implemented the Outbox Pattern to guarantee eventual consistency. When a blog post is created, the event is persisted in the database and then published to RabbitMQ.

Hangfire: Dedicated background processing for long-running tasks like email notifications or data cleanup.

Dockerization: Full multi-container setup using Docker Compose (API, SQL Server, RabbitMQ, Hangfire).

🔐 Security & "Under the Hood" Logic
I didn't just use libraries; I focused on understanding the internals of security:

🛡️ Custom Hashing & Identity
Salted Hashing: Explored the internal mechanics of password security. Implemented logic that handles Salt generation and Iteration counts to mitigate brute-force and rainbow table attacks.

Algorithm Logic: Focused on how hashing functions work at the bitwise level to ensure non-reversible data storage.

Username Sanitization: Custom algorithms to prevent injection and ensure unique identity constraints.

🧪 Quality & Monitoring
Unit Testing (xUnit & Moq): Each vertical slice is independently tested to ensure business rules are met.

Serilog: Structured logging to track the flow of a request through the slices and into the message broker.
