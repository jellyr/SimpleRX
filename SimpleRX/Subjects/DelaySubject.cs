using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace SimpleRX
{
    public class DelaySubject<T> : BaseSubject<T>
    {
        private IDisposable subscriped;
        private BaseSubject<T> subject;
        private TimeSpan dueTime;

        public DelaySubject(IObservable<T> source, TimeSpan dueTime)
        {
            BaseSubject<T> subject = (BaseSubject<T>)source;
            this.dueTime = dueTime;
            this.subject = subject;
            var observer =
                new Observer<T>(
                    value => InnerExecute(value),
                    ex => NotifyErrorObservers(ex),
                    () => NotifyCompleteObservers() );
            this.subscriped = subject.ColdSubscribe(observer);
        }

        private void InnerExecute(T value)
        {
            NotifyObservers(value);                 
        }

        public override void Execute()
        {
            Timer timer = new Timer(_ =>
            {
                try
                {
                    subject.Execute();
                }
                catch (Exception exception)
                {
                    NotifyErrorObservers(exception);
                }          
            }, null, dueTime, new TimeSpan(-1));
        }
    }
}
