using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRX
{
    public class LastOrDefaultSubject<T> : BaseSubject<T>
    {
        private IObservable<T> source;        
        private IDisposable subscriped;
        private BaseSubject<T> subject;
        private T last;

        public T Last
        {
            get { return last; }
            set { last = value; }
        }

        public LastOrDefaultSubject(IObservable<T> source)
        {
            this.source = source;
            BaseSubject<T> subject = (BaseSubject<T>)source;
            this.subject = subject;
            var observer =
                new Observer<T>(
                    value => InnerExecute(value),
                    ex => NotifyErrorObservers(ex),
                    () => NotifyCompleteObservers() );
            this.subscriped = subject.ColdSubscribe(observer);
        }

        private void InnerExecute(T value)
        {
            last = value;
        }

        public override void Execute()
        {
            Task task = Task.Factory.StartNew(() => 
            {
                try
                {
                    subject.Execute();
                }
                catch (Exception exception)
                {
                    NotifyErrorObservers(exception);
                }         
            });
            Task.WaitAll(task);
        }
    }
}
