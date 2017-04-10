using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SimpleRX
{
    public class ThreadPoolScheduler : IScheduler
    {
        public void Schedule(Action action)
        {
            Action<object> objectAction = (o) => { action(); };
            ThreadPool.QueueUserWorkItem(new WaitCallback(objectAction));
        }

        public void Schedule<T>(Action<T> action, T value)
        {
            Action<object> objectAction = (o) => { action(value); };
            ThreadPool.QueueUserWorkItem(new WaitCallback(objectAction), value);
        }

        public void Schedule(Action action, CancellationToken cancellationToken)
        {
            Action<object> objectAction = (o) => { action(); };
            ThreadPool.QueueUserWorkItem(new WaitCallback(objectAction));
        }

        public void Schedule<T>(Action<T> action, T value, CancellationToken cancellationToken)
        {
            Action<object> objectAction = (o) => { action(value); };
            ThreadPool.QueueUserWorkItem(new WaitCallback(objectAction), value);
        }
    }
}
