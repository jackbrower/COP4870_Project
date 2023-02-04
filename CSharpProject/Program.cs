
using App.TaskManagement.Helpers;
using Library.TaskManagement.Models;

namespace CSharpProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var studentHelper = new StudentHelper();

            bool cont = true;
            while (cont)
            {
                Console.WriteLine("Choose an action: ");
                Console.WriteLine("1. Add student enrollment");
                Console.WriteLine("2. List all students");
                Console.WriteLine("3. Exit");
                var input = Console.ReadLine();

                if (int.TryParse(input, out var result))
                {
                    if (result == 1)
                    {
                        studentHelper.CreateStudentRecord();
                    }
                    else if (result == 2)
                    {
                        studentHelper.ListStudents();
                    }
                    else if (result == 3)
                    {
                        cont = false;
                    }
                }
            }
        }
    }
}