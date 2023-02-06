﻿using Library.TaskManagement.Models;
using Library.TaskManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.TaskManagement.Helpers
{
    internal class CourseHelper
    {
        private CourseService courseService = new CourseService();

        public void CreateCourseRecord(Course? selectedCourse = null)
        {
            Console.WriteLine("What is the code of the course?");
            var code = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("What is the name of the course?");
            var name = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("What is the description of the course?");
            var desc = Console.ReadLine() ?? string.Empty;

            bool isNewCourse = false;
            if(selectedCourse == null)
            {
                isNewCourse = true;
                selectedCourse = new Course();
            }

            selectedCourse.Code = code;
            selectedCourse.Name = name;
            selectedCourse.Description = desc;

            if (isNewCourse)
            {
                courseService.Add(selectedCourse);
            }
        }

        public void ListCourses()
        {
            courseService.Courses.ForEach(Console.WriteLine);
        }

        public void SearchCourses()
        {
            Console.WriteLine("Enter a query: ");
            var query = Console.ReadLine() ?? string.Empty;

            courseService.Search(query).ToList().ForEach(Console.WriteLine);
        }

        public void UpdateCourseRecord()
        {
            Console.WriteLine("Enter the code of a course to update: ");
            ListCourses();

            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
            if (selectedCourse != null)
            {
                CreateCourseRecord(selectedCourse);
            }
        }
    }
}
