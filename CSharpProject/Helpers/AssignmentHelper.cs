using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.LearningManagement.Helpers;

public class AssignmentHelper
{
        private CourseService courseService;
        private StudentService studentService;

        public AssignmentHelper()
        {
            courseService = CourseService.current;
            studentService = StudentService.current;
        }

        public void CreateAssignment()
        {
            Console.WriteLine("What is the name of the assignment?");
            var name = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("What is the description of this assignment?");
            var desc = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("How many points are available on this assignment?");
            var points = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("What is the due date for this assignment? (MM/DD/YYYY HH:MM:SS AM/PM)");
            var duedate = Console.ReadLine() ?? string.Empty;
            DateTime dateconv = DateTime.Parse(duedate, System.Globalization.CultureInfo.InvariantCulture);
            
            Console.WriteLine("Which course is this assignment for?");
            var course = Console.ReadLine() ?? string.Empty;

            Course courseret = courseService.Courses.FirstOrDefault(s => s.Code.Equals(course, StringComparison.InvariantCultureIgnoreCase));

            var assignment = new Assignment()
            {
                Name = name,
                Description = desc,
                TotalAvailablePoints = Int32.Parse(points),
                DueDate = dateconv
            };

            if (courseret == null) return;
            
            CourseService.AddAssignment(courseret, assignment);
        }
}