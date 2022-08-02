using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Mossad_Recruitment.Front;
using Mossad_Recruitment.Front.Services;
using Mossad_Recruitment.Front.Services.Contracts;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["ApiBaseAddress"]) });
builder.Services.AddTransient<ICandidatesService, CandidatesService>();
builder.Services.AddTransient<ICriteriaService, CriteriaService>();
builder.Services.AddTransient<ITechnologyService, TechnologyService>();

await builder.Build().RunAsync();
