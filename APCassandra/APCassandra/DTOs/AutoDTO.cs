using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APCassandra.DTOs
{
    public class AutoDTO
    {
        public string Brand { get; set; }

        public string Model { get; set; }

        public string Type { get; set; }

        public string Fuel { get; set; }

        public Guid Id { get; set; }

        public string ShowImage { get; set; }

        public int Power { get; set; }

        public int Price { get; set; }

        public int Year { get; set; }

    }
}
