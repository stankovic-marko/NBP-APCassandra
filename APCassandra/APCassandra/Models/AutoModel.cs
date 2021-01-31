using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APCassandra.Models
{
    public class AutoModel
    {
        public Guid Id { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
        public string Contact { get; set; }
        public string EquipmentList { get; set; }
        public string Fuel { get; set; }
        public List<string> ImagesList { get; set; }
        public string Model { get; set; }
        public int Power { get; set; }
        public int Price { get; set; }
        public string ShowImage { get; set; }
        public string Type { get; set; }
        public string UserId { get; set; }
        public int Volume { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
    }
}
