using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using unclesam.Models;

namespace unclesam.Factories.AutoMapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<VmNewsComment, NewsComment>();
            CreateMap<NewsComment, VmNewsComment>();
        }
    }
}
