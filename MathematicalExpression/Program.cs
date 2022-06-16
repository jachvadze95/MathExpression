using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MathematicalExpression
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string expression = Console.ReadLine();

            string formatedExpression = formatExpression(expression);
            String[] expressionArr = new[] { formatedExpression };

            eliminateBrackets(expressionArr);
            double answer = evaluateExpression(expressionArr);

            Console.WriteLine(answer);
            Console.ReadKey(); 
        }

        private static string formatExpression(string expression)
        {
            var exp = Regex.Replace(expression, @"\s+", "");


            var sb = new StringBuilder();
            sb.Append(exp[0]);

            for (int i = 1; i < exp.Length; i++)
            {
                var ch = exp[i];
                var prev = exp[i - 1];

                if ((ch == '(' && prev == '(') || (ch == ')' && prev == ')'))
                {
                    continue;
                }

                if ((ch == '-' && prev == '-'))
                {
                    sb.Remove(i - 1, 1);
                    sb.Append('+');

                    continue;
                }

                sb.Append(ch);
            }

            return sb.ToString();
        }

        private static void eliminateBrackets(string[] expressionArr)
        {
            var item = expressionArr[0];
            var sb = new StringBuilder();

            int lastOpenBracked = item.LastIndexOf('(');
            if (lastOpenBracked == -1)
            {
                return;
            }

            int i = lastOpenBracked + 1;

            while (item[i] != ')')
            {
                sb.Append(item[i]);
                i++;
            }

            int properClosingIndex = i;

            double value = evaluateExpression(new[] { sb.ToString() });


            sb.Insert(0, '(');
            sb.Append(')');
            expressionArr[0] = expressionArr[0].Replace(sb.ToString(), value.ToString());

            eliminateBrackets(expressionArr);
        }

        static double evaluateExpression(string[] expressionArr)
        {
            string[] items = decoupleToArray(expressionArr[0]).ToArray();

            while (items.Contains("/") || items.Contains("*"))
            {
                var i = Array.FindIndex(items, x => x == "/");

                if (i != -1)
                {
                    items[i - 1] = (decimal.Parse(items[i - 1]) / decimal.Parse(items[i + 1])).ToString();
                    Array.Clear(items, i - 1, 2);
                }

                var j = Array.FindIndex(items, x => x == "*");

                if (j != -1)
                {
                    items[j - 1] = (decimal.Parse(items[j - 1]) * decimal.Parse(items[j + 1])).ToString();
                    Array.Clear(items, j - 1, 2);
                }
            }

            return items.Sum(x =>
            {
                if (int.TryParse(x, out var i))
                {
                    return i;
                }

                return 0;
            });
        }

        private static IEnumerable<string> decoupleToArray(string v)
        {
            for (int i = 0; i < v.Length; i++)
            {
                var c = v[i];

                if (c == '+')
                    continue;

                if (c == '/' || c == '*')
                {
                    yield return c.ToString();
                }

                int start = i;

                if (c == '-')
                {
                    i++;
                }

                while (v.Length > i && isNum((int)v[i]))
                {
                    i++;
                }

                i--;
                yield return v.Substring(start, i - start + 1);
            }
        }

        static bool isNum(int c)
        {
            int z = (int)'0';
            int n = (int)'9';
            return (c >= z && c <= n) || c == '.';
        }
    }
}
