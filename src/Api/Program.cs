using Application.UseCases.Login;
using Infrastructure.MongoDb.Extensions;
using Infrastructure.Redis.Extensions;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

builder.Services
    .AddCache(builder.Configuration)
    .AddDatabase(builder.Configuration);


builder.Services.AddMediatR(typeof(LoginUseCase));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.Seed();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();