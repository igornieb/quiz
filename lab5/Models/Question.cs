using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab5.Models
{
    public class Question
    {
        public long Id { get; set; }
        public long IdQuiz { get; set; }
        public string Quest { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string Answear { get; set; }

        public override string ToString()
        {
            return $"{Quest}";
        }
    }
}
