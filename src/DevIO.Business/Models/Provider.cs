
using System.Collections.Generic;

namespace DevIO.Business.Models
{
    public class Provider : Entity
    {
        public string Name { get; set; }
        public string Document { get; set; }
        public bool Actve {get; set;}
        public TypeProvider TypeProvider { get; set; }
        
        /* EF Relation */
        public IEnumerable<Product> Products { get; set; }
        public Address Address { get; set; }
    }
}
