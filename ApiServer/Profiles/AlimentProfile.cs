using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace ApiServer.Profiles
{
    // used by AutoMapper
    public class AlimentProfile : Profile
    {
        public AlimentProfile()
        {
            /// <summary>
            /// Maps between VIEWMODEL and MODEL
            /// Source -> Target
            /// </summary>
            CreateMap<Aliment, AlimentReadDto>();
            CreateMap<AlimentCreateDto, Aliment>();
            CreateMap<AlimentUpdateDto, Aliment>();
            CreateMap<Aliment, AlimentUpdateDto>();
        }
    }
}
