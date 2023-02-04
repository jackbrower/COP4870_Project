using Library.TaskManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TaskManagement.Services
{
    public class StudentService
    {
        public List<Person> StudentList = new List<Person>();

        public void Add(Person student)
        {
            StudentList.Add(student);
        }

        public void ListStudents()
        {
            StudentList.ForEach(Console.WriteLine);
        }
    }
}
