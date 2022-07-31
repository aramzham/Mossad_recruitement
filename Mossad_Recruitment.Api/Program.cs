using Mossad_Recruitment.Api.Infrastructure.Services;
using Mossad_Recruitment.Api.Infrastructure.Services.Interfaces;
using Mossad_Recruitment.Common.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<ICandidateService, CandidateService>(client => client.BaseAddress = new Uri(builder.Configuration["ScoutingDepartmentBaseAddress"]));
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddTransient<ICriteriaService, CriteriaService>();

var app = builder.Build();

// criterias
app.MapGet("/criterias", (ICriteriaService criteriaService) => criteriaService.Get());

app.MapPost("/criterias", (ICriteriaService criteriaService, IEnumerable<Criteria> criterias) => criteriaService.Set(criterias));

// candidates
app.MapGet("/candidate/next/{id}", (ICandidateService candidateService, Guid id) => candidateService.Next(id));

app.MapGet("/candidate/accepted", (ICandidateService candidateService) => candidateService.GetAccepted());

app.MapPost("/candidate/accept/{id}", (ICandidateService candidateService, Guid id) => candidateService.Accept(id));

app.MapPost("/candidate/reject/{id}", (ICandidateService candidateService, Guid id) => candidateService.Reject(id));

app.Run("http://localhost:3210");
