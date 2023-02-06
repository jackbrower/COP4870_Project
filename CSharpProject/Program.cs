
using App.TaskManagement.Helpers;
using Library.TaskManagement.Models;

namespace App.TaskManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var studentHelper = new StudentHelper();
            var courseHelper = new CourseHelper();

            bool cont = true;
            while (cont)
            {
                Console.WriteLine("Choose an action: ");
                Console.WriteLine("1. Add student enrollment");
                Console.WriteLine("2. Update a student");
                Console.WriteLine("3. List all students");
                Console.WriteLine("4. Search for a student");
                Console.WriteLine("5. Add a new course");
                Console.WriteLine("6. Update a course");
                Console.WriteLine("7. List all courses");
                Console.WriteLine("8. Search for a course");
                Console.WriteLine("9. List a course's description");
                Console.WriteLine("10. Add a student to a course");
                Console.WriteLine("11. Remove a student from a course");
                Console.WriteLine("12. List all courses a student is taking");
                Console.WriteLine("13. Create an assignment for a course");
                Console.WriteLine("14. Exit");
                var input = Console.ReadLine();

                if (int.TryParse(input, out var result))
                {
                    if (result == 1)
                    {
                        studentHelper.CreateStudentRecord();
                    }
                    else if (result == 2)
                    {
                        studentHelper.UpdateStudentRecord();
                    }
                    else if (result == 3)
                    {
                        studentHelper.ListStudents();
                    }
                    else if (result == 4)
                    {
                        studentHelper.SearchStudents();
                    }
                    else if (result == 5)
                    {
                        courseHelper.CreateCourseRecord();
                    }
                    else if (result == 6)
                    {
                        courseHelper.UpdateCourseRecord();
                    }
                    else if (result == 7)
                    {
                        courseHelper.ListCourses();
                    }
                    else if (result == 8)
                    {
                        courseHelper.SearchCourses();
                    }
                    else if (result == 9)
                    {

                    }
                    else if (result == 10)
                    {

                    }
                    else if (result == 11)
                    {

                    }
                    else if (result == 12)
                    {

                    }
                    else if (result == 13)
                    {

                    }
                    else if (result == 14)
                    {
                        cont = false;
                    }
                }
            }
        }
    }
}