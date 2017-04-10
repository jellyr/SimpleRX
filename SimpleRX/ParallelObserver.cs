using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleRX
{
    public class ParallelObserver<T> : IObserver<T>
    {        
        private Action<T> OnNext_ = p => { };
        private Action OnCompleted_ = () => { };
        private Action<Exception> OnError_ = ex => { };

        public ParallelObserver(Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            this.OnNext_ = onNext;
            this.OnError_ = onError;
            this.OnCompleted_ = onCompleted;
        }

        public ParallelObserver(Action<T> onNext, Action<Exception> onError)
        {
            this.OnNext_ = onNext;
            this.OnError_ = onError;
        }

        public ParallelObserver(Action<T> onNext)
        {
            this.OnNext_ = onNext;            
        }

        public ParallelObserver(IObserver<T> observer)
        {
            this.OnNext_ = observer.OnNext;
            this.OnError_ = observer.OnError;
            this.OnCompleted_ = observer.OnCompleted;
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
