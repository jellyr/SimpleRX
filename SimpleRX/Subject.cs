using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class Subject<T> : BaseSubject<T>, IObservable<T>, IObserver<T>, IDisposable
    {
        private Action<T> OnNext_ = p => { };
        private Action OnCompleted_ = () => { };
        private Action<Exception> OnError_ = ex => { };

        public Subject(Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            this.OnNext_ = onNext;
            this.OnError_ = onError;
            this.OnCompleted_ = onCompleted;
        }

        public Subject(Action<T> onNext, Action<Exception> onError)
        {
            this.OnNext_ = onNext;
            this.OnError_ = onError;
        }

        public Subject(Action<T> onNext)
        {
            this.OnNext_ = onNext;            
        }

        public Subject()
        {
            
        }
       
        public void OnCompleted()
        {
            NotifyCompleteObservers();
        }

        public void OnError(Exception error)
        {
            NotifyErrorObservers(error);
        }

        public void OnNext(T value)
        {
            NotifyObservers(value);
        }       

        public void Dispose()
        {
            NotifyCompleteObservers();
        }

        public override void Execute()
        {
            
        }
    }
}
