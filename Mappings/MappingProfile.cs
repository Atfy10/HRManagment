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
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Employee --> EmployeeViewModel
            CreateMap<Employee, EmployeeViewModel>()
                .ForMember(dest => dest.Departments, opt => opt.Ignore())
                .ForMember(dest => dest.Genders, opt => opt.Ignore())
                .ForMember(dest => dest.Positions, opt => opt.Ignore());
                
        }
    }
}
