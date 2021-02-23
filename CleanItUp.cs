using System;
using System.Text.RegularExpressions;
using System.Threading;

namespace GroceryTracker
{
    class CleanItUp
    {
        public static void InitialCleaning(string preCleanedText)
        {
            //removes everything at and above member ID and AID in footer
            string pattern = @"\d{12}"; //gets 12 digits
            var rx = new Regex(pattern, RegexOptions.IgnoreCase);
            var splitResult = rx.Split(preCleanedText);
            string removeCostcoHeader = splitResult[1];

            // removes everything at and below the word "total"
            pattern = @"\b(TOTAL)\b";
            rx = new Regex(pattern, RegexOptions.IgnoreCase);
            splitResult = rx.Split(removeCostcoHeader);
            string removeBelowTotal = splitResult[0];

            // removes everything at and below the word "subtotal"
            pattern = @"\b(SUBTOTAL)\b";
            rx = new Regex(pattern, RegexOptions.IgnoreCase);
            splitResult = rx.Split(removeBelowTotal);
            string removeBelowSubtotal = splitResult[0];

            // removes anything measured per pound -> ex 3 @ 4.49
            pattern = @"\d\s@\s\d[.]\d{2}";
            rx = new Regex(pattern, RegexOptions.IgnoreCase);
            splitResult = rx.Split(removeBelowSubtotal);
            string removePerPound = "";
            foreach (string strings in splitResult)
            {
                removePerPound += strings;
            }

            // removes astricks, _A_, and _E_ 
            pattern = @"[/*]|\s[A]\s|\s[E]\s";
            rx = new Regex(pattern, RegexOptions.IgnoreCase);
            splitResult = rx.Split(removePerPound);
            string removeAstricksAE = "";
            foreach (string strings in splitResult)
            {
                removeAstricksAE += strings;
            }

            // remove any letter that repeats more than twice
            pattern = @"([a-z])\1{2}";
            rx = new Regex(pattern, RegexOptions.IgnoreCase);
            string removeDuplicatingLetters = rx.Replace(removeAstricksAE, "");

            // removes multiple m_'s 
            pattern = @"[m]\s";
            rx = new Regex(pattern, RegexOptions.IgnoreCase);
            splitResult = rx.Split(removeDuplicatingLetters);
            string removeM = "";
            foreach (string strings in splitResult)
            {
                removeM += strings;
            }

            // removes any 1 char alone on a line
            pattern = @"\s\w\s";
            rx = new Regex(pattern, RegexOptions.IgnoreCase);
            splitResult = rx.Split(removeM);
            string removeExtraCharOnLine = "";
            foreach (string strings in splitResult)
            {
                removeExtraCharOnLine += strings + "\n";
            }

            //TODO: remove x.xx- from removeExtraCharOnLine aka list of prices 
                //could find x.xx- in the list, convert to double and *1 then remove the end - if easier 
            //find all x.xx- and turn it to -x.xx
            pattern = @"\d{1,3}(?:[.,]\d{3})*(?:[.,]\d{2})[-]"; // targets x.xx-
            rx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection returns = rx.Matches(removeExtraCharOnLine);
            string getReturns = "";
            foreach (Match match in returns)
            {
                getReturns += match + "\n";
            }

            pattern = @"\d{1,3}(?:[.,]\d{3})*(?:[.,]\d{2})"; // targets x.xx
            rx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection numbers = rx.Matches(getReturns);
            string negativeNums = "";
            foreach (Match strings in numbers)
            {
                negativeNums += "-" + strings.ToString() + "\n";
            }
            Console.WriteLine(negativeNums);

            // get all prices 
            pattern = @"\d{1,3}(?:[.,]\d{3})*(?:[.,]\d{2})";
            rx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection prices = rx.Matches(removeExtraCharOnLine);
            string getPrices = "";
            foreach (Match match in prices)
            {
                getPrices += match + "\n";
            }
            Console.WriteLine(getPrices);

            //TODO: remove any stray - from lines 

            // get all products
            pattern = @"\d{1,3}(?:[.,]\d{3})*(?:[.,]\d{2})";
            rx = new Regex(pattern, RegexOptions.IgnoreCase);
            splitResult = rx.Split(removeExtraCharOnLine);
            string getProducts = "";
            foreach (string strings in splitResult)
            {
                getProducts += strings + "\n";
            }
            Console.WriteLine(getProducts);
            Thread.Sleep(200000);
        }
    }
}
