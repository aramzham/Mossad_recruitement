using Microsoft.AspNetCore.Components;
using Mossad_Recruitment.Common.Dtos;
using Mossad_Recruitment.Front.Services.Contracts;

namespace Mossad_Recruitment.Front.Pages.Base
{
    public class CandidatesBase : ComponentBase
    {
        [Inject] public ICandidatesService CandidatesService { get; set; }

        protected CandidateDto Candidate { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        protected override async Task OnInitializedAsync()
        {
            Candidate = await CandidatesService.Next();
        }

        protected async Task Accept_OnClick(Guid id)
        {
            await CandidatesService.Accept(id);
            Candidate = await CandidatesService.Next();
        }

        protected async Task Reject_OnClick(Guid id)
        {
            await CandidatesService.Reject(id);
            Candidate = await CandidatesService.Next();
        }
    }
}
