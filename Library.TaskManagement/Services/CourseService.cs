using Library.TaskManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TaskManagement.Services
{
    public class CourseService
    {
        public List<Course> courseList = new List<Course>();

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
            || c.Code.ToUpper().Contains(query.ToUpper())   );
        }
    }
}
