using HRManagment.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRManagment.Services
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

        public List<SelectListItem> GetGovernorates() => Enum.GetValues<Governorate>()
            .Select(g => new SelectListItem
            {
                Value = g.ToString(),
                Text = g.ToString()
            }).ToList();
    }
}
