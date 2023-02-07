﻿using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace App.LearningManagement.Helpers
{
    internal class StudentHelper
    {
        private CourseService courseService;
        private StudentService studentService;
        
        public StudentHelper()
        {
            courseService = CourseService.current;
            studentService = StudentService.current;
        }

        public void CreateStudentRecord(Person? selectedStudent = null)
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
            else
            {
                classEnum = PersonClassification.Freshman;
            }

            bool isCreate = false;
            if (selectedStudent == null)
            {
                isCreate = true;
                selectedStudent = new Person();
            }

            selectedStudent.ID = int.Parse(id ?? "0");
            selectedStudent.Name = name ?? string.Empty;
            selectedStudent.Classification = classEnum;

            if (isCreate)
            {
                studentService.Add(selectedStudent);
            }
            
        }

        public void ListStudents()
        {
            studentService.Students.ForEach(Console.WriteLine);
        }

        public void SearchStudents()
        {
            Console.WriteLine("Enter a query: ");
            var query = Console.ReadLine() ?? string.Empty;

            studentService.Search(query).ToList().ForEach(Console.WriteLine);
        }

        public void UpdateStudentRecord()
        {
            Console.WriteLine("Select a student to update: ");
            ListStudents();
            
            var selectionStr = Console.ReadLine();

            if(int.TryParse(selectionStr, out int selectionInt))
            {
                var selectedStudent = studentService.StudentList.FirstOrDefault(s => s.ID == selectionInt);
                if(selectedStudent != null)
                {
                    CreateStudentRecord(selectedStudent);
                }
            }
        }

        public void ListCourses()
        {
            Console.WriteLine("Select a student to see courses for (ID): ");
            ListStudents();
            
            var selection = Console.ReadLine() ?? string.Empty;
            var student = studentService.StudentList.FirstOrDefault(s => s.ID == Int32.Parse(selection));

            if (student == null) return;
            courseService.Courses.ForEach(c =>
            {
                if (c.Roster.Contains(student))
                {
                    Console.WriteLine(c);
                }
            });
        }
    }
}
