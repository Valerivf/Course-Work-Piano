using Piano.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Piano
{
    public partial class InformationalForm : Form
    {
        private Note Data;

        private Dictionary<Octave, String> Octaves = new Dictionary<Octave, string>()
        {
            { Octave.First, "Первая октава" },
            { Octave.Second, "Вторая октава" }
        };

        public InformationalForm(Note data)
        {
            InitializeComponent();
            Data = data;
        }

        private void Inform_Load(object sender, EventArgs e)
        {
            lblText.Text = Data.Name;
            // Получаем имя октавы
            lblOctave.Text = Octaves[Data.Octave];
            img1.Image = Data.Image;
        }
        
    }
}
