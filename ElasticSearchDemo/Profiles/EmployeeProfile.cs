using AutoMapper;
using ElasticSearchDemo.Models;
using ElasticSearchDemo.Models.Elastic;

namespace ElasticSearchDemo.Profiles
{
    public class EmployeeProfile : Profile
    {

        //ReverseMap :for birectional mapping
        public EmployeeProfile()
        {
            CreateMap<Employee, ElasticEmployee>()
                .ForMember(q => q.IsPermanent, option => option.MapFrom(src => src.IsEmployeePermanent)).ReverseMap();

            CreateMap<Skill, ElasticSkill>();
        }
    }
}
