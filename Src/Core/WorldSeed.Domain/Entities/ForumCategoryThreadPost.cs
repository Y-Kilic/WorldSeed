﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldSeed.Domain.Entities
{
    public class ForumCategoryThreadPost
    {
        public int Id { get; set; }
        public ForumCategoryThread ForumCategoryThread { get; set; }
    }
}
