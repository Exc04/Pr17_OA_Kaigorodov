using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pr17_Kaigorodov
{
    public class Country
    {
        
        public string Name { get; set; }

       
        public long Population { get; set; }

        
        public override string ToString()
        {
            return $"{Name} {Population:N0}";
        }
    }
}
