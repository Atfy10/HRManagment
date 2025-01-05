using AutoMapper;
using HRManagment.Models;
using HRManagment.ViewModels;

namespace HRManagment.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            // EmployeeViewModel --> Employee
            CreateMap<EmployeeViewModel, Employee>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));

            // Employee --> EmployeeViewModel
            CreateMap<Employee, EmployeeViewModel>();

            // EmployeeFormViewModel --> EmployeeViewModel
            //CreateMap<EmployeeFormViewModel, EmployeeViewModel>();

        }
    }
}
