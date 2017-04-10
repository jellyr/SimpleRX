using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SimpleRX
{
    public class CurrentThreadScheduler : IScheduler
    {
        public void Schedule(Action action)
        {
            action();
        }

        public void Schedule<T>(Action<T> action, T value)
        {
            action(value);
        }

        public void Schedule(Action action, CancellationToken cancellationToken)
        {
            action();
        }

        public void Schedule<T>(Action<T> action, T value, CancellationToken cancellationToken)
        {
            action(value);
        }
    }
}
