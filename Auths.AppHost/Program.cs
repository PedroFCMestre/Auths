var builder = DistributedApplication.CreateBuilder(args);

//var redis = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.Auths_ApiService>("apiservice");
//.WithReference(redis);

builder.AddProject<Projects.Auths_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    //.WithReference(redis)
    .WaitFor(apiService);


builder.Build().Run();
