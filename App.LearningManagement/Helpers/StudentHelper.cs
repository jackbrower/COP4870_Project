using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using COP4870_Project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.LearningManagement.Helpers
{
    internal class StudentHelper
    {
        private StudentService studentService;
        private CourseService courseService;
        private ListNavigator<Student> studentNavigator;

        public StudentHelper()
        {
            studentService = StudentService.Current;
            courseService = CourseService.Current;

            studentNavigator = new ListNavigator<Student>(studentService.Students, 2);
        }

        public void CreateStudentRecord(Student? selectedStudent = null)
        {
            bool isCreate = false;
            if (selectedStudent == null)
            {
                isCreate = true;
                Console.WriteLine("What type of person would you like to add?");
                Console.WriteLine("(S)tudent");
                //Console.WriteLine("(T)eachingAssistant");
                //Console.WriteLine("(I)nstructor");
                var choice = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrEmpty(choice))
                {
                    return;
                }

                if (choice.Equals("S", StringComparison.InvariantCultureIgnoreCase))
                {
                    selectedStudent = new Student();
                }
                // TODO: Reimplement TA and instructor 
            }

            Console.WriteLine("What is the name of the student?");
            var name = Console.ReadLine();
            if (selectedStudent is Student)
            {
                Console.WriteLine("What is the classification of the student? [(F)reshman, S(O)phomore, (J)unior, (S)enior]");
                var classification = Console.ReadLine() ?? string.Empty;
                PersonClassification classEnum = PersonClassification.Freshman;

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
                var studentRecord = selectedStudent as Student;
                if (studentRecord != null)
                {
                    studentRecord.Classification = classEnum;
                    studentRecord.Name = name ?? string.Empty;

                    if (isCreate)
                    {
                        studentService.Add(selectedStudent);
                    }
                }
            } else {
                if (selectedStudent != null)
                {
                    selectedStudent.Name = name ?? string.Empty;
                    if (isCreate)
                    {
                        studentService.Add(selectedStudent);
                    }
                }
            }
        }

        public void UpdateStudentRecord()
        {
            Console.WriteLine("Select a person to update:");
            studentService.Students.ForEach(Console.WriteLine);

            var selectionStr = Console.ReadLine();

            if (int.TryParse(selectionStr, out int selectionInt))
            {
                var selectedStudent = studentService.Students.FirstOrDefault(s => s.Id == selectionInt);
                if (selectedStudent != null)
                {
                    CreateStudentRecord(selectedStudent);
                }
            }
        }

        private void NavigateStudents(string? query = null)
        {
            ListNavigator<Student>? currentNavigator = null;
            if(query == null)
            {
                currentNavigator = studentNavigator;
            }else
            {
                currentNavigator = new ListNavigator<Student>(studentService.Search(query).ToList(), 2);
            }
            
            bool keepPaging = true;
            while(keepPaging)
            {
                foreach (var pair in currentNavigator.GetCurrentPage())
                {
                    Console.WriteLine($"{pair.Key}. {pair.Value}");
                }

                if (currentNavigator.HasPreviousPage)
                {
                    Console.WriteLine("P. Previous Page");
                }

                if (currentNavigator.HasNextPage)
                {
                    Console.WriteLine("N. Next Page");
                }

                Console.WriteLine("Make a selection:");
                var selectionStr = Console.ReadLine();

                if ((selectionStr?.Equals("P", StringComparison.InvariantCultureIgnoreCase) ?? false)
                    || (selectionStr?.Equals("N", StringComparison.InvariantCultureIgnoreCase) ?? false)) {
                    //Navigate through pages
                    if(selectionStr.Equals("P", StringComparison.InvariantCultureIgnoreCase))
                    {
                        currentNavigator.GoBackward();
                    }
                    else if (selectionStr.Equals("N", StringComparison.InvariantCultureIgnoreCase))
                    {
                        currentNavigator.GoForward();
                    }
                }
                else
                {
                    var selectionInt = int.Parse(selectionStr ?? "0");

                    Console.WriteLine("Student Course List:");
                    courseService.Courses.Where(c => c.Roster.Any(s => s.Id == selectionInt)).ToList().ForEach(Console.WriteLine);
                    keepPaging = false;
                }
            }
        }

        public double CalcGPA(Student student)
        {
            double TotalStudentCredits;
            double gpa = TotalStudentCredits = new double();
            Dictionary<Course, double> TotalCourseScore;
            Dictionary<Course, double> StudentCourseGrades = TotalCourseScore = new Dictionary<Course, double>();

            foreach (var allc in courseService.Courses)
            {
                foreach (var alla in allc.Assignments)
                {  
                    TotalCourseScore[allc] += alla.TotalAvailablePoints;
                }
            }

            foreach (var submits in student.Submissions)
            {
                StudentCourseGrades[submits.Key.ParentCourse] += submits.Value;
            }

            foreach (var courses in TotalCourseScore)
            {
                TotalStudentCredits += courses.Key.CreditHours;
            }

            double pre_div_grade_sum = new double();
            foreach (var grades in StudentCourseGrades)
            {
                var coursecred = grades.Key.CreditHours;
                var studentpts = grades.Value;

                pre_div_grade_sum += (coursecred * (studentpts / TotalCourseScore[grades.Key]));
            }

            return pre_div_grade_sum / TotalStudentCredits;
        }

        public void ListStudents()
        {
            NavigateStudents();    
        }

        public void SearchStudents()
        {
            Console.WriteLine("Enter a query:");
            var query = Console.ReadLine() ?? string.Empty;

            //studentService.Search(query).ToList().ForEach(Console.WriteLine);
            //var selectionStr = Console.ReadLine();
            //var selectionInt = int.Parse(selectionStr ?? "0");

            //Console.WriteLine("Student Course List:");
            //courseService.Courses.Where(c => c.Roster.Any(s => s.Id == selectionInt)).ToList().ForEach(Console.WriteLine);
            NavigateStudents(query);
        }
    }
}
