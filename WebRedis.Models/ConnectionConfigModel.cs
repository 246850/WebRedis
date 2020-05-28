using System;
using System.Collections.Generic;
using System.Text;

namespace WebRedis.Models
{
    public class ConnectionConfigModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
