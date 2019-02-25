using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizIt.Models.ViewModels
{
    public class QuizQuestionsVm
    {
        public Quiz Quiz { get; set; }
        public Question Question { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
        public List<Quiz> Quizzez { get; set; } = new List<Quiz>();
    }
}
