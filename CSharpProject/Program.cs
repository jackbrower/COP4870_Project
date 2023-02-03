
using App.TaskManagement.Helpers;
using Library.TaskManagement.Models;

namespace CSharpProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Choose an action: ");
            Console.WriteLine("1. Add student enrollment");
            Console.WriteLine("2. Exit")
            var input = Console.ReadLine();

            if(int.TryParse(input, out var result))
            {
                if(result == 1)
                {
                    new StudentHelper().CreateStudentRecord();
                }
            }
        }
    }
}