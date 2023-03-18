using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.LearningManagement.Models
{
    public class Student : Person
    {
        // Dict is sorted where int = class credit hours, double = relative weighted average
        // Do not need to store each individual grade, just query the class for total grade.
        public Dictionary<int, double> Grades { get; set; }
        
        // Query assignments for course, double is grade (points)
        public Dictionary<Assignment, double> Submissions { get; set; }

        public PersonClassification Classification { get; set; }

        public Student() {
            Grades = new Dictionary<int, double>();
        }

        public override string ToString()
        {
            return $"[{Id}] {Name} - {Classification}";
        }

        public float GPA { get; set; }
    }

    public enum PersonClassification
    {
        Freshman, Sophomore, Junior, Senior
    }
}
