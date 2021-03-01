using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace GroceryTracker
{
    class CleanItUp
    {
        public static void DataCleaning(TextData data)
        {
            try
            {
                string date = GetDate(data.PreCleanedText);
                string firstProcessedString = GeneralCleaning(data.PreCleanedText);
                List<string> prices = IsolatePrices(firstProcessedString);
                string[] products = ProductList(firstProcessedString);
                List<string> productNums = IsolateProductNum(products);
                List<string> productNames = IsolateProductName(products);

                data.PurchaseDate = date;
                data.ProductNames = productNames;
                data.ProductNumbers = productNums;
                data.ProductPrices = prices;
            }
            catch
            {
                Console.WriteLine("Error when cleaning data. Please try again.");
                Driver.Main();
            }
        }

        // isolate product name 
        public static List<string> IsolateProductName(string[] products)
        {

            // split on at least 3 nums 
            string pattern = @"\d{3,}";
            List<string> productName = new List<string>();
            Regex rx = new Regex(pattern, RegexOptions.IgnoreCase);
            foreach (string product in products)
            {
                var ogProductName = rx.Split(product);
                productName.Add(ogProductName[1]);
            }
            return productName;
        }

        // isolate product number 
        public static List<string> IsolateProductNum(string[] products)
        {
            //split on at least 3 nums 
            string pattern = @"\d{3,}";
            List<string> productNums = new List<string>();
            Match ogProductNum;
            foreach (string product in products) 
            {
                ogProductNum = Regex.Match(product, pattern, RegexOptions.IgnoreCase);
                productNums.Add(ogProductNum.ToString());
            }
          
            return productNums;
        }

        // gets product num + name 
        public static string[] ProductList(string firstProcessedString)
        {
            // get all products
            string pattern = @"[-]\d{1,3}(?:[.,]\d{3})*(?:[.,]\d{2})|\d{1,3}(?:[.,]\d{3})*(?:[.,]\d{2})";
            Regex rx = new Regex(pattern, RegexOptions.IgnoreCase);
            var splitResult = rx.Split(firstProcessedString);

            // replace return item num and name with "00000 RETURN" 
            pattern = @"[A-Z]\d{11}\s\w{3,}";
            string toReplace = "00000 RETURN";
            rx = new Regex(pattern, RegexOptions.IgnoreCase);
            for(int i = 0; i < splitResult.Length; i++)
            {
                splitResult[i] = rx.Replace(splitResult[i], toReplace);
            }

            // if extra \n, replace with "" 
            // if -, replace with ""
            // if only 1 character, replace with ""
            pattern = @"/^\s+|\s+$/g";
            string removeDashPattern = @"[-]";
            string removeOneChar = @"\b\w\b";
            toReplace = "";
            rx = new Regex(pattern);
            Regex rx2 = new Regex(removeDashPattern);
            Regex rx3 = new Regex(removeOneChar);
            
            for (int i = 0; i < splitResult.Length; i++)
            {
                splitResult[i] = rx.Replace(splitResult[i], toReplace);
                splitResult[i] = rx2.Replace(splitResult[i], toReplace);
                splitResult[i] = rx3.Replace(splitResult[i], toReplace);
            }

            // if no product num, assign "99999"
            pattern = @"\n[a-z]";
            toReplace = "99999 ";
            rx = new Regex(pattern, RegexOptions.IgnoreCase);
            for (int i = 0; i < splitResult.Length; i++)
            {
                splitResult[i] = rx.Replace(splitResult[i], toReplace);
            }

            // remove any empty values from splitResult 
            splitResult = splitResult.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

            return splitResult;

        }
        // isolate the prices 
        public static List<string> IsolatePrices(string generalCleaning)
        {
            //find all x.xx- and turn it to -x.xx
            string negPattern = @"\d{1,3}(?:[.,]\d{3})*(?:[.,]\d{2})[-]";
            Regex rx = new Regex(negPattern, RegexOptions.IgnoreCase);
            MatchCollection returns = rx.Matches(generalCleaning);
            string getReturns = "";
            foreach (Match match in returns)
            {
                getReturns += match + "\n";
            }
            CleanItUp p = new CleanItUp();

            MatchEvaluator evalatue = new MatchEvaluator(p.EvaluatorRegex);
            Regex r = new Regex(negPattern);

            // do actual conversion 
            string newString = r.Replace(generalCleaning, evalatue);

            // get all prices 
            string pattern = @"[-]\d{1,3}(?:[.,]\d{3})*(?:[.,]\d{2})|\d{1,3}(?:[.,]\d{3})*(?:[.,]\d{2})"; // targets x.xx- OR x.xx
            rx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection prices = rx.Matches(newString);
            string getPrices = "";
            foreach (Match match in prices)
            {
                getPrices += match + "\n";
            }

            string[] tempPrices = getPrices.Split('\n');
            List<string> finalPrices = tempPrices.ToList();

            return finalPrices;
        }

        // get date, if no date present, set date as today 
        public static string GetDate(string testing)
        {
            string pattern = @"\d{2,}/\d{2,}/\d{2}"; 
            var rx = new Regex(pattern, RegexOptions.IgnoreCase);
            Match matchedDate = rx.Match(testing);
            string date; 
            if (matchedDate.Success)
            {
                date = matchedDate.Value;
            }
            else
            {
                date = DateTime.Now.ToString("dd/mm/yyyy");
            }

            return date;

        }

        // general text cleaning 
        public static string GeneralCleaning(string testing)
        {
            // removes everything at and above member ID and AID in footer
            string pattern = @"\d{12}"; //gets 12 digits
            var rx = new Regex(pattern, RegexOptions.IgnoreCase);
            var splitResult = rx.Split(testing);
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
                return removeExtraCharOnLine;
        }

        // replace every x.xx- with -x.xx
        public string EvaluatorRegex(Match m)
        {
            string pattern = @"[-]";
            string[] number = Regex.Split(m.ToString(), pattern);
            string num = "-" + number[0];
            return num;
        }

    }
}
