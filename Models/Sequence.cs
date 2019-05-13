using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piano.Models
{
    [Serializable]
    public class Sequence
    {
        public string Name;
        public List<int> Notes = new List<int>();
    }
}
