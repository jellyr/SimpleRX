using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class ScanSubject<T> : BaseSubject<T>
    {
        private IObservable<T> source;
        private Func<T, T, T> accumulator;
        private IDisposable subscriped;
        private BaseSubject<T> subject;
        private T accumulatorValue = default(T);

        public ScanSubject(IObservable<T> source, Func<T, T, T> accumulator)
        {
            this.source = source;
            this.accumulator = accumulator;
            BaseSubject<T> subject = (BaseSubject<T>)source;
            this.subject = subject;
            var observer =
                new Observer<T>(
                    value => InnerExecute(value),
                    ex => NotifyErrorObservers(ex),
                    () => NotifyCompleteObservers());
            this.subscriped = subject.ColdSubscribe(observer);
        }

        public ScanSubject(IObservable<T> source, T seed, Func<T, T, T> accumulator) : this(source, accumulator)
        {
            accumulatorValue = seed;
        }

        private void InnerExecute(T value)
        {
            accumulatorValue = accumulator(value, accumulatorValue);
            NotifyObservers(accumulatorValue);
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
