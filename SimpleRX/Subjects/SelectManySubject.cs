using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class SelectManySubject<T, TResult> : BaseSubject<TResult>
    {
        private IObservable<T> source;
        private Func<T, IEnumerable<TResult>> selector;
        private IDisposable subscriped;
        private BaseSubject<T> subject;

        public SelectManySubject(IObservable<T> source, Func<T, IEnumerable<TResult>> selector)
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
            IEnumerable<TResult> selectorValue = selector(value);
            foreach (var item in selectorValue)
            {
                NotifyObservers(item);
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
