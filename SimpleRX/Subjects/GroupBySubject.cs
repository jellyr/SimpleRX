using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class GroupBySubject<T, TKey> : BaseSubject<IGroupedObservable<T, TKey>> //IObservable<IGroupedObservable<TKey, T>>
    {
        private IObservable<T> source;
        private IDisposable subscriped;
        private BaseSubject<T> subject;
        private Func<T, TKey> keySelector;
        private Dictionary<TKey, GroupByObservableSubject<T>> dictionary = new Dictionary<TKey, GroupByObservableSubject<T>>();

        public GroupBySubject(IObservable<T> source, Func<T, TKey> keySelector)
        {
            this.source = source;
            this.keySelector = keySelector; 
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
            TKey key = keySelector(value);
            GroupByObservableSubject<T> groupByObservableSubject = new GroupByObservableSubject<T>();
            groupByObservableSubject.Value = value;            
            GroupBySubSubject<T, TKey> groupBySubSubject = new GroupBySubSubject<T, TKey>(groupByObservableSubject, key);
            NotifyObservers(groupBySubSubject);         
        }

        public override void Execute()
        {
            subject.Execute();                      
        }
    }
}
