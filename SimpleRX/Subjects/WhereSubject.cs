using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class WhereSubject<T> : BaseSubject<T>
    {
        private IObservable<T> source;
        private Predicate<T> predicate;
        private IDisposable subscriped;
        private BaseSubject<T> subject;

        public WhereSubject(IObservable<T> source, Predicate<T> predicate)
        {
            this.source = source;
            this.predicate = predicate;
            BaseSubject<T> subject = (BaseSubject<T>)source;
            this.subject = subject;
            var observer =
                new Observer<T>(
                    value => InnerExecute(value),
                    ex => NotifyErrorObservers(ex),
                    () => NotifyCompleteObservers());
            this.subscriped = subject.ColdSubscribe(observer);
        }

        private void InnerExecute(T value)
        {
            if (predicate(value))
            {
                NotifyObservers(value);
            }
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
