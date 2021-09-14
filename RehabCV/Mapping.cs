using AutoMapper;
using RehabCV.DTO;
using RehabCV.Models;

namespace RehabCV
{
    public class Mapping : Profile
    {
        public MapperConfiguration Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChildDTO, Child>()
                    .ForMember(x => x.FirstName, y => y.MapFrom(t => t.FirstNameOfChild))
                    .ForMember(x => x.MiddleName, y => y.MapFrom(t => t.MiddleNameOfChild))
                    .ForMember(x => x.LastName, y => y.MapFrom(t => t.LastNameOfChild))
                    .ForMember(x => x.Birthday, y => y.MapFrom(t => t.BirthdayOfChild)); 
                cfg.CreateMap<Child, ChildDTO>()
                    .ForMember(x => x.FirstNameOfChild, y => y.MapFrom(t => t.FirstName))
                    .ForMember(x => x.MiddleNameOfChild, y => y.MapFrom(t => t.MiddleName))
                    .ForMember(x => x.LastNameOfChild, y => y.MapFrom(t => t.LastName))
                    .ForMember(x => x.BirthdayOfChild, y => y.MapFrom(t => t.Birthday));
            });

            return config;
        }
    }
}
