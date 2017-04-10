using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRX
{
    public class LevenshteinSubject : Subject<string>
    {
        private Action<string> OnNext_ = p => { };
        private Action OnCompleted_ = () => { };
        private Action<Exception> OnError_ = ex => { };
        private IEnumerable<string> comparableValues;

        public LevenshteinSubject(Action<string> onNext, Action<Exception> onError, Action onCompleted, IEnumerable<string> comparableValues)
        {
            this.OnNext_ = onNext;
            this.OnError_ = onError;
            this.OnCompleted_ = onCompleted;
            this.comparableValues = comparableValues;
        }

        public LevenshteinSubject(Action<string> onNext, Action<Exception> onError, IEnumerable<string> comparableValues)
        {
            this.OnNext_ = onNext;
            this.OnError_ = onError;
            this.comparableValues = comparableValues;
        }

        public LevenshteinSubject(Action<string> onNext, IEnumerable<string> comparableValues)
        {
            this.OnNext_ = onNext;
            this.comparableValues = comparableValues;
        }

        public LevenshteinSubject(IEnumerable<string> comparableValues)
        {
            this.comparableValues = comparableValues;
        }
       
        public void OnCompleted()
        {
            NotifyCompleteObservers();
        }

        public void OnError(Exception error)
        {
            NotifyErrorObservers(error);
        }

        public void OnNext(string value)
        {
            List<Tuple<int, string>> list = new List<Tuple<int, string>>();
            Levenshtein levensthein = new Levenshtein();
            foreach (var item in comparableValues)
            {
                var level = levensthein.CalculateValue(value, item);
                list.Add(new Tuple<int, string>(level, item));
            }
            var orderedList = list.OrderBy(t => t.Item1);
            int order = orderedList.First().Item1;
            foreach (var orderedItem in orderedList)
            {
                if (orderedItem.Item1 > order)
                    break;
                NotifyObservers(orderedItem.Item2);    
            }
        }       

        public void Dispose()
        {
            NotifyCompleteObservers();
        }

        public override void Execute()
        {
            
        }

        private class Levenshtein
        {
            public int CalculateValue(string source, string target)
            {
                var array = new int[source.Length + 1, target.Length + 1];

                for (int i = 0; i <= source.Length; i++)
                {
                    array[i, 0] = i;
                }
                for (int i = 0; i <= target.Length; i++)
                {
                    array[0, i] = i;
                }
                for (int i = 1; i <= source.Length; i++)
                {
                    for (int j = 1; j <= target.Length; j++)
                    {
                        var equal = (source[i - 1] == target[j - 1]) ? 0 : 2;
                        var minInsert = array[i - 1, j] + 1;
                        var minDelete = array[i, j - 1] + 1;
                        var minInterchange = array[i - 1, j - 1] + equal;
                        array[i, j] = Math.Min(Math.Min(minInsert, minDelete), minInterchange);
                    }
                }
                return array[source.Length, target.Length];
            }
        }
    }
}
