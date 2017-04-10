using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SimpleRX
{
    public class ThreadScheduler : IScheduler
    {
        public void Schedule(Action action)
        {
            Thread thread = new Thread(new ThreadStart(action));
            thread.Start();
        }

        public void Schedule<T>(Action<T> action, T value)
        {
            Action<object> actionObject = (object o) => { action(value); };
            Thread thread = new Thread(new ParameterizedThreadStart(actionObject));
            thread.Start(value);
        }

        public void Schedule(Action action, CancellationToken cancellationToken)
        {
            Thread thread = new Thread(new ThreadStart(action));
            thread.Start();
        }

        public void Schedule<T>(Action<T> action, T value, CancellationToken cancellationToken)
        {
            Action<object> actionObject = (object o) => { action(value); };
            Thread thread = new Thread(new ParameterizedThreadStart(actionObject));
            thread.Start(value);
        }
    }
}
