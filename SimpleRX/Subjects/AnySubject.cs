using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class AnySubject<T> : BaseSubject<bool>
    {
        private IObservable<T> source;
        private IDisposable subscriped;
        private BaseSubject<T> subject;
        private bool passed;

        public AnySubject(IObservable<T> source)
        {
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

        private void InnerExecute(T value)
        {            
            passed = true;            
        }

        public override void Execute()
        {
            try
            {
                subject.Execute();
                NotifyObservers(passed);
            }
            catch (Exception exception)
            {
                NotifyErrorObservers(exception);
            }            
        }
    }
}
