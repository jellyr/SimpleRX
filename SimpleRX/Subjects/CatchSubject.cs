using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class CatchSubject<T> : BaseSubject<T>
    {
        private IObservable<T> firstsource;
        private IObservable<T> secondsource;
        private IDisposable subscriped;
        private BaseSubject<T> firstsubject;
        private BaseSubject<T> secondsubject;

        public CatchSubject(IObservable<T> firstsource, IObservable<T> secondsource)
        {
            this.firstsource = firstsource;
            this.secondsource = secondsource;
            BaseSubject<T> firstsubject = (BaseSubject<T>)firstsource;
            this.firstsubject = firstsubject;
            var observer =
                new Observer<T>(
                    value => InnerExecute(value),
                    ex => ErrorExecute(ex),
                    () => NotifyCompleteObservers());
            this.subscriped = firstsubject.ColdSubscribe(observer);
        }

        private void InnerExecute(T value)
        {
            NotifyObservers(value);
        }

        private void ErrorExecute(Exception exception)
        {
            BaseSubject<T> secondsubject = (BaseSubject<T>)secondsource;
            this.secondsubject = secondsubject;
            var observer =
                new Observer<T>(
                    value => InnerExecute(value),
                    ex => NotifyErrorObservers(exception),
                    () => NotifyCompleteObservers());
            this.subscriped = secondsubject.Subscribe(observer);
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
