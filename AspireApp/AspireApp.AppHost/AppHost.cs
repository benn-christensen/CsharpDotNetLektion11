var builder = DistributedApplication.CreateBuilder(args);

// PostgreSQL server container med pgAdmin og persistent datalagring
var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithPgAdmin();

// Database inden i PostgreSQL serveren
var db = postgres.AddDatabase("appdb");

// TODO: Tilføj jeres Web API projekt reference:
// var apiService = builder.AddProject<Projects.Your_Web_Api_Project>("apiservice")
//     .WithReference(db);

// TODO: Tilføj jeres Blazor WASM projekt reference:
// builder.AddProject<Projects.Your_Blazor_Wasm_Project>("webfrontend")
//     .WithReference(apiService);

builder.Build().Run();
