using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class AmbSubject<T> : BaseSubject<T>
    {
        private IDisposable firstsubscriped;
        private IDisposable secondsubscriped;
        private BaseSubject<T> first;
        private BaseSubject<T> second;
        private TimeSpan dueTime;
        private bool firstReacted;
        private bool secondReacted;

        public AmbSubject(IObservable<T> first, IObservable<T> second)
        {
            this.first = (BaseSubject<T>)first;
            this.second = (BaseSubject<T>)second;            
            var firstobserver =
                new Observer<T>(
                    value => InnerExecuteFirst(value),
                    ex => NotifyErrorObservers(ex),
                    () => NotifyCompleteObservers() );
            var secondobserver =
                new Observer<T>(
                    value => InnerExecuteSecond(value),
                    ex => NotifyErrorObservers(ex),
                    () => NotifyCompleteObservers() );
            this.firstsubscriped = this.first.ColdSubscribe(firstobserver);
            this.secondsubscriped = this.second.ColdSubscribe(secondobserver);
        }

        private void InnerExecuteFirst(T value)
        {
            try
            {
                if (!secondReacted)
                {
                    firstReacted = true;
                    NotifyObservers(value);
                }
            }
            catch (Exception exception)
            {
                NotifyErrorObservers(exception);
            }
        }

        private void InnerExecuteSecond(T value)
        {
            try
            {
                if (!firstReacted)
                {
                    secondReacted = true;
                    NotifyObservers(value);
                }
            }
            catch (Exception exception)
            {
                NotifyErrorObservers(exception);
            }
        }

        public override void Execute()
        {
            first.Execute();
            second.Execute();            
        }
    }

    public class AmbSubjectArray<T> : BaseSubject<T>
    {
        private IObservable<T>[] source;
        private bool firstReacted;

        public class AmbSubjectObject<T> : BaseSubject<T>
        {
            private IDisposable subscriped;
            private BaseSubject<T> subject;
            private BaseSubject<T> parentSubject;
            private bool localReacted;

            private Reactor reactor;

            public AmbSubjectObject(BaseSubject<T> subject, BaseSubject<T> parentSubject, Reactor reactor)
            {
                this.reactor = reactor;
                this.parentSubject = parentSubject;
                var observer =
                    new Observer<T>(
                        value => InnerExecute(value),
                        ex => { throw new Exception(ex.Message); },
                        () => { });
                this.subject = subject;
                this.subject.ColdSubscribe(observer);
            }

            private void InnerExecute(T value)
            {
                if (!reactor.FirstReacted)
                {
                    reactor.FirstReacted = true;
                    reactor.firstReactedObject = this;                    
                    foreach (var item in parentSubject.observers)
                    {
                        item.OnNext(value);
                    }
                }
                else if (reactor.firstReactedObject.Equals(this))
                {
                    foreach (var item in parentSubject.observers)
                    {
                        item.OnNext(value);
                    }
                }
            }

            public override void Execute()
            {
                
            }
        }

        private List<AmbSubjectObject<T>> list = new List<AmbSubjectObject<T>>();

        public class Reactor
        {
            public bool FirstReacted;
            public object firstReactedObject;
        }

        public AmbSubjectArray(params IObservable<T>[] source)
        {
            this.source = source;
            Reactor reactor = new Reactor();
            foreach (var item in source)
            {
                BaseSubject<T> subject = (BaseSubject<T>)item;
                AmbSubjectObject<T> obj = new AmbSubjectObject<T>(subject, this, reactor);                
            }            
        }
        
        public override void Execute()
        {
            foreach (var item in source)
            {
                BaseSubject<T> subject = (BaseSubject<T>)item;
                subject.Execute();
            }
        }
    }
}
