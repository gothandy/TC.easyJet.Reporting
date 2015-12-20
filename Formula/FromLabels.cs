using System;
using System.Collections.Generic;

namespace Vincente.Formula
{
    public class FromLabels
    {
        public static string GetEpic(List<string> labels)
        {
            return MatchOneElseReturnNull(labels, "eJ ");
        }

        private static string MatchOneElseReturnNull(List<string> labels, string match)
        {
            // Should use linq here.
            List<string> matching = GetMatchingLabels(labels, match);

            if (matching.Count == 2)
            {
                if (matching[0] == matching[1]) return matching[0];
            }

            if (matching.Count > 1) return null;

            if (matching.Count == 0) return null;

            return matching[0];
        }

        public static DateTime? GetInvoice(List<string> labels, string listName)
        {
            List<string> concat = new List<string>();
            concat.AddRange(labels);
            concat.Add(listName);

            string fromLabels = MatchOneElseReturnNull(concat, "Invoice ");

            if (fromLabels == null) return null;

            var year = int.Parse(fromLabels.Substring(0, 4));
            var month = int.Parse(fromLabels.Substring(5, 2));
            var day = int.Parse(fromLabels.Substring(8, 2));

            return new DateTime(year, month, day);
        }

        private static List<string> GetMatchingLabels(List<string> labels, string match)
        {
            var matchingLabels = new List<String>();

            foreach (string label in labels)
            {
                if (label.StartsWith(match))
                {
                    matchingLabels.Add(label.Substring(match.Length));
                }
            }

            return matchingLabels;
        }
    }
}
