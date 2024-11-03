﻿using Base.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class AssignmentUser : EntityObject
    {
        public int AssignmentId { get; set; }
        public Assignments Assignment { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}