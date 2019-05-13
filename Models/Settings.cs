using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Piano.Models
{
    [Serializable]
    public class Settings
    {
        public string Title { get; set; } = "Фортепиано";
        public string GitHub { get; set; } = "https://github.com";
    }
}
