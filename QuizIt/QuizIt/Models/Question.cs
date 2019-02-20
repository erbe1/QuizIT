using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizIt.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string TrackQuestion { get; set; }
        public string Answer { get; set; }

        public Track Track { get; set; }
        public int TrackId { get; set; } //Ev behöva ändra till string, ändra i databasen i såna fall, trackid ska heller inte autogenereras
    }
}
