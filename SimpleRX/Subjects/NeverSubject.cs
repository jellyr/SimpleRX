using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class NeverSubject<T> : BaseSubject<T>
    {
        public NeverSubject()
        {

        }

        public override void Execute()
        {

        }
    }
}
