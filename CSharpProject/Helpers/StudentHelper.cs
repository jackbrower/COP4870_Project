using Library.TaskManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.TaskManagement.Helpers
{
    internal class StudentHelper
    {
        private List<Person> StudentList = new List<Person>();

        public void CreateStudentRecord()
        {
            Console.WriteLine("What is the ID of the student?");
            var id = Console.ReadLine();

            Console.WriteLine("What is the name of the student?");
            var name = Console.ReadLine();

            Console.WriteLine("What is the classification of the student? [(F)reshman, S(O)phomore, (J)unior, (S)enior]");
            var classification = Console.ReadLine() ?? string.Empty;
            PersonClassification classEnum;

            if (classification.Equals("O", StringComparison.InvariantCultureIgnoreCase))
            {
                classEnum = PersonClassification.Sophomore;
            }
            else if (classification.Equals("J", StringComparison.InvariantCultureIgnoreCase))
            {
                classEnum = PersonClassification.Junior;
            }
            else if (classification.Equals("S", StringComparison.InvariantCultureIgnoreCase))
            {
                classEnum = PersonClassification.Senior;
            }
            else (classification.Equals("F", StringComparison.InvariantCultureIgnoreCase))
            {
                classEnum = PersonClassification.Freshman;
            }

            var student = new Person
            {
                ID = int.Parse(id ?? "0"),
                Name = name ?? string.Empty,
                Classification = classEnum
            };

            StudentList.Add(student);

            StudentList.ForEach(Console.WriteLine);
        }
    }
}
