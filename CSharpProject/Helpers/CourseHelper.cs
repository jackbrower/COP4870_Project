using Library.TaskManagement.Models;
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

        public void CreateCourseRecord()
        {
            Console.WriteLine("What is the code of the course?");
            var code = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("What is the name of the course?");
            var name = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("What is the description of the course?");
            var desc = Console.ReadLine() ?? string.Empty;

            var course = new Course
            {
                Code = code,
                Name = name,
                Description = desc
            };

            courseService.Add(course);

            courseService.courseList.ForEach(Console.WriteLine);
        }

        public void ListCourses()
        {
            courseService.Courses.ForEach(Console.WriteLine);
        }
    }
}
