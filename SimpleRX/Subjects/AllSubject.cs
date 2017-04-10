using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class AllSubject<T> : BaseSubject<bool>
    {
        private IObservable<T> source;
        private IDisposable subscriped;
        private BaseSubject<T> subject;
        private Func<T, bool> predicate;
        private bool passed = true;

        public AllSubject(IObservable<T> source, Func<T, bool> predicate)
        {
            this.source = source;
            this.predicate = predicate;
            BaseSubject<T> subject = (BaseSubject<T>)source;
            this.subject = subject;
            var observer =
                new Observer<T>(
                    value => InnerExecute(value),
                    ex => NotifyErrorObservers(ex),
                    () => { });
            this.subscriped = subject.ColdSubscribe(observer);
        }

        private void InnerExecute(T value)
        {
            if (!predicate(value))
            {
                passed = false;
            }
        }

        public override void Execute()
        {
            try
            {
                subject.Execute();
                NotifyObservers(passed);
                NotifyCompleteObservers();
            }
            catch (Exception exception)
            {
                NotifyErrorObservers(exception);
            }            
        }
    }
}
