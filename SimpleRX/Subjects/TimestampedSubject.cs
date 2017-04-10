using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class TimestampedSubject<T> : BaseSubject<Timestamped<T>>
    {
        public Timestamped<T> Timestamped_ { get; private set; }
        private IDisposable subscriped;
        private BaseSubject<T> subject;

        public TimestampedSubject(IObservable<T> source)
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

        private void InnerExecute(T value)
        {
            Timestamped_ = new Timestamped<T>() 
            { 
                Timestamp = DateTimeOffset.Now,
                Value = value 
            };
            NotifyObservers(Timestamped_);            
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
