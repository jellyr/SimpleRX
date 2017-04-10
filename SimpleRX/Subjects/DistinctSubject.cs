using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class DistinctSubject<T> : BaseSubject<T>
    {
        private IDisposable subscriped;
        private BaseSubject<T> subject;
        private List<T> values = new List<T>();

        public DistinctSubject(IObservable<T> source)
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
            if (!values.Contains(value))
            {
                NotifyObservers(value);
                values.Add(value);
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
