
using Library.TaskManagement.Models;

namespace CSharpProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Person> StudentList = new List<Person>();

            Console.WriteLine("What is the ID of the student?");
            var id = Console.ReadLine();

            Console.WriteLine("What is the name of the student?");
            var name = Console.ReadLine();

            Console.WriteLine("What is the classification of the student?");
            var classification = Console.ReadLine();

            var student = new Person
            {
                ID = int.Parse(id ?? "0"),
                Name = name ?? string.Empty,
                Classification = string.IsNullOrEmpty(classification) ? 'F' : classification.ToUpper()[0]
            };

            StudentList.Add(student);

            StudentList.ForEach(Console.WriteLine);
        }
    }
}