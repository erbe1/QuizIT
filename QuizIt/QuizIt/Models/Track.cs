using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizIt.Models
{
    public class Track
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }

        //public Question Question { get; set; }
        //public int QuestionId { get; set; }

        public List<Question> Questions { get; set; }
    }
}
