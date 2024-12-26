using HRManagment.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRManagment.ViewModels
{
    public class DropDownService
    {
        private readonly HRManagmentContext context;
        public DropDownService(HRManagmentContext context)
        {
            this.context = context;
        }

        public List<SelectListItem> GetDepartments() => context.Departments
            .Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name,
            }).ToList();

        public List<SelectListItem> GetPositions() => Enum.GetValues<Position>()
            .Select(p => new SelectListItem
            {
                Value = p.ToString(),
                Text = p.ToString()
            }).ToList();

        public List<SelectListItem> GetGenders() => Enum.GetValues<GenderType>()
            .Select(g => new SelectListItem
            {
                Value = ((int)g).ToString(),
                Text = g.ToString(),
            }).ToList();
    }
}
