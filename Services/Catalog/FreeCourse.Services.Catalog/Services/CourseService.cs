﻿using AutoMapper;
using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Services
{
    public class CourseService:ICourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;
        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
            _mapper = mapper;
        }

        public async Task<Response<List<CourseDto>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find(course => true).ToListAsync();
            if(courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstAsync();
                }
            }
            else
            {
                courses = new List<Course>();
            }
            return Response<List<CourseDto>>.Success(this._mapper.Map<List<CourseDto>>(courses), (int)HttpStatusCode.OK);
        }

        public async Task<Response<CourseDto>> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if(course == null)
            {
                return Response<CourseDto>.Fail("Course not found", (int)HttpStatusCode.NotFound);
            }
            course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
            return Response<CourseDto>.Success(this._mapper.Map<CourseDto>(course), (int)HttpStatusCode.OK);
        }

        public async Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
        {
            var courses = await _courseCollection.Find<Course>(x => x.UserId == userId).ToListAsync();
            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstAsync();
                }
            }
            else
            {
                courses = new List<Course>();
            }
            return Response<List<CourseDto>>.Success(this._mapper.Map<List<CourseDto>>(courses), (int)HttpStatusCode.OK);

        }

        public async Task<Response<CourseDto>> CreateAsync(CourseCreateDto input)
        {
            var newCourse = this._mapper.Map<Course>(input);
            newCourse.CreatedTime = DateTime.Now;
            await _courseCollection.InsertOneAsync(newCourse);
            return Response<CourseDto>.Success(this._mapper.Map<CourseDto>(newCourse), (int)HttpStatusCode.OK);
        }
        public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDto input)
        {
            var updatedCourse = this._mapper.Map<Course>(input);
            var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == input.Id, updatedCourse);
            if(result == null)
            {
                return Response<NoContent>.Fail("Course not found.", (int)HttpStatusCode.NotFound);
            }
            return Response<NoContent>.Success((int)HttpStatusCode.NoContent);
        }

        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
            var result = await _courseCollection.DeleteOneAsync(x => x.Id == id);
            if(result.DeletedCount > 0)
            {
                return Response<NoContent>.Success((int)HttpStatusCode.NoContent);
            }

            return Response<NoContent>.Fail("Course not found",(int)HttpStatusCode.NotFound);
        }
    }
}
