using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Course.Services.Catalog.Dtos;
using Course.Services.Catalog.Services.Abstract;
using Course.Shared.ControllerBase;

namespace Course.Services.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : CustomBaseController
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var course = await _courseService.GetAllAsync();

            return CreateActionResultInstance(course);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var course = await _courseService.GetByIdAsync(id);

            return CreateActionResultInstance(course);
        }

        [HttpGet]
        [Route("/api/[controller]/GetAllByUserId/{userId}")]
        public async Task<IActionResult> GetAllByUserId(string userId)
        {
            var course = await _courseService.GetAllByUserIdAsync(userId);

            return CreateActionResultInstance(course);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateDto courseCreateDto)
        {
            var course = await _courseService.CreateAsync(courseCreateDto);

            return CreateActionResultInstance(course);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CourseUpdateDto courseUpdateDto)
        {
            var course = await _courseService.UpdateAsync(courseUpdateDto);

            return CreateActionResultInstance(course);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var course = await _courseService.DeleteAysnc(id);

            return CreateActionResultInstance(course);
        }
    }
}
