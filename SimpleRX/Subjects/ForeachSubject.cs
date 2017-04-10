using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class ForeachSubject<T> : BaseSubject<T>
    {
        private IObservable<T> source;
        private Action<T> action;
        private IDisposable subscriped;
        private BaseSubject<T> subject;

        public ForeachSubject(IObservable<T> source, Action<T> action)
        {
            this.source = source;
            this.action = action;
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
            action(value);
            NotifyObservers(value);
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
