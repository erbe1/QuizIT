using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        public string TrackTitle { get; set; }
        public string TrackId { get; set; }

        public List<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();

        [NotMapped]
        public List<string> Suggestions { get; set; } = new List<string>();

    }
}
