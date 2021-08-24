using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Course.Services.Catalog.Dtos;
using Course.Services.Catalog.Model;
using Course.Services.Catalog.Services.Abstract;
using Course.Services.Catalog.Settings;
using Course.Shared.Dtos;
using Course.Shared.Messages.Events.Concreate;
using MassTransit;
using MongoDB.Driver;

namespace Course.Services.Catalog.Services.Concrete
{
    public class CourseService : ICourseService
    {
        private readonly IMongoCollection<Model.Course> _courseCollection;

        private readonly IMongoCollection<Category> _categoryCollection;

        private readonly IMapper _mapper;

        private readonly IPublishEndpoint _publishEndpoint;

        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings, IPublishEndpoint publishEndpoint)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _courseCollection = database.GetCollection<Model.Course>(databaseSettings.CourseCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Shared.Dtos.Response<List<CourseDto>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find(course => true).ToListAsync();

            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstAsync();
                }
            }
            else
            {
                courses = new List<Model.Course>();
            }

            return Shared.Dtos.Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<Shared.Dtos.Response<CourseDto>> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (course == null)
            {
                return Shared.Dtos.Response<CourseDto>.Fail("Course not found", 404);
            }

            course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstAsync();

            return Shared.Dtos.Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);
        }

        public async Task<Shared.Dtos.Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
        {
            var courses = await _courseCollection.Find(x => x.UserId == userId).ToListAsync();

            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstAsync();
                }
            }
            else
            {
                courses = new List<Model.Course>();
            }

            return Shared.Dtos.Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);

        }

        public async Task<Shared.Dtos.Response<CourseDto>> CreateAsync(CourseCreateDto couseCreateDto)
        {
            var newCourse = _mapper.Map<Model.Course>(couseCreateDto);
            newCourse.CreatedTime = DateTime.Now;
            await _courseCollection.InsertOneAsync(newCourse);

            return Shared.Dtos.Response<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse), 200);
        }

        public async Task<Shared.Dtos.Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
        {
            var updateCourse = _mapper.Map<Model.Course>(courseUpdateDto);
            var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == updateCourse.Id, updateCourse);

            if (result == null)
            {
                return Shared.Dtos.Response<NoContent>.Fail("Course not found", 404);
            }

            await _publishEndpoint.Publish<CourseNameChangedEvent>(new()
            {
                CourseId = courseUpdateDto.Id,
                UpdatedName = courseUpdateDto.Name
            });

            await _publishEndpoint.Publish<BasketCourseNameChangedEvent>(new()
            {
                CourseId = courseUpdateDto.Id,
                UpdatedName = courseUpdateDto.Name,
                UserId = courseUpdateDto.UserId
            });

            return Shared.Dtos.Response<NoContent>.Success(204);
        }

        public async Task<Shared.Dtos.Response<NoContent>> DeleteAysnc(string id)
        {
            var result = await _courseCollection.DeleteOneAsync(x => x.Id == id);

            if (result.DeletedCount <= 0)
            {
                return Shared.Dtos.Response<NoContent>.Fail("Course Not Found", 404);
            }

            return Shared.Dtos.Response<NoContent>.Success(204);
        }
    }
}
