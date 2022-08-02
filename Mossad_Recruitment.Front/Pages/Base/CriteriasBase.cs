using Microsoft.AspNetCore.Components;
using Mossad_Recruitment.Common.Models;
using Mossad_Recruitment.Front.Services.Contracts;
using Radzen.Blazor;

namespace Mossad_Recruitment.Front.Pages.Base
{
    public class CriteriasBase : ComponentBase
    {
        [Inject] public ICriteriaService CriteriaService { get; set; }
        [Inject] public ITechnologyService TechnologyService { get; set; }

        protected IEnumerable<Criteria> Criterias { get; set; }
        protected IDictionary<Guid, Technology> Technologies { get; set; }
        protected RadzenDataGrid<Criteria> CriteriaDataGrid { get; set; }
        protected List<Criteria> SelectedCriterias { get; set; }
        protected bool IsSaveButtonBusy { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Criterias = await CriteriaService.Get();
            Technologies = await TechnologyService.GetAll(); // may be retrieved from local storage
        }

        protected void OnChange(string numberOfYears, Criteria data) 
        {
            if (int.TryParse(numberOfYears, out var noy)) 
            {
                data.YearsOfExperience = noy;
                SelectedCriterias.Add(data);
            }
        }

        protected Task OnSaveButtonClick() 
        {
            return CriteriaService.Set(Criterias.Concat(SelectedCriterias));
        }
    }
}
