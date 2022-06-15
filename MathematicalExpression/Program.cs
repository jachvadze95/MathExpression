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

            Console.WriteLine(evaluateExpression(Regex.Replace(expression, @"\s+", "")));
            Console.ReadKey();
        }


        static double evaluateExpression(string expression)
        {
            String[] expressionArr = new[] { expression };

            //Get First Left Part
            double left = getMostLeftPart(expressionArr);
            expression = expressionArr[0];
            if (expression.Length == 0)
            {
                return left;
            }

            char operand = expression[0];
            expression = expression.Substring(1);

            //Do Operation With Right Part Which Gets Evaluated Recursively
            while (operand == '/' || operand == '*')
            {
                expressionArr[0] = expression;
                double right = getMostLeftPart(expressionArr);
                expression = expressionArr[0];

                if (operand == '*')
                {
                    left = left * right;
                }
                else
                {
                    left = left / right;
                }

                if (expression.Length > 0)
                {
                    operand = expression[0];
                    expression = expression.Substring(1);
                }
                else
                {
                    return left;
                }
            }

            if (operand == '+')
            {
                return left + evaluateExpression(expression);
            }
            else
            {
                return left - evaluateExpression(expression);
            }
        }

        static double getMostLeftPart(String[] expArr)
        {
            double val;
            var item = expArr[0];

            if (item.StartsWith("("))
            {
                int open = 1;
                int len = 1;
                while (open != 0)
                {
                    if (item[len] == '(')
                    {
                        open++;
                    }
                    else if (item[len] == ')')
                    {
                        open--;
                    }
                    len++;
                }
                val = evaluateExpression(item.Substring(1, len - 2));
                expArr[0] = item.Substring(len);
            }
            else
            {
                int len = 1;
                if (item[0] == '-')
                {
                    len++;
                }
                while (item.Length > len && isNum((int)item[len]))
                {
                    len++;
                }
                val = Double.Parse(item.Substring(0, len));
                expArr[0] = item.Substring(len);
            }
            return val;
        }

        static bool isNum(int c)
        {
            int z = (int)'0';
            int n = (int)'9';
            return (c >= z && c <= n) || c == '.';
        }
    }
}
