﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Course.Web.Models.Catalog
{
    public class CourseUpdateInput
    {
        public string Id { get; set; }

        [Display(Name = "Kurs ismi")]
        public string Name { get; set; }

        [Display(Name = "Kurs açıklaması")]
        public string Description { get; set; }

        [Display(Name = "Kurs fiyatı")]
        public decimal Price { get; set; }

        public string UserId { get; set; }

        public string Picture { get; set; }

        public FeatureViewModel Feature { get; set; }

        [Display(Name = "Kurs kategorisi")]
        public string CategoryId { get; set; }
        
        [Display(Name = "Kurs Resmi")]
        public IFormFile PhotoFormFile { get; set; }
    }
}
