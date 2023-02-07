using Library.LearningManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.LearningManagement.Services
{
    public class StudentService
    {
        public List<Person> StudentList;

        private static StudentService? _instance;

        private StudentService()
        {
            StudentList = new List<Person>();
        }

        public static StudentService current
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new StudentService();
                }

                return _instance;
            }
        }

        public void Add(Person student)
        {
            StudentList.Add(student);
        }

        public List<Person> Students
        {
            get
            {
                return StudentList;
            }
        }

        public IEnumerable<Person> Search(string query)
        {
            return StudentList.Where(s => s.Name.ToUpper().Contains(query.ToUpper()));
        }
    }
}
