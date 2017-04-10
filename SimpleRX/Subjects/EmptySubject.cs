using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class EmptySubject<T> : BaseSubject<T>
    {
        public EmptySubject()
        {
        }

        public override void Execute()
        {
            foreach (var item in observers)
            {
                item.OnNext(default(T));
                item.OnCompleted();
            }
        }
    }
}
