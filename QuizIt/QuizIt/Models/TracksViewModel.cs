using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizIt.Models
{
    public class TracksViewModel
    {
        public IEnumerable<Track> Tracks { get; set; }
        public Track Track { get; set; }
    }
}
