using AprioriAlgorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRX
{
    public class AprioriSubject : Subject<string>
    {
        private Action<string> OnNext_ = p => { };
        private Action OnCompleted_ = () => { };
        private Action<Exception> OnError_ = ex => { };
        private IEnumerable<IEnumerable<string>> comparableValues;

        public AprioriSubject(Action<string> onNext, 
            Action<Exception> onError, 
            Action onCompleted, 
            IEnumerable<IEnumerable<string>> comparableValues)
        {
            this.OnNext_ = onNext;
            this.OnError_ = onError;
            this.OnCompleted_ = onCompleted;
            this.comparableValues = comparableValues;
        }

        public AprioriSubject(Action<string> onNext, 
            Action<Exception> onError, 
            IEnumerable<IEnumerable<string>> comparableValues)
        {
            this.OnNext_ = onNext;
            this.OnError_ = onError;
            this.comparableValues = comparableValues;
        }

        public AprioriSubject(Action<string> onNext, 
            IEnumerable<IEnumerable<string>> comparableValues)
        {
            this.OnNext_ = onNext;
            this.comparableValues = comparableValues;
        }

        public AprioriSubject(IEnumerable<IEnumerable<string>> comparableValues)
        {
            this.comparableValues = comparableValues;
        }
       
        public new void OnCompleted()
        {
            NotifyCompleteObservers();
        }

        public new void OnError(Exception error)
        {
            NotifyErrorObservers(error);
        }

        public new void OnNext(string value)
        {
            List<string> words = new List<string>();
            foreach(string word in value.Split(' '))
            {
                if(word != "")
                    words.Add(word);
            }
            BestWordRecognizer bestWordRecognizer = new BestWordRecognizer(comparableValues);
            var associtionRules = bestWordRecognizer.BestItems(words);
            var associtionRule = bestWordRecognizer.BestItem(words);   
            if(associtionRule != null)
                NotifyObservers(associtionRule.ToItem.ToString());
            else
                NotifyObservers("");  
        }       

        public new void Dispose()
        {
            NotifyCompleteObservers();
        }

        public override void Execute()
        {

        }
    }
}
