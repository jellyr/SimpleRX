using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class DistinctUntilChangedSubject<T> : BaseSubject<T>
    {
        private IDisposable subscriped;
        private BaseSubject<T> subject;
        private T lastValue = default(T);

        public DistinctUntilChangedSubject(IObservable<T> source)
        {
            BaseSubject<T> subject = (BaseSubject<T>)source;
            this.subject = subject;
            var observer =
                new Observer<T>(
                    value => InnerExecute(value),
                    ex => NotifyErrorObservers(ex),
                    () => NotifyCompleteObservers() );
            this.subscriped = subject.ColdSubscribe(observer);
        }

        public void InnerExecute(T value)
        {
            if (!lastValue.Equals(value))
            {
                NotifyObservers(value);
            }
            lastValue = value;
        }

        public override void Execute()
        {
            try
            {
                subject.Execute();
            }
            catch (Exception exception)
            {
                NotifyErrorObservers(exception);
            }          
        }
    }
}
