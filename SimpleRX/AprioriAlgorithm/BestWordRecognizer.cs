using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AprioriAlgorithm
{
    public class BestWordRecognizer
    {
        private IEnumerable<IEnumerable<string>> transactions;

        public BestWordRecognizer(IEnumerable<IEnumerable<string>> transactions)
        {
            this.transactions = transactions;
        }

        private Item BuildItem(List<string> words)
        {
            Item wordItem = new Item();
            foreach (var word in words)
            {
                wordItem.ItemKeys.Add(word);
            }
            return wordItem;
        }

        private Dictionary<int, List<Item>> BuildLResultItems(List<List<Item>> lresultList)
        {
            Dictionary<int, List<Item>> lresultItems = new Dictionary<int, List<Item>>();
            foreach (var list in lresultList)
            {
                if (list.Count > 0)
                {
                    var length = list[0].ItemKeys.Count;
                    lresultItems.Add(length, list);
                }
            }
            return lresultItems;
        }

        private Item FindItem(Item wordItem, List<Item> lresultItemList)
        {
            Item foundItem = null;
            foreach (var lresultItem in lresultItemList)
            {
                if (lresultItem.Equals(wordItem))
                {
                    foundItem = lresultItem;
                }
            }
            return foundItem;
        }

        public List<AssociationItem> BestItems(List<string> words)
        {
            Apriori apriori = new Apriori();
            AssociationRules associtionRules = new AssociationRules();
            List<Item> items = apriori.BuildStartItems(transactions);
            var result = apriori.CalcuateApriori(transactions);
            Dictionary<int, List<Item>> lresultDictionary = BuildLResultItems(apriori.LResultLists);                        
            var wordItem = BuildItem(words);
            var wordsLength = wordItem.ItemKeys.Count;
            if (lresultDictionary.ContainsKey(wordsLength))
            {
                List<Item> lresultItemList = lresultDictionary[wordsLength];
                Item foundItem = FindItem(wordItem, lresultItemList);
                if(foundItem != null)
                {
                    List<Item> foundItems = new List<Item>() { foundItem };
                    return associtionRules.CalcuateAssoctionRules(foundItems, items, transactions);
                }
            }
            return null;
        }

        private AssociationItem FindBestAssoctiationRule(List<AssociationItem> wordAssociationItems)
        {
            AssociationItem wordAssocationBestRule = null;
            if (wordAssociationItems != null)
            {
                foreach (var wordAssocationRule in wordAssociationItems)
                {
                    if (wordAssocationBestRule == null)
                    {
                        wordAssocationBestRule = wordAssocationRule;
                    }
                    else
                    {
                        if (wordAssocationBestRule.Confidence < wordAssocationRule.Confidence)
                        {
                            wordAssocationBestRule = wordAssocationRule;
                        }
                    }
                }
            }
            return wordAssocationBestRule;
        }

        public AssociationItem BestItem(List<string> words)
        {
            List<AssociationItem> associtionRules = BestItems(words);
            return FindBestAssoctiationRule(associtionRules);
        }
    }
}
