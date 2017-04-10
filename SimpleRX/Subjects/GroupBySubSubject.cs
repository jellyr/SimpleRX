using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class GroupBySubSubject<T, TKey> : BaseSubject<T>, IGroupedObservable<T, TKey>
    {
        private IObservable<T> source;
        private IDisposable subscriped;
        private BaseSubject<T> subject;
        private TKey key;

        public GroupBySubSubject(IObservable<T> source, TKey key)
        {
            this.source = source;
            this.key = key;
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
            NotifyObservers(value);         
        }

        public override void Execute()
        {
            subject.Execute();
        }

        public TKey Key
        {
            get { return key; }
        }
    }
}
