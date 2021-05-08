﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Course.Services.Catalog.Dtos;
using Course.Services.Catalog.Model;

namespace Course.Services.Catalog.Mapping
{
    public class GeneralMapping: Profile
    {
        public GeneralMapping()
        {
            CreateMap<Model.Course, CourseDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Feature, FeatureDto>().ReverseMap();
            CreateMap<Model.Course, CourseCreateDto>().ReverseMap();
            CreateMap<Model.Course, CourseUpdateDto>().ReverseMap();
        }
    }
}
