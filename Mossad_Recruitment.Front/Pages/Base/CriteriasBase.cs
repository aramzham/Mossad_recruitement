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

        protected IEnumerable<Criteria> CriteriasToChoose { get; set; }
        protected RadzenDataGrid<Criteria> CriteriaDataGrid { get; set; }
        protected List<Criteria> SelectedCriterias { get; set; }
        protected bool IsSaveButtonBusy { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var criteriasAlreadySet = await CriteriaService.Get();
            var technologies = await TechnologyService.GetAll(); // may be retrieved from local storage

            var criterias = new List<Criteria>();
            foreach (var technology in technologies)
            {
                var criteria = criteriasAlreadySet.FirstOrDefault(x => x.TechnologyId == technology.Key);
                criterias.Add(criteria is not null
                    ? criteria
                    : new Criteria
                    {
                        TechnologyId = technology.Key,
                        TechnologyName = technology.Value.Name
                    });
            }

            CriteriasToChoose = criterias;
        }

        protected void OnChange(string numberOfYears, Criteria data)
        {
            if (int.TryParse(numberOfYears, out var noy) && noy != 0)
            {
                data.YearsOfExperience = noy;
                SelectedCriterias.Add(data);
            }
        }

        protected Task OnSaveButtonClick()
        {
            return CriteriaService.Set(CriteriasToChoose.Concat(SelectedCriterias));
        }
    }
}
