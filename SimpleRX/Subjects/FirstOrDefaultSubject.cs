using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRX
{
    public class FirstOrDefaultSubject<T> : BaseSubject<T>
    {
        private IObservable<T> source;        
        private IDisposable subscriped;
        private BaseSubject<T> subject;
        private bool firstSet;
        private T first = default(T);

        public T First
        {
            get { return first; }
            set { first = value; }
        }

        public FirstOrDefaultSubject(IObservable<T> source)
        {
            this.source = source;
            BaseSubject<T> subject = (BaseSubject<T>)source;
            this.subject = subject;
            var observer =
                new Observer<T>(
                    localvalue => InnerExecute(localvalue),
                    ex => NotifyErrorObservers(ex),
                    () => NotifyCompleteObservers() );
            this.subscriped = subject.ColdSubscribe(observer);
        }

        private void InnerExecute(T value)
        {
            if (!firstSet)
            {
                firstSet = true;
                first = value;
            }
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
