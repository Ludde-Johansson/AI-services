using ai_api;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("translator", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://api.cognitive.microsofttranslator.com/");
    httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", builder.Configuration.GetValue<string>("apiKey"));
    httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Region", "westeurope");
    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
