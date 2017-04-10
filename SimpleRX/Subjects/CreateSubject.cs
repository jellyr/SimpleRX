using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class CreateSubject<T> : BaseSubject<T>
    {
        private Func<IObserver<T>, Action> subscribe;

        public CreateSubject(Func<IObserver<T>, Action> subscribe)
        {
            this.subscribe = subscribe;            
            this.Scheduler = new CurrentThreadScheduler();
        }

        public CreateSubject(Func<IObserver<T>, Action> subscribe, IScheduler scheduler) :
            this(subscribe)
        {
            this.Scheduler = scheduler;
        }

        public class ThreadExecuter
        {
            public Func<IObserver<T>, Action> subscribe;
            public List<IObserver<T>> Observers { get; set; }
            public IScheduler Scheduler { get; set; }
            public Action DisposeAction { get; set; }

            public void Execute()
            {
                IObserver<T> observer = new Observer<T>(o =>
                {
                    foreach (var item in Observers)
                    {
                        item.OnNext(o);
                    }
                }, e =>
                {
                    foreach (var item in Observers)
                    {
                        item.OnError(e);
                    }
                }, () => 
                {
                    foreach (var item in Observers)
                    {
                        item.OnCompleted();
                    }
                });
                Action action = subscribe(observer);
                DisposeAction = action;                  
            }
        }

        public override void Execute()
        {
            ThreadExecuter threadExecuter =
                new ThreadExecuter()
                {
                    subscribe = this.subscribe,
                    Observers = observers,                    
                    Scheduler = this.Scheduler
                };
            Scheduler.Schedule(threadExecuter.Execute);            
        }
    }
}
