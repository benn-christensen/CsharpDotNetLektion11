using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// PostgreSQL server container med pgAdmin og persistent datalagring
var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithPgAdmin();

// Database inden i PostgreSQL serveren
var db = postgres.AddDatabase("subscriptiondb");

var migrations = builder.AddProject<MigrationWorker>("migrations")
    .WithReference(db)
    .WaitFor(db);
var apiService = builder.AddProject<EaaaTrackApi>("trackApi")
    .WithReference(db)
    .WaitFor(db)
    .WithReference(migrations)
    .WaitForCompletion(migrations);

// TODO: Tilføj jeres Blazor WASM projekt reference:
// builder.AddProject<Projects.Your_Blazor_Wasm_Project>("webfrontend")
//     .WithReference(apiService);

builder.Build().Run();
