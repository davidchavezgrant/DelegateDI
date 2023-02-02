using DelegateDI;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDemoServices();
builder.Services.AddBylinesServices();

var app = builder.Build();

app.UseSwagger();
app.MapDemoEndpoints();
app.MapBylinesEndpoints();
app.UseSwaggerUI();
app.Run();