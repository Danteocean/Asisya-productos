using AutoMapper;
using CoreLibrary.DTOs.Employee.Requests;
using Domain.Entities;

namespace CoreLibrary.Mappings;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<EmployeeAddDtoRequest, Employee>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); 
    }
}