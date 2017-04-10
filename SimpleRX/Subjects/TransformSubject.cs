using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SimpleRX
{
    public class TransformSubject<T> : BaseSubject<T>
    {
        private IObservable<T> source;
        private IDisposable subscriped;
        private BaseSubject<T> subject;
        private Func<T, T> transform;
        private T value;
        private bool valueIsSet;
        private ThreadExecuter<T> threadExecuter;

        public TransformSubject(IObservable<T> source, Func<T, T> transform)
        {
            this.Scheduler = new CurrentThreadScheduler();
            this.source = source;
            this.transform = transform;
            BaseSubject<T> subject = (BaseSubject<T>)source;
            this.subject = subject;
            var observer =
                new Observer<T>(
                    value => InnerExecute(value),
                    ex => NotifyErrorObservers(ex),
                    () => { OnCompleted();  });
            this.subscriped = subject.ColdSubscribe(observer);
        }

        public TransformSubject(IObservable<T> source, Func<T, T> transform, IScheduler scheduler) :
            this(source, transform)
        {
            this.Scheduler = scheduler;
        }

        public class ThreadExecuter<T>
        {
            public T Value { get; set; }
            public IObservable<T> Source { get; set; }
            public Func<T, T> Transform { get; set; }
            public List<IObserver<T>> Observers { get; set; }
            public IScheduler Scheduler { get; set; }
            private bool valueIsSet;

            public AutoResetEvent autoResetEvent = new AutoResetEvent(false);

            public void StartExecute()
            {
                
            }

            public void Execute(T value)
            {
                this.Value = Transform(value);
            }

            public void OnCompleted()
            {
                autoResetEvent.Set();
            }
        }

        private void OnCompleted()
        {
            threadExecuter.OnCompleted();
        }

        private void InnerExecute(T value)
        {
            threadExecuter.Execute(value);
        }

        public override void Execute()
        {
            try
            {
                threadExecuter =
                    new ThreadExecuter<T>()
                    {
                        Value = this.value,
                        Source = this.source,
                        Transform = this.transform,
                        Observers = observers,
                        Scheduler = this.Scheduler
                    };
                Scheduler.Schedule(threadExecuter.StartExecute);
                subject.Execute();
                WaitHandle.WaitAll(new WaitHandle[] { threadExecuter.autoResetEvent });
                NotifyObservers(threadExecuter.Value);
            }
            catch (Exception exception)
            {
                NotifyErrorObservers(exception);
            }
        }
    }
}
