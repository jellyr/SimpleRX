using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleRX
{
    public class TaskScheduler : IScheduler
    {
        private CancellationToken cancellationToken;

        public TaskScheduler()
        {
            
        }

        public TaskScheduler(CancellationToken cancellationToken)
        {
            this.cancellationToken = cancellationToken;
        }

        public void Schedule(Action action)
        {
            Task.Factory.StartNew(action);
        }

        public void Schedule(Action action, CancellationToken cancellationToken)
        {
            Task.Factory.StartNew(action, cancellationToken);
        }

        public void Schedule<T>(Action<T> action, T value)
        {
            Action<object> actionObject = (object o) => { action(value); };
            Task.Factory.StartNew(actionObject, (object)value);
        }

        public void Schedule<T>(Action<T> action, T value, CancellationToken cancellationToken)
        {
            Action<object> actionObject = (object o) => { action(value); };
            Task.Factory.StartNew(actionObject, (object)value, cancellationToken);
        }

        //public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
        //{
        //    throw new NotImplementedException();
        //}

        //public IDisposable Schedule<TState>(TState state, DateTime dueTime, Func<IScheduler, TState, IDisposable> action)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
