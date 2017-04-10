using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class AverageSubject<T> : BaseSubject<double>
    {
        private IObservable<int> source;
        private IDisposable subscriped;
        private BaseSubject<int> subject;
        private double value;
        private int count;

        public AverageSubject(IObservable<int> source)
        {
            this.source = source;
            BaseSubject<int> subject = (BaseSubject<int>)source;
            this.subject = subject;
            var observer =
                new Observer<int>(
                    value => InnerExecute(value),
                    ex => NotifyErrorObservers(ex),
                    () => { } );
            this.subscriped = subject.ColdSubscribe(observer);
        }

        private void InnerExecute(int value)
        {
            this.value += value;
            this.count++;
        }

        public override void Execute()
        {            
            try
            {
                subject.Execute();
                double average = this.value / count;
                NotifyObservers(average);
                NotifyCompleteObservers();
            }
            catch (Exception exception)
            {
                NotifyErrorObservers(exception);
            }         
        }
    }
}
