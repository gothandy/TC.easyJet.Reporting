using System;

namespace Vincente.Formula
{
    public class FromName
    {
        public static string GetShortName(string fullName)
        {
            string name;

            var domId = Formula.FromName.GetDomID(fullName);

            if (domId == null)
            {
                name = fullName;
            }
            else
            {
                var pos = fullName.IndexOf(domId.Substring(1));

                name = fullName.Substring(pos + domId.Length);
                
            }

            name = name.Replace("â€“", "-");
            name = name.Replace("â€˜", "'");
            name = name.Replace("â€™", "'");

            if (name.StartsWith("- ")) name = name.Substring(2);

            name = name.Trim();

            return name;
        }

        public static string GetDomID(string name)
        {
            string[] words = name.Split(' ');

            foreach (string word in words)
            {

                if (word.StartsWith("20") && word.Contains("."))
                {
                    var decimalIndex = word.IndexOf(".");

                    if (decimalIndex != 8) return null;
                    if (word.Length - decimalIndex > 3) return null;
                    if (word.Length - decimalIndex < 2) return null;
                    return String.Format("D{0}", word);
                }
            }

            return null;
        }
    }
}
