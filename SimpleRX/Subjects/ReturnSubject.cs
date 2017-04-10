using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRX
{
    public class ReturnSubject<T> : BaseSubject<T>
    {
        private T value;

        public ReturnSubject(T value)
        {
            this.value = value;
        }

        public override void Execute()
        {
            NotifyObservers(value);
            NotifyCompleteObservers();            
        }
    }

    public class ReturnSubjectArray<T> : BaseSubject<T>
    {
        private T[] array;

        public ReturnSubjectArray(T[] array)
        {
            this.array = array;
        }

        public override void Execute()
        {
            foreach (var arrayItem in array)
            {
                NotifyObservers(arrayItem);
            }
            NotifyCompleteObservers();                  
        }
    }

    public class ReturnSubjectCollection<T> : BaseSubject<T>
    {
        private IEnumerable<T> collection;

        public ReturnSubjectCollection(IEnumerable<T> collection)
        {
            this.collection = collection;
        }

        public override void Execute()
        {
            foreach (var arrayItem in collection)
            {
                NotifyObservers(arrayItem);
            }
            NotifyCompleteObservers();             
        }
    }
}
