using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class CastSubject<T> : BaseSubject<T>
    {
        private IObservable<object> source;
        private IDisposable subscriped;
        private BaseSubject<object> subject;        

        public CastSubject(IObservable<object> source)
        {
            this.source = source;
            BaseSubject<object> subject = (BaseSubject<object>)source;
            this.subject = subject;
            var observer =
                new Observer<object>(
                    value => InnerExecute(value),
                    ex => NotifyErrorObservers(ex),
                    () => NotifyCompleteObservers() );
            this.subscriped = subject.ColdSubscribe(observer);
        }

        private void InnerExecute(object value)
        {
            T castValue = (T)value;
            NotifyObservers(castValue);            
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
