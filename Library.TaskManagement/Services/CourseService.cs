﻿using Library.TaskManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TaskManagement.Services
{
    public class CourseService
    {
        public List<Course> courseList = new List<Course>();

        public void Add(Course course)
        {
            courseList.Add(course);
        }
    }
}