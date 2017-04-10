using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SimpleRX
{
    public class FromAsyncSubject<T> : BaseSubject<T>
    {
        private Func<CancellationToken, T> func;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public FromAsyncSubject(Func<CancellationToken, T> func)
        {                        
            this.func = func;
        }

        public override void Execute()
        {
            func(cancellationTokenSource.Token);
        }
    }
}
