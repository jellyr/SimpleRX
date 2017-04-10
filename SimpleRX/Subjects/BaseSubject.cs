using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public abstract class BaseSubject<T> : IObservable<T>
    {
        public List<IObserver<T>> observers;
        private IScheduler scheduler = new TaskScheduler();
        public bool running;

        public IScheduler Scheduler
        {
            get { return scheduler; }
            set { this.scheduler = value; }
        }

        public abstract void Execute();

        protected virtual void NotifyObservers(T value)
        {
            foreach (var item in observers)
            {
                item.OnNext(value);
            }     
        }        

        protected virtual void NotifyErrorObservers(Exception exception)
        {
            foreach (var item in observers)
            {
                item.OnError(exception);
            }
        }

        protected virtual void NotifyCompleteObservers()
        {
            foreach (var item in observers)
            {
                item.OnCompleted();
            }     
        }

        public BaseSubject()
        {
            observers = new List<IObserver<T>>();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            IDisposable disposable = new DisposeObject<T>(observers, this, observer);
            observers.Add(observer);
            if (!running)
            {
                this.running = true;
                Execute();
            }
            return disposable;
        }

        public IDisposable ColdSubscribe(IObserver<T> observer)
        {
            IDisposable disposalble = new DisposeObject<T>(observers, this, observer);
            observers.Add(observer);
            return disposalble;
        }
    }
}
