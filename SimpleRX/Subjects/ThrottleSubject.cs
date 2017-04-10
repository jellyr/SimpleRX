using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class ThrottleSubject<T> : BaseSubject<T>
    {
        private IDisposable subscriped;
        private BaseSubject<T> subject;
        private TimeSpan dueTime;
        private Stopwatch stopWatch;

        public ThrottleSubject(IObservable<T> source, TimeSpan dueTime)
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
            if (stopWatch.Elapsed > dueTime)
            {
                stopWatch.Stop();
                NotifyObservers(value);   
            }
        }

        public override void Execute()
        {
            try
            {
                stopWatch = new Stopwatch();
                stopWatch.Start();
                subject.Execute();
            }
            catch (Exception exception)
            {
                NotifyErrorObservers(exception);
            }      
        }
    }
}
