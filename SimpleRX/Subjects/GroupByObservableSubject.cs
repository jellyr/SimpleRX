using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class GroupByObservableSubject<T> : BaseSubject<T>
    {
        private T value_;

        public T Value
        {
            get { return value_; }
            set { value_ = value; }
        }

        public List<IObserver<T>> observers;
        public bool running;

        public GroupByObservableSubject()
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

        public override void Execute()
        {
            NotifyObservers(value_);
        }
    }
}
