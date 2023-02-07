using Library.LearningManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.LearningManagement.Services
{
    public class CourseService
    {
        public List<Course> courseList;

        private static CourseService? _instance;

        private CourseService()
        {
            courseList = new List<Course>();
        }

        public static CourseService current
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CourseService();
                }

                return _instance;
            }
        }

        public void Add(Course course)
        {
            courseList.Add(course);
        }

        public List<Course> Courses
        {
            get
            {
                return courseList;
            }
        }

        public IEnumerable<Course> Search(string query)
        {
            return courseList.Where(c => c.Name.ToUpper().Contains(query.ToUpper())
            || c.Description.ToUpper().Contains(query.ToUpper())
            || c.Code.ToUpper().Contains(query.ToUpper()));
        }
        
        public static void AddAssignment(Course course, Assignment assignment)
        {
            course.Assignments.Add(assignment);
        }
    }
}
