using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class ConcatSubject<T> : BaseSubject<T>
    {
        private IObservable<T> firstsource;
        private IObservable<T> secondsource;
        private IDisposable firstsubscriped;
        private IDisposable secondsubscriped;
        private BaseSubject<T> firstsubject;
        private BaseSubject<T> secondsubject;
        private double value;
        private int count;

        public ConcatSubject(IObservable<T> firstsource, IObservable<T> secondsource)
        {
            this.firstsource = firstsource;
            this.secondsource = secondsource;
            BaseSubject<T> firstsubject = (BaseSubject<T>)firstsource;
            this.firstsubject = firstsubject;
            var observer =
                new Observer<T>(
                    value => InnerExecute(value),
                    ex => NotifyErrorObservers(ex),
                    () => InnerCompleted());
            this.firstsubscriped = firstsubject.ColdSubscribe(observer);
        }

        private void InnerExecute(T value)
        {
            NotifyObservers(value);
        }

        private void InnerCompleted()
        {
            BaseSubject<T> secondsubject = (BaseSubject<T>)secondsource;
            this.secondsubject = secondsubject;
            var observer =
                new Observer<T>(
                    value => InnerExecute(value),
                    ex => NotifyErrorObservers(ex),
                    () => NotifyCompleteObservers() );
            this.secondsubscriped = secondsubject.Subscribe(observer);
        }

        public override void Execute()
        {
            try
            {
                firstsubject.Execute();                
            }
            catch (Exception exception)
            {
                NotifyErrorObservers(exception);
            }          
        }
    }
}
