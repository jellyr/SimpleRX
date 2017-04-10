using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class ZipSubject<TFirst, TSecond, TResult> : BaseSubject<TResult>
    {
        private IObservable<TFirst> first;
        private IEnumerable<TSecond> second;
        private Func<TFirst, TSecond, TResult> resultSelector;
        private IDisposable subscriped;
        private BaseSubject<TFirst> subject;

        public ZipSubject(IObservable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            this.first = first;
            this.second = second;
            this.resultSelector = resultSelector;
            BaseSubject<TFirst> subject = (BaseSubject<TFirst>)first;
            this.subject = subject;
            var observer =
                new Observer<TFirst>(
                    value => InnerExecute(value),
                    ex => NotifyErrorObservers(ex),
                    () => NotifyCompleteObservers() );
            this.subscriped = subject.ColdSubscribe(observer);
        }

        private void InnerExecute(TFirst value)
        {
            foreach (var secondItem in second)
            {
                TResult result = resultSelector(value, secondItem);
                NotifyObservers(result);
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
