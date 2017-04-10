using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class StartWithSubject<T> : BaseSubject<T>    
    {
        private IObservable<T> source;
        private IDisposable subscriped;
        private BaseSubject<T> subject;
        private T[] values;

        public StartWithSubject(IObservable<T> source, T[] values)
        {
            this.source = source;
            this.values = values;
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
            NotifyObservers(value);
        }

        private void ValuesExecution()
        {
            foreach (var value in values)
            {
                NotifyObservers(value);
            }
        }

        public override void Execute()
        {
            try
            {
                ValuesExecution();
                subject.Execute();
            }
            catch (Exception exception)
            {
                NotifyErrorObservers(exception);
            }      
        }
    }
}
