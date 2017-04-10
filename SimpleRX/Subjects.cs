using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public static class Subjects
    {
        public static IObservable<int> Min(this Subject<int> source)
        {
            return new MinSubjectSubjectInt<int>(source);
        }
    }
}
