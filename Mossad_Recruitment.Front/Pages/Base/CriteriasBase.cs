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
        protected RadzenDataGrid<Criteria> CriteriaDataGrid { get; set; }
        protected bool IsSaveButtonBusy { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var criteriasAlreadySet = await CriteriaService.Get();
            var technologies = await TechnologyService.GetAll(); // may be retrieved from local storage

            var criterias = new List<Criteria>();
            foreach (var technology in technologies)
            {
                var criteria = criteriasAlreadySet.FirstOrDefault(x => x.TechnologyId == technology.Key);
                criterias.Add(criteria ?? new Criteria
                {
                    TechnologyId = technology.Key,
                    TechnologyName = technology.Value.Name
                });
            }

            Criterias = criterias;
        }

        protected void OnChange(string numberOfYears, Criteria data)
        {
            if (int.TryParse(numberOfYears, out var noy) && noy != 0)
            {
                var criteria = Criterias.FirstOrDefault(x => x.TechnologyId == data.TechnologyId);
                if (criteria is not null)
                {
                    criteria.YearsOfExperience = noy;
                }
            }
        }

        protected async Task OnSaveButtonClick()
        {
            IsSaveButtonBusy = true;

            await Task.Delay(2000); // just to work out the animation
            await CriteriaService.Set(Criterias.Where(x => x.YearsOfExperience > 0));

            IsSaveButtonBusy = false;
        }
    }
}