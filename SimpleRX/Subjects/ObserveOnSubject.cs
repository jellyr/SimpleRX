using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class ObserveOnSubject<T> : BaseSubject<T>
    {
        private IObservable<T> source;        
        private BaseSubject<T> subject;
        private IDisposable subscriped;

        private ObserveOnSubject(IObservable<T> source)
        {
            base.Scheduler = new CurrentThreadScheduler();
            this.source = source;
            BaseSubject<T> subject = (BaseSubject<T>)source;
            this.subject = subject;
            var observer =
                new Observer<T>(
                    value => InnerExecute(value),
                    ex => NotifyErrorObservers(ex),
                    () => NotifyCompleteObservers() );
            this.subscriped = subject.ColdSubscribe(observer);
        }

        public ObserveOnSubject(IObservable<T> source, IScheduler scheduler) : this(source)
        {
            base.Scheduler = scheduler;
        }

        private void InnerExecute(T value)
        {
            NotifyObservers(value);            
        }

        public override void Execute()
        {
            Scheduler.Schedule(() => 
            {
                try
                {
                    subject.Execute();
                }
                catch (Exception exception)
                {
                    NotifyErrorObservers(exception);
                }      
            });
        }
    }
}
