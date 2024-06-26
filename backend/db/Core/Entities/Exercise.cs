﻿using Base.Core.Entities;
using Core.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Exercise : EntityObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Project { get; set; }
        public Language Language { get; set; }
        public Year Year { get; set; }
        public Subject Subject { get; set; }
    }
}
