var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Auths_ApiService>("apiservice");

builder.AddProject<Projects.Auths_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
