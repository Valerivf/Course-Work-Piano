using Piano.Models;
using Piano.Models.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piano
{
    public class Game
    {
        // Общие переменные

        public static int NotesToShow = 5;

        public Sequence Sequence = null;

        public double Percents
        {
            get
            {
                return (double)Step / (Size - 1) * 100;
            }
        }

        // Игровые переменные

        private int Step = 0;

        private int Size = 0;

        public Game(Sequence sequence)
        {
            Sequence = sequence;
        }

        public Result Play(Note note)
        {
            // Если шаг был верным
            if (note.ID == Sequence.Notes[Step])
            {
                if (Step == (Size - 1))
                {
                    return new Result()
                    {
                        State = State.Win,
                        Notes = new List<int>(),
                    };
                }
                else
                {
                    Step++;

                    return new Result()
                    {
                        State = State.Continue,
                        Notes = Sequence.Notes.Skip(Step).Take(NotesToShow).ToList()
                    };
                }
            }
            else
            {
                return Start();
            }
        }

        public Result Start()
        {
            Step = 0;
            Size = Sequence.Notes.Count;

            return new Result()
            {
                State = State.Start,
                Notes = Sequence.Notes.Take(NotesToShow).ToList()
            };
        }
    }
}
