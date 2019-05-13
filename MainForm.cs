using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using Piano.Models;
using Piano.Properties;
using System.Diagnostics;
using Piano.Models.Game;
using System.IO;
using System.Linq;

namespace Piano
{
    public partial class MainForm : Form
    {
        public enum WorkingMode
        {
            Sound = 0,
            Information = 1
        }
        public WorkingMode Mode;

        // Объект для хранения настроек (хранение названия формы и ссылка на гитхаб)
        private Models.Settings settings = null;
        // Путь к файлу настроек
        private string settingsPath = "settings.xml";

        // Объект для текущей игры
        private Game sequenceGame = null;

        private List<Sequence> Sequences = new List<Sequence>();

        public MainForm()
        {
            InitializeComponent();
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;

            // Загрузка настроек из файла (тип - моделс.сеттингс, путь)
            settings = Loader.Load<Models.Settings>(settingsPath);

            // Устанавливаем название формы
            Text = settings.Title;

            // Получаем все файлы в текущей директории с расширением
            string[] gameFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.gamexml");
            foreach (string file in gameFiles)
            {
                Sequence sequence = Loader.Load<Sequence>(file);
                Sequences.Add(sequence);
            }

            // Заполняем комбобокс именами писюнек!!!!
            comboBoxSequences.Items.AddRange(Sequences.Select(x => x.Name).ToArray());
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Loader.Save(settingsPath, settings);
        }

        // Коллекция нот (ключ - строка, значение - нота)
        public Dictionary<string, Note> Notes = new Dictionary<string, Note>()
        {
            { "btn1", new Note(1, "До", Octave.First, Resources.до, Resources.C, Resources.C_Drum) },
            { "btn2", new Note(2, "Ре", Octave.First, Resources.ре, Resources.D, Resources.D_Drum) },
            { "btn3", new Note(3, "Ми", Octave.First, Resources.ми, Resources.E, Resources.E_Drum) },
            { "btn4", new Note(4, "Фа", Octave.First, Resources.фа, Resources.F, Resources.F_Drum) },
            { "btn5", new Note(5, "Соль", Octave.First, Resources.соль, Resources.G, Resources.G_Drum) },
            { "btn6", new Note(6, "Ля", Octave.First, Resources.ля, Resources.A, Resources.A_Drum) },
            { "btn7", new Note(7, "Си", Octave.First, Resources.си, Resources.B, Resources.B_Drum) },
            { "btn8", new Note(8, "До", Octave.Second, Resources.до1, Resources.C1, Resources.C1_Drum) },
            { "btn9", new Note(9, "Ре", Octave.Second, Resources.ре1, Resources.D1, Resources.D1_Drum) },
            { "btn10", new Note(10, "Ми", Octave.Second, Resources.ми1, Resources.E1, Resources.E1_Drum) },
            { "btn11", new Note(11, "Фа", Octave.Second, Resources.фа1, Resources.F1, Resources.F1_Drum) },
            { "btn12", new Note(12, "До диез", Octave.First, Resources.до2, Resources.C_s, Resources.Cq_Drum) },
            { "btn13", new Note(13, "Ми бемоль", Octave.First, Resources.ми2, Resources.D_s, Resources.Dq_Drum) },
            { "btn14", new Note(14, "Фа диез", Octave.First, Resources.фа2, Resources.F_s, Resources.Fq_Drum) },
            { "btn15", new Note(15, "Соль диез/Ля бемоль", Octave.First, Resources.соль2, Resources.G_s, Resources.Gq_Drum) },
            { "btn16", new Note(16, "Ля диез/Си бемоль", Octave.First, Resources.си2, Resources.Bb, Resources.Bb_Drum) },
            { "btn17", new Note(17, "До диез", Octave.Second, Resources._2, Resources.C_s1, Resources.Cq1_Drum) },
            { "btn18", new Note(18, "Ми бемоль", Octave.Second, Resources._3, Resources.D_s1, Resources.Dq1_Drum) }
        };

        private void buttonClick(object sender, EventArgs e)
        {
            try
            {
                // Вытаскиваем из sender объект кнопки
                //все кнопки приходят в этот метод, единственное что отличает их - sender
                Button button = sender as Button;

                Note note = Notes[button.Name];

                // Проверяем режим работы
                if (Mode == WorkingMode.Sound)
                {
                    // Создаём ту штуку, которая воспроизводит звуки
                    using (SoundPlayer player = new SoundPlayer())
                    {
                        // Если фортепиано
                        if (radioButton1.Checked)
                        {
                            // Возвращаем позицию считывания. "Головка в последовательности"
                            note.Sound.Position = 0;
                            player.Stream = note.Sound;
                        }
                        else if (radioButton2.Checked)
                        {
                            note.SoundDrum.Position = 0;
                            player.Stream = note.SoundDrum;
                        }
                        player.Play();
                    }

                    // Если игра не пуста
                    if (sequenceGame != null)
                    {
                        // Передаём в игру текущую ноту (которую мы нажали) и получаем в ответ результат игры
                        Result gameResult = sequenceGame.Play(note);

                        // Обрабатываем результат игры
                        processGameResult(gameResult);
                    }
                }
                // Если режим вывода информации
                else
                {
                    OpenInformationForm(note);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка во время выполнения программы", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenInformationForm(Note note)
        {
            InformationalForm informationWindow = new InformationalForm(note);
            informationWindow.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gitHubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Uri link;
            // Пытаемся сделать ссылку из "GitHub" настроек
            bool isValid = Uri.TryCreate(settings.GitHub, UriKind.Absolute, out link);

            // Если ссылка валидна
            if (isValid)
            {
                // Стартуем процесс с ссылкой -> откроется браузер
                Process.Start(link.ToString());
            }
            else
            {
                MessageBox.Show("Link isn't valid!");
            }
        }

        // Изменение режима работы по чек боксу
        private void informationCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (informationCheckBox.Checked) Mode = WorkingMode.Information;
            else Mode = WorkingMode.Sound;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            Sequence sequence = Sequences.Find(x => x.Name == comboBoxSequences.SelectedItem.ToString());
            if (sequence == null)
            {
                MessageBox.Show("error");
            }
            else
            {
                sequenceGame = new Game(sequence);
                processGameResult(sequenceGame.Start());
            }
        }

        private void processGameResult(Result gameResult)
        {
            labelName.Text = sequenceGame.Sequence.Name;
            progressBarSequence.Value = Convert.ToInt32(sequenceGame.Percents);

            switch (gameResult.State)
            {
                case State.Start:
                    textBoxSequence.Text = String.Join(",", gameResult.Notes);
                    checkBox1.Checked = false;
                    break;

                case State.Continue:
                    textBoxSequence.Text = String.Join(",", gameResult.Notes);
                    break;

                case State.Win:
                    textBoxSequence.Text = "Win!";
                    checkBox1.Checked = true;
                    break;
            }
        }
    }
}
