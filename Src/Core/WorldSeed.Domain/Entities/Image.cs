using System;
using System.Collections.Generic;
using System.Text;

namespace WorldSeed.Domain.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
