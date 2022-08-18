﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldSeed.Domain.Entities.UserRelated;

namespace WorldSeed.Domain.Entities.CharacterRelated
{
    public class Character
    {
        public int Id { get; set; }
        public User User { get; set; }
        public String Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
