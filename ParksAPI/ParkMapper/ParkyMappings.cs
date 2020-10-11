using AutoMapper;
using ParksAPI.Models;
using ParksAPI.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParksAPI.ParkMapper
{
    public class ParkyMappings: Profile
    {
        public ParkyMappings()
        {
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
        }
    }
}
