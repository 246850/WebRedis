using System;
using System.Collections.Generic;
using System.Text;

namespace WebRedis.Models
{
    public class DatabaseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int KeyCount { get; set; }
        public long Size { get; set; }
        public string IconClass { get; set; }
        public string Href { get; set; }
        public List<DatabaseModel> Children { get; set; }
    }
}
