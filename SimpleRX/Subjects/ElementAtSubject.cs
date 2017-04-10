using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class ElementAtSubject<T> : BaseSubject<T>
    {
        private IObservable<T> source;
        private int index;
        private int startIndex = 0;
        private IDisposable subscriped;
        private BaseSubject<T> subject;

        public ElementAtSubject(IObservable<T> source, int index)
        {
            this.source = source;
            this.index = index;
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
            if (startIndex == index)
            {
                NotifyObservers(value);
            }
            startIndex++;
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
