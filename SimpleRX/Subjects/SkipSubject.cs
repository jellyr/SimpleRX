using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRX
{
    public class SkipSubject<T> : BaseSubject<T>
    {
        private IObservable<T> source;        
        private IDisposable subscriped;
        private BaseSubject<T> subject;
        private int count;
        private int counter = 0;

        public SkipSubject(IObservable<T> source, int count)
        {
            this.source = source;
            this.count = count;
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
            if (counter >= count)
            {
                NotifyObservers(value);
            }
            counter++;
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
