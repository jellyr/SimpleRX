using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class DisposeObject<T> : IDisposable
    {
        private List<IObserver<T>> observers;
        private BaseSubject<T> subject; 
        private IObserver<T> observer;

        public DisposeObject(List<IObserver<T>> observers, BaseSubject<T> subject, IObserver<T> observer)
        {
            this.observers = observers;
            this.observer = observer;
            this.subject = subject;
        }

        public void Dispose()
        {
            if (observers.Contains(observer))
                observers.Remove(observer);
            if (observers.Count == 0)
                subject.running = false;
        }
    }
}
