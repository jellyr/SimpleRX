using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class FinallySubject<T> : BaseSubject<T>
    {
        private IObservable<T> source;
        private IDisposable subscriped;
        private BaseSubject<T> subject;
        private Action action;

        public FinallySubject(IObservable<T> source, Action action)
        {
            this.source = source;
            this.action = action;
            BaseSubject<T> subject = (BaseSubject<T>)source;
            this.subject = subject;
            var observer =
                new Observer<T>(
                    value => InnerExecute(value),
                    ex => InnerError(ex),
                    () => InnerComplete());
            this.subscriped = subject.ColdSubscribe(observer);
        }

        private void InnerExecute(T value)
        {
            NotifyObservers(value);
        }

        private void InnerComplete()
        {            
            NotifyCompleteObservers();
            action();
        }

        private void InnerError(Exception exception)
        {            
            NotifyErrorObservers(exception);
            action();
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
