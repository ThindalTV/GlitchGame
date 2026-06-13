var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.GlitchGame_ApiService>("api");

var frontEnd = builder.AddProject<Projects.GlitchGame_Web>("frontend")
    .WaitFor(api);

builder.Build().Run();
