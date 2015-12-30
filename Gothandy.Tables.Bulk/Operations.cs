using Gothandy.Tables.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gothandy.Tables.Bulk
{
    public class Operations<T> where T : ICompare<T>
    {
        private ITableWrite<T> table;

        public Operations(ITableWrite<T> table)
        {
            this.table = table;
        }

        public Results CompareLists(List<T> oldList, List<T> newList)
        {
            return new Results();
        }

        public Results BatchCompare(List<T> oldList, List<T> newList)
        {
            Results results = new Results();

            InsertReplaceOrIgnore(oldList, newList, results);
            Delete(oldList, newList, results);

            table.BatchComplete();

            return results;
        }

        private void InsertReplaceOrIgnore(List<T> oldList, List<T> newList, Results results)
        {
            foreach (T newCard in newList)
            {

                var oldCard = (from c in oldList
                                    where c.KeyEquals(newCard)
                                    select c).FirstOrDefault();

                if (oldCard == null)
                {
                    results.Inserted++;
                    table.BatchInsert(newCard);
                }
                else if (oldCard.ValueEquals(newCard))
                {
                    results.Ignored++;
                }
                else
                {
                    results.Replaced++;
                    table.BatchReplace(newCard);
                }
            }
        }

        private void Delete(List<T> oldList, List<T> newList, Results results)
        {
            foreach (T oldCard in oldList)
            {
                var newCard =
                    (from c in newList
                     where c.KeyEquals(oldCard)
                     select c).FirstOrDefault();

                if (newCard == null)
                {
                    results.Deleted++;
                    table.BatchDelete(oldCard);
                }
            }
        }
    }
}
