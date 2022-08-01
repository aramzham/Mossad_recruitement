using Microsoft.AspNetCore.Components;
using Mossad_Recruitment.Common.Models;
using Mossad_Recruitment.Front.Services.Contracts;

namespace Mossad_Recruitment.Front.Pages.Base
{
    public class AcceptedBase : ComponentBase
    {
        [Inject] public ICandidatesService CandidatesService { get; set; }

        protected IEnumerable<Candidate> Candidates { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Candidates = await CandidatesService.GetAcceptedList();
        }
    }
}
