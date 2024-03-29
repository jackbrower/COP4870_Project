﻿using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.LearningManagement.Helpers
{
    public class CourseHelper
    {
        private CourseService courseService;
        private StudentService studentService;

        public CourseHelper()
        {
            studentService= StudentService.Current;
            courseService = CourseService.Current;
        }

        public void CreateCourseRecord(Course? selectedCourse = null)
        {
            bool isNewCourse = false;
            if (selectedCourse == null)
            {
                isNewCourse = true;
                selectedCourse = new Course();
            }

            var choice = "Y";
            if (!isNewCourse)
            {
                Console.WriteLine("Do you want to update the course code?");
                choice = Console.ReadLine() ?? "N";
            }

            if(choice.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
            {
                CourseTag:
                Console.WriteLine("What is the code of the course?");
                selectedCourse.Code = Console.ReadLine() ?? string.Empty;

                var checkCourse = courseService.Courses.FirstOrDefault(c => c.Code.Equals(selectedCourse.Code, StringComparison.InvariantCultureIgnoreCase));
                // TODO: Dont use goto
                if (checkCourse != null)
                {
                    Console.WriteLine("Course code already exists.");
                    goto CourseTag;
                }
            }

            if(!isNewCourse)
            {
                Console.WriteLine("Do you want to update the course name?");
                choice = Console.ReadLine() ?? "N";
            } else
            {
                choice = "Y";
            }
            if (choice.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("What is the name of the course?");
                selectedCourse.Name = Console.ReadLine() ?? string.Empty;
            }

            if(!isNewCourse)
            {
                Console.WriteLine("Do you want to update the course description?");
                choice = Console.ReadLine() ?? "N";
            } else
            {
                choice = "Y";
            }
            if (choice.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("What is the description of the course?");
                selectedCourse.Description = Console.ReadLine() ?? string.Empty;
            }

            if (isNewCourse)
            {

                SetupRoster(selectedCourse);
                SetupAssignments(selectedCourse);
                SetupModules(selectedCourse);
            }

            if (!isNewCourse)
            {
                Console.WriteLine("How many credit hours is this course?");
                var preconvInt = Console.ReadLine() ?? string.Empty;
                selectedCourse.CreditHours = Int32.Parse(preconvInt);
            }
            
            
            if(isNewCourse)
            {
                courseService.Add(selectedCourse);
            }

        }

        public void UpdateCourseRecord()
        {
            Console.WriteLine("Enter the code for the course to update:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
            if (selectedCourse != null)
            {
                CreateCourseRecord(selectedCourse);
            }
        }

        public void SearchCourses(string? query = null)
        {
            if(string.IsNullOrEmpty(query))
            {
                courseService.Courses.ForEach(Console.WriteLine);
            } else
            {
                courseService.Search(query).ToList().ForEach(Console.WriteLine);
            }

            Console.WriteLine("Select a course:");
            var code = Console.ReadLine() ?? string.Empty;

            var selectedCourse = courseService
                .Courses
                .FirstOrDefault(c => c.Code.Equals(code, StringComparison.InvariantCultureIgnoreCase));
            if(selectedCourse != null)
            {
                Console.WriteLine(selectedCourse.DetailDisplay);
            }
        }
        public void AddStudent()
        {
            Console.WriteLine("Enter the code for the course to add the student to:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
            if (selectedCourse != null)
            {
                studentService.Students.Where(s => !selectedCourse.Roster.Any(s2 => s2.Id == s.Id)).ToList().ForEach(Console.WriteLine);
                if (studentService.Students.Any(s => !selectedCourse.Roster.Any(s2 => s2.Id == s.Id)))
                {
                    selection = Console.ReadLine() ?? string.Empty;
                }

                if(selection != null)
                {
                    var selectedId = int.Parse(selection);
                    var selectedStudent = studentService.Students.FirstOrDefault(s => s.Id == selectedId);
                    if (selectedStudent != null)
                    {
                        selectedCourse.Roster.Add(selectedStudent);
                    }
                }

            }
        }
        public void RemoveStudent()
        {
            Console.WriteLine("Enter the code for the course to remove the student from:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
            if (selectedCourse != null)
            {
                selectedCourse.Roster.ForEach(Console.WriteLine);
                if (selectedCourse.Roster.Any())
                {
                    selection = Console.ReadLine() ?? string.Empty;
                } else
                {
                    selection = null;
                }

                if (selection != null)
                {
                    var selectedId = int.Parse(selection);
                    var selectedStudent = studentService.Students.FirstOrDefault(s => s.Id == selectedId);
                    if (selectedStudent != null)
                    {
                        selectedCourse.Roster.Remove(selectedStudent);
                    }
                }

            }
        }
        public void AddAssignment()
        {
            Console.WriteLine("Enter the code for the course to add the assignment to:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
            if (selectedCourse != null)
            {
                var new_assn = CreateAssignment(selectedCourse);
                selectedCourse.Assignments.Add(new_assn);
                foreach (var s in selectedCourse.Roster)
                {
                    s.Submissions.Add(new_assn, -1);
                }
            }
        }
        public void AddModule()
        {
            Console.WriteLine("Enter the code for the course to add the module to:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
            if (selectedCourse != null)
            {
                selectedCourse.Modules.Add(CreateModule(selectedCourse));
            }
        }
        public void RemoveModule()
        {
            Console.WriteLine("Enter the code for the course:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));

            if (selectedCourse != null)
            {
                Console.WriteLine("Choose an module to delete:");
                selectedCourse.Modules.ForEach(Console.WriteLine);
                var selectionStr = Console.ReadLine() ?? string.Empty;
                var selectionInt = int.Parse(selectionStr);
                var selectedModule = selectedCourse.Modules.FirstOrDefault(m => m.Id == selectionInt);
                if (selectedModule != null)
                {
                    selectedCourse.Modules.Remove(selectedModule);
                }
            }
        }
        public void UpdateModule()
        {
            Console.WriteLine("Enter the code for the course:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
            
            if(selectedCourse != null && selectedCourse.Modules.Any())
            {
                Console.WriteLine("Enter the id for the module to update:");
                selectedCourse.Modules.ForEach(Console.WriteLine);

                selection = Console.ReadLine();
                var selectedModule = selectedCourse
                    .Modules
                    .FirstOrDefault(m => m.Id.ToString().Equals(selection, StringComparison.InvariantCultureIgnoreCase));

                if(selectedModule != null)
                {
                    Console.WriteLine("Would you like to modify the module name?");
                    selection = Console.ReadLine();
                    if (selection?.Equals("Y", StringComparison.InvariantCultureIgnoreCase) ?? false)
                    {
                        Console.WriteLine("Name:");
                        selectedModule.Name = Console.ReadLine();
                    }
                    Console.WriteLine("Would you like to modify the module description?");
                    selection = Console.ReadLine();
                    if (selection?.Equals("Y", StringComparison.InvariantCultureIgnoreCase) ?? false)
                    {
                        Console.WriteLine("Description:");
                        selectedModule.Description = Console.ReadLine();
                    }

                    Console.WriteLine("Would you like to delete content from this module?");
                    selection = Console.ReadLine();
                    if(selection?.Equals("Y", StringComparison.InvariantCultureIgnoreCase) ?? false)
                    {
                        var keepRemoving = true;
                        while(keepRemoving)
                        {
                            selectedModule.Content.ForEach(Console.WriteLine);
                            selection = Console.ReadLine();

                            var contentToRemove = selectedModule
                                .Content
                                .FirstOrDefault(c => c.Id.ToString().Equals(selection, StringComparison.InvariantCultureIgnoreCase));
                            if (contentToRemove != null)
                            {
                                selectedModule.Content.Remove(contentToRemove);
                            }

                            Console.WriteLine("would you like to remove more content?");
                            selection = Console.ReadLine();
                            if(selection?.Equals("N", StringComparison.InvariantCultureIgnoreCase) ?? false)
                            {
                                keepRemoving = false;
                            }
                        }
                        
                    }

                    Console.WriteLine("Would you like to add content?");
                    var choice = Console.ReadLine() ?? "N";
                    while (choice.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Console.WriteLine("What type of content would you like to add?");
                        Console.WriteLine("1. Assignment");
                        Console.WriteLine("2. File");
                        Console.WriteLine("3. Page");
                        var contentChoice = int.Parse(Console.ReadLine() ?? "0");

                        switch (contentChoice)
                        {
                            case 1:
                                var newAssignmentContent = CreateAssignmentItem(selectedCourse);
                                if (newAssignmentContent != null)
                                {
                                    selectedModule.Content.Add(newAssignmentContent);
                                }
                                break;
                            case 2:
                                var newFileContent = CreateFileItem(selectedCourse);
                                if (newFileContent != null)
                                {
                                    selectedModule.Content.Add(newFileContent);
                                }
                                break;
                            case 3:
                                var newPageContent = CreatePageItem(selectedCourse);
                                if (newPageContent != null)
                                {
                                    selectedModule.Content.Add(newPageContent);
                                }
                                break;
                            default:
                                break;
                        }

                        Console.WriteLine("Would you like to add more content?");
                        choice = Console.ReadLine() ?? "N";
                    }

                }

            }

        }
        public void ViewAssignment()
        {
            Console.WriteLine("Enter the code for the course:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
            if (selectedCourse != null)
            {
                Console.WriteLine("Choose an assignment to update:");
                selectedCourse.Assignments.ForEach(Console.WriteLine);
                var selectionStr = Console.ReadLine() ?? string.Empty;
                var selectionInt = int.Parse(selectionStr);
                var selectedAssignment = selectedCourse.Assignments.FirstOrDefault(a => a.Id == selectionInt);
                if(selectedAssignment != null)
                {
                    Console.Write(selectedAssignment);
                }
            }
        }
        public void UpdateAssignment()
        {
            Console.WriteLine("Enter the code for the course:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
            if (selectedCourse != null)
            {
                Console.WriteLine("Choose an assignment to update:");
                selectedCourse.Assignments.ForEach(Console.WriteLine);
                var selectionStr = Console.ReadLine() ?? string.Empty;
                var selectionInt = int.Parse(selectionStr);
                var selectedAssignment = selectedCourse.Assignments.FirstOrDefault(a => a.Id == selectionInt);
                if(selectedAssignment != null)
                {
                    var new_assn = CreateAssignment(selectedCourse);
                    var index = selectedCourse.Assignments.IndexOf(selectedAssignment);
                    selectedCourse.Assignments.RemoveAt(index);
                    selectedCourse.Assignments.Insert(index, new_assn);
                    
                    foreach (var s in selectedCourse.Roster)
                    {
                        s.Submissions.Add(new_assn, s.Submissions[selectedAssignment]);
                        s.Submissions.Remove(selectedAssignment);
                    }
                }
            }
        }
        public void RemoveAssignment()
        {
            Console.WriteLine("Enter the code for the course:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
            if (selectedCourse != null)
            {
                Console.WriteLine("Choose an assignment to delete:");
                selectedCourse.Assignments.ForEach(Console.WriteLine);
                var selectionStr = Console.ReadLine() ?? string.Empty;
                var selectionInt = int.Parse(selectionStr);
                var selectedAssignment = selectedCourse.Assignments.FirstOrDefault(a => a.Id == selectionInt);
                if (selectedAssignment != null)
                {
                    selectedCourse.Assignments.Remove(selectedAssignment);
                    
                    foreach (var s in selectedCourse.Roster)
                    {
                        s.Submissions.Remove(selectedAssignment);
                    }
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
        public void GradeSubmission()
        {
            Console.WriteLine("Enter the code for the course:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
            if (selectedCourse != null)
            {
                Console.WriteLine("Choose an assignment to grade:");
                selectedCourse.Assignments.ForEach(Console.WriteLine);
                var selectionStr = Console.ReadLine() ?? string.Empty;
                var selectionInt = int.Parse(selectionStr);
                var selectedAssignment = selectedCourse.Assignments.FirstOrDefault(a => a.Id == selectionInt);
                if(selectedAssignment != null)
                {
                    Console.WriteLine("Select a student to grade:");
                    selectedCourse.Roster.ForEach(Console.WriteLine);
                    var selectedId = Console.ReadLine() ?? string.Empty;
                    var selectedStudent = studentService.Students.FirstOrDefault(s => s.Id == int.Parse(selectedId));
                    if (selectedStudent != null)
                    {
                        Console.WriteLine("Enter the grade:");
                        var gradeint = Console.ReadLine() ?? string.Empty;
                        selectedStudent.Submissions[selectedAssignment] = double.Parse(gradeint);

                        CalcGPA(selectedStudent);
                    }
                }
            }
        }
        private Announcements CreateAnnouncement(Course c)
        {
            //Name
            Console.WriteLine("Name:");
            var name = Console.ReadLine() ?? string.Empty;
            //Author
            // TODO: Make author a Person and not a string
            Console.WriteLine("Author:");
            var author = Console.ReadLine() ?? string.Empty;
            //Description
            Console.WriteLine("Body:");
            var body = Console.ReadLine() ?? string.Empty;

            var announcement = new Announcements()
            {
                Name = name,
                Author = author,
                Body = body
            };

            return announcement; 
        }
        public void AddAnnouncement()
        {
            Console.WriteLine("Enter the code for the course to add the announcement to:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
            if (selectedCourse != null)
            {
                var announcement = CreateAnnouncement(selectedCourse);
                if (selectedCourse.Announcements == null)
                {
                    selectedCourse.Announcements = new List<Announcements>();
                }
                selectedCourse.Announcements.Add(announcement);
            }
        }
        public void RemoveAnnouncement()
        {
            Console.WriteLine("Enter the code for the course:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));

            if (selectedCourse != null)
            {
                Console.WriteLine("Choose an assignment to delete:");
                selectedCourse.Announcements.ForEach(Console.WriteLine);
                var selectionStr = Console.ReadLine() ?? string.Empty;
                var selectionInt = int.Parse(selectionStr);
                var selectedAssignment = selectedCourse.Assignments.FirstOrDefault(m => m.Id == selectionInt);
                if (selectedAssignment != null)
                {
                    selectedCourse.Assignments.Remove(selectedAssignment);
                }
            }
        }
        public void UpdateAnnouncement()
        {
            Console.WriteLine("Enter the code for the course:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
            
            if(selectedCourse != null && selectedCourse.Announcements.Any())
            {
                Console.WriteLine("Enter the name for the announcement to update:");
                selectedCourse.Announcements.ForEach(Console.WriteLine);

                selection = Console.ReadLine();
                var selectedAnnouncement = selectedCourse
                    .Announcements
                    .FirstOrDefault(a => a.Name.Equals(selection, StringComparison.InvariantCultureIgnoreCase));

                if(selectedAnnouncement != null)
                {
                    Console.WriteLine("Would you like to modify the announcement name?");
                    selection = Console.ReadLine();
                    if (selection?.Equals("Y", StringComparison.InvariantCultureIgnoreCase) ?? false)
                    {
                        Console.WriteLine("Name:");
                        selectedAnnouncement.Name = Console.ReadLine();
                    }
                    Console.WriteLine("Would you like to modify the announcement body?");
                    selection = Console.ReadLine();
                    if (selection?.Equals("Y", StringComparison.InvariantCultureIgnoreCase) ?? false)
                    {
                        Console.WriteLine("Description:");
                        selectedAnnouncement.Body = Console.ReadLine();
                    }
                    Console.WriteLine("Would you like to modify the announcement author?");
                    selection = Console.ReadLine();
                    if (selection?.Equals("Y", StringComparison.InvariantCultureIgnoreCase) ?? false)
                    {
                        Console.WriteLine("Author:");
                        selectedAnnouncement.Author = Console.ReadLine();
                    }
                }
            }
        }
        private void SetupRoster(Course c)
        {
            Console.WriteLine("Which students should be enrolled in this course? ('Q' to quit)");
            bool continueAdding = true;
            while (continueAdding)
            {
                studentService.Students.Where(s => !c.Roster.Any(s2 => s2.Id == s.Id)).ToList().ForEach(Console.WriteLine);
                var selection = "Q";
                if (studentService.Students.Any(s => !c.Roster.Any(s2 => s2.Id == s.Id)))
                {
                    selection = Console.ReadLine() ?? string.Empty;
                }

                if (selection.Equals("Q", StringComparison.InvariantCultureIgnoreCase))
                {
                    continueAdding = false;
                }
                else
                {
                    var selectedId = int.Parse(selection);
                    var selectedStudent = studentService.Students.FirstOrDefault(s => s.Id == selectedId);

                    if (selectedStudent != null)
                    {
                        c.Roster.Add(selectedStudent);
                    }
                }
            }
        }
        private void SetupAssignments(Course c)
        {
            Console.WriteLine("Would you like to add assignments? (Y/N)");
            var assignResponse = Console.ReadLine() ?? "N";
            bool continueAdding;
            if (assignResponse.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
            {
                continueAdding = true;
                while (continueAdding)
                {
                    c.Assignments.Add(CreateAssignment(c));
                    Console.WriteLine("Add more assignments? (Y/N)");
                    assignResponse = Console.ReadLine() ?? "N";
                    if (assignResponse.Equals("N", StringComparison.InvariantCultureIgnoreCase))
                    {
                        continueAdding = false;
                    }
                }
            }

        }
        private void SetupModules(Course c)
        {
            Console.WriteLine("Would you like to add modules? (Y/N)");
            var assignResponse = Console.ReadLine() ?? "N";
            bool continueAdding;
            if (assignResponse.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
            {
                continueAdding = true;
                while (continueAdding)
                {
                    c.Modules.Add(CreateModule(c));
                    Console.WriteLine("Add more modules? (Y/N)");
                    assignResponse = Console.ReadLine() ?? "N";
                    if (assignResponse.Equals("N", StringComparison.InvariantCultureIgnoreCase))
                    {
                        continueAdding = false;
                    }
                }
            }
        }
        private Module CreateModule(Course c)
        {
            //Name
            Console.WriteLine("Name:");
            var name = Console.ReadLine() ?? string.Empty;
            //Description
            Console.WriteLine("Description:");
            var description = Console.ReadLine() ?? string.Empty;

            var module = new Module
            {
                Name = name,
                Description = description
            };
            Console.WriteLine("Would you like to add content?");
            var choice = Console.ReadLine() ?? "N";
            while(choice.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("What type of content would you like to add?");
                Console.WriteLine("1. Assignment");
                Console.WriteLine("2. File");
                Console.WriteLine("3. Page");
                var contentChoice = int.Parse(Console.ReadLine() ?? "0");

                switch(contentChoice)
                {
                    case 1:
                        var newAssignmentContent = CreateAssignmentItem(c);
                        if(newAssignmentContent != null)
                        {
                            module.Content.Add(newAssignmentContent);
                        }
                        break;
                    case 2:
                        var newFileContent = CreateFileItem(c);
                        if (newFileContent != null)
                        {
                            module.Content.Add(newFileContent);
                        }
                        break;
                    case 3:
                        var newPageContent = CreatePageItem(c);
                        if (newPageContent != null)
                        {
                            module.Content.Add(newPageContent);
                        }
                        break;
                    default:
                        break;
                }

                Console.WriteLine("Would you like to add more content?");
                choice = Console.ReadLine() ?? "N";
            }

            return module; 
        }

        private AssignmentItem? CreateAssignmentItem(Course c)
        {
            //Name
            Console.WriteLine("Name:");
            var name = Console.ReadLine() ?? string.Empty;
            //Description
            Console.WriteLine("Description:");
            var description = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Which assignment should be added?");
            c.Assignments.ForEach(Console.WriteLine);
            var choice = int.Parse(Console.ReadLine() ?? "-1");
            if(choice >= 0)
            {
                var assignment = c.Assignments.FirstOrDefault(a => a.Id == choice);
                return new AssignmentItem
                {
                    Assignment = assignment,
                    Name = name,
                    Description = description
                };
            }
            return null;
        }

        private FileItem? CreateFileItem(Course c)
        {
            //Name
            Console.WriteLine("Name:");
            var name = Console.ReadLine() ?? string.Empty;
            //Description
            Console.WriteLine("Description:");
            var description = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Enter a path to the file:");
            var filepath = Console.ReadLine();

            return new FileItem { 
                Name = name,
                Description = description,
                Path = filepath
            };
        }

        private PageItem? CreatePageItem(Course c)
        {
            //Name
            Console.WriteLine("Name:");
            var name = Console.ReadLine() ?? string.Empty;
            //Description
            Console.WriteLine("Description:");
            var description = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Enter page content:");
            var body = Console.ReadLine();

            return new PageItem
            {
                Name = name,
                Description = description,
                HtmlBody = body
            };
        }
        private Assignment CreateAssignment(Course currentCourse)
        {
            //Name
            Console.WriteLine("Name:");
            var assignmentName = Console.ReadLine() ?? string.Empty;
            //Description
            Console.WriteLine("Description:");
            var assignmentDescription = Console.ReadLine() ?? string.Empty;
            //TotalPoints
            Console.WriteLine("TotalPoints:");
            var totalPoints = double.Parse(Console.ReadLine() ?? "100");
            //DueDate
            Console.WriteLine("DueDate:");
            var dueDate = DateTime.Parse(Console.ReadLine() ?? "01/01/1900");

            return new Assignment
            {
                ParentCourse = currentCourse,
                Name = assignmentName,
                Description = assignmentDescription,
                TotalAvailablePoints = totalPoints,
                DueDate = dueDate
            };
        }
    }
}
