using Piano.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piano.Models
{
    public class Note
    {
        public int ID;
        public string Name;
        public Octave Octave;
        public Image Image;
        public Stream Sound;
        public Stream SoundDrum;
        public Note() { }

        public Note(int id, string name, Octave octave, Image image, Stream sound, Stream soundDrum)
        {
            ID = id;
            Name = name;
            Octave = octave;
            Image = image;
            Sound = sound;
            SoundDrum = soundDrum;
        }
    }
}
