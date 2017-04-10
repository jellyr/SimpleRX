using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class SelectSubject<T, TResult> : BaseSubject<TResult>
    {
        private IObservable<T> source;
        private Func<T, TResult> selector;
        private IDisposable subscriped;
        private BaseSubject<T> subject;

        public SelectSubject(IObservable<T> source, Func<T, TResult> selector)
        {
            this.source = source;
            this.selector = selector;            
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
            TResult selectorValue = selector(value);
            NotifyObservers(selectorValue);            
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
