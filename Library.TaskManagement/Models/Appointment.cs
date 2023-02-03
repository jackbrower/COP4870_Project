﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Library.TaskManagement.Models
{
    public class Appointment : Item
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public override string Display => $"{base.Display}\n{Start} - {End}";

        public Appointment() {

        }

        public Appointment(Item i) {
            Name = i.Name;
            Description = i.Description;
        }
    }
}