using Microsoft.AspNetCore.Mvc;
using Mossad_Recruitment.Api.Infrastructure.Services;
using Mossad_Recruitment.Api.Infrastructure.Services.Interfaces;
using Mossad_Recruitment.Common.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddHttpClient<ICandidateService, CandidateService>(client => client.BaseAddress = new Uri(builder.Configuration["ScoutingDepartmentBaseAddress"]));
builder.Services.AddTransient<ICriteriaService, CriteriaService>();
builder.Services.AddHttpClient<ITechnologyService, TechnologyService>(client => client.BaseAddress = new Uri(builder.Configuration["ScoutingDepartmentBaseAddress"]));
builder.Services.AddSingleton<ICacheService, CacheService>();

var app = builder.Build();

app.UseCors(builder => builder
     .AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader());

// technologies
app.MapGet("/technologies", (ITechnologyService technologyService) => technologyService.GetAll());

// criterias
app.MapGet("/criterias", (ICriteriaService criteriaService) => criteriaService.Get());

app.MapPost("/criterias", (ICriteriaService criteriaService, [FromBody] IEnumerable<Criteria> criterias) => criteriaService.Set(criterias));

// candidates
app.MapGet("/candidate/next", (ICandidateService candidateService) => candidateService.Next());

app.MapGet("/candidate/accepted", (ICandidateService candidateService) => candidateService.GetAccepted());

app.MapPost("/candidate/accept/{id:guid}", (ICandidateService candidateService, Guid id) => candidateService.Accept(id));

app.MapPost("/candidate/reject/{id:guid}", (ICandidateService candidateService, Guid id) => candidateService.Reject(id));

app.Run("http://localhost:3210");
