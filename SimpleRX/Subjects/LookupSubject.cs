using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRX
{
    public class LookupSubject<T, TKey> : BaseSubject<ILookup<TKey, T>>
    {
        private IObservable<T> source;
        private Func<T, TKey> keySelector;
        private IDisposable subscriped;
        private BaseSubject<T> subject;
        private Lookup<TKey, T> lookup = new Lookup<TKey, T>();

        public LookupSubject(IObservable<T> source, Func<T, TKey> keySelector)
        {
            this.source = source;
            this.keySelector = keySelector;
            BaseSubject<T> subject = (BaseSubject<T>)source;
            this.subject = subject;
            var observer =
                new Observer<T>(
                    value => InnerExecute(value),
                    ex => NotifyErrorObservers(ex),
                    () => NotifyCompleteObservers());
            this.subscriped = subject.ColdSubscribe(observer);
        }

        private void InnerExecute(T value)
        {
            var selectorValue = keySelector(value);
            lookup.Add(selectorValue, value);
        }

        public override void Execute()
        {
            try
            {
                subject.Execute();
                NotifyObservers(lookup);
            }
            catch (Exception exception)
            {
                NotifyErrorObservers(exception);
            }
        }
    }

    public class Lookup<TKey, TElement> : ILookup<TKey, TElement>
    {
        private Dictionary<TKey, List<TElement>> lookup = new Dictionary<TKey, List<TElement>>();
        private List<TKey> keys;

        public bool Contains(TKey key)
        {
            return lookup.ContainsKey(key);
        }

        public void Add(TKey key, TElement element)
        {
            List<TElement> list = null;
            if(!lookup.TryGetValue(key, out list))
            {
                list = new List<TElement>();
                list.Add(element);
                lookup.Add(key, list);
            }
            list.Add(element);
        }

        public int Count
        {
            get { return lookup.Count(); }
        }

        public IEnumerable<TElement> this[TKey key]
        {
            get
            {
                List<TElement> list = null;
                if (!lookup.TryGetValue(key, out list))
                {
                    yield return default(TElement);
                }
                foreach (var item in list)
                {
                    yield return item;
                }
            }
        }

        public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
        {
            foreach (KeyValuePair<TKey, List<TElement>> item in lookup)
            {
                Grouping<TKey, TElement> grouping = new Grouping<TKey, TElement>(item.Key);
                foreach (var element in item.Value)
                {
                    grouping.Add(element);
                }
                yield return grouping;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
    {
        private TKey key = default(TKey);
        private List<TElement> group = new List<TElement>();

        public Grouping(TKey key)
        {
            this.key = key;
        }

        public void Add(TElement element)
        {
            group.Add(element);
        }

        public TKey Key
        {
            get { return key; }
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            return group.Select(x => x).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
