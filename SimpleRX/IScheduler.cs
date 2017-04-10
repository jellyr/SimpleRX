using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SimpleRX
{
    public interface IScheduler
    {
        //DateTimeOffset Now { get; }
        void Schedule(Action action);
        void Schedule(Action action, CancellationToken cancellationToken);
        void Schedule<T>(Action<T> action, T value);
        void Schedule<T>(Action<T> action, T value, CancellationToken cancellationToken);
        //IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action);
        //IDisposable Schedule<TState>(TState state, DateTime dueTime, Func<IScheduler, TState, IDisposable> action);
    }
}
