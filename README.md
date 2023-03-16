# SerilogEcsLogging
Library adding extensions methods to log ECS-structured events using serilog

## Usage

Add `SerilogEcsLogging` nuget package to your project:
```shell
dotnet add package SerilogEcsLogging
```

Program.cs:
```csharp
var builder = WebApplication.CreateBuilder(args);
// ...
builder.Host.UseSerilogEvents(logEcsEvents: true);
// ...
var app = builder.Build();
// ...
app.Run();
```

Create event class for each event type:
```csharp
public class ServiceStartupEvent : EventBase
{
    private const string ActionName = "ServiceStartup";
    
    public ServiceStartupEvent(bool success = false)
        : base(ActionName, success, null /*Captured implicitly by TimedEvent*/, success ? "Service started successfully" : "Service startup failed")
    {
    }
}
```

```csharp
public class DatabaseConnectionEvent : EventBase
{
    private const string ActionName = "DatabaseConnection";
    
    public class DatabaseConnectionDetails
    {
        public string ConnectionString { get; }
        
        public string SchemaVersion { get; }

        public DatabaseConnectionDetails(string connectionString, string schemaVersion)
        {
            ConnectionString = connectionString;
            SchemaVersion = schemaVersion;
        }
    }
    
    public DatabaseConnectionEvent() : base(ActionName, false, null /*Captured implicitly by TimedEvent*/, "Unable to establish connection to database")
    {
    }

    public DatabaseConnectionEvent(string connectionString, string schemaVersion)
        : base(ActionName, true, null /*Captured implicitly by TimedEvent*/, "Connected to database", new DatabaseConnectionDetails(connectionString, schemaVersion))
    {
    }
}
```

Trace events:
```csharp
public async Task StartAsync(CancellationToken cancellationToken)
{
    using (var startupEvent = logger.BeginTimedEvent(new ServiceStartupEvent(), autoComplete: false))
    {
        try
        {
            // Perform app startup tests
            
            // Startup success
            startupEvent.Complete(new ServiceStartupEvent(true));
        }
        catch (Exception e)
        {
            // Startup failure
            startupEvent.Fail(e);
            throw;
        }
    }
```

```csharp
using (var ev = logger.BeginTimedEvent(new DatabaseConnectionEvent(), autoComplete: false))
{
    try
    {
        // ...
        ev.Complete(new DatabaseConnectionEvent(dataAccess?.ConnectionString, dataAccess?.SchemaVersion));
    }
    catch (Exception e)
    {
        ev.Fail(e);
        throw;
    }
}
```

Traced ECS event:
```json
{
  "@timestamp": "2023-02-10T08:55:22.6289711-06:00",
  "log.level": "Information",
  "message": "Connected to database",
  "tags": ["testApp"],
  "ecs": {
    "version": "1.5.0"
  },
  "event": {
    "kind": "event",
    "action": "DatabaseConnection",
    "outcome": "success",
    "severity": 4,
    "timezone": "Central Standard Time",
    "created": "2023-02-10T08:55:22.6289711-06:00",
    "start": "2023-02-10T08:55:22-06:00"
  },
  "host": {
    "name": "test-ec2"
  },
  "log": {
    "logger": "testApp.Startup.Startup",
    "original": null,
    "origin": {
      "file": {
        "name": "Startup.cs",
        "line": 37
      },
      "function": "StartAsync"
    }
  },
  "process": {
    "thread": {
      "id": 4
    },
    "pid": 348,
    "name": "testApp",
    "executable": "testApp"
  },
  "server": {
    "user": {
      "name": "test"
    }
  },
  "service": {
    "name": "testApp",
    "version": "1.0.0.0"
  },
  "Data": {
    "connection_string": "Server=localhost;Port=3306;Database=test_db;User ID=test_user",
    "schema_version": "1.0",
    "$type": "DatabaseConnectionDetails"
  }
}
```