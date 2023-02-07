using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.LearningManagement.Helpers
{
    internal class CourseHelper
    {
        private CourseService courseService;
        private StudentService studentService;

        public CourseHelper()
        {
            courseService = CourseService.current;
            studentService = StudentService.current;
        }

        public void CreateCourseRecord(Course? selectedCourse = null)
        {
            Console.WriteLine("What is the code of the course?");
            var code = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("What is the name of the course?");
            var name = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("What is the description of the course?");
            var desc = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Which students should be enrolled in this course? ('Q' to quit)");
            var roster = new List<Person>();
            bool continueAdding = true;
            while(continueAdding)
            {
                studentService.Students.Where(s => !roster.Any(s2 => s2.ID == s.ID)).ToList().ForEach(Console.WriteLine);
                var selection = "Q";
                if (studentService.Students.Any(s => !roster.Any(s2 => s2.ID == s.ID)))
                {
                    selection = Console.ReadLine() ?? String.Empty;
                }

                if(selection.Equals("Q", StringComparison.InvariantCultureIgnoreCase))
                {
                    continueAdding = false;
                }
                else
                {
                    var selectedID = int.Parse(selection);
                    var selectedStudent = studentService.Students.FirstOrDefault(s => s.ID == selectedID);

                    if(selectedStudent != null)
                    {
                        roster.Add(selectedStudent);
                    }
                }
            }

            bool isNewCourse = false;
            if(selectedCourse == null)
            {
                isNewCourse = true;
                selectedCourse = new Course();
            }

            selectedCourse.Code = code;
            selectedCourse.Name = name;
            selectedCourse.Description = desc;
            selectedCourse.Roster = new List<Person>();
            selectedCourse.Roster.AddRange(roster);

            if (isNewCourse)
            {
                courseService.Add(selectedCourse);
            }
        }

        public void AddStudentToCourse()
        {

        }

        public void ListCourses()
        {
            courseService.Courses.ForEach(Console.WriteLine);
        }

        public void SearchCourses()
        {
            Console.WriteLine("Enter a query: ");
            var query = Console.ReadLine() ?? string.Empty;

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(query, StringComparison.InvariantCultureIgnoreCase));

            if (selectedCourse != null)
            {
                Console.WriteLine($"{selectedCourse.Code} - {selectedCourse.Name}\n{selectedCourse.Description}");
                selectedCourse.Roster.ForEach(Console.WriteLine);
            }
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
