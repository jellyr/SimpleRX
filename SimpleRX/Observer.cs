using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{    
    public class Observer<T> : IObserver<T>
    {
        private Action<T> OnNext_ = p => { };
        private Action OnCompleted_ = () => { };
        private Action<Exception> OnError_ = ex => { };

        public Observer(Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            this.OnNext_ = onNext;
            this.OnError_ = onError;
            this.OnCompleted_ = onCompleted;
        }

        public Observer(Action<T> onNext, Action<Exception> onError)
        {
            this.OnNext_ = onNext;
            this.OnError_ = onError;
        }

        public Observer(Action<T> onNext)
        {
            this.OnNext_ = onNext;            
        }
       
        public void OnCompleted()
        {
            OnCompleted_();
        }

        public void OnError(Exception error)
        {
            OnError_(error);
        }

        public void OnNext(T value)
        {
            OnNext_(value);
        }       
    }
}
