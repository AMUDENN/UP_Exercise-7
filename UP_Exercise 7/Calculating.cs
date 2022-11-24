using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UP_Exercise_7
{
    public class CalculateEventArgs
    {
        public string Message { get; }

        public CalculateEventArgs(string msg)
        {
            Message = msg;
        }
    }
    internal class Calculating
    {
        public delegate void Message(CalculateEventArgs e);
        public static event Message ErrorMessage;

        public static string[] numbers = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        public static double Evaluate(string expression)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            return Convert.ToDouble(table.Compute(expression, String.Empty));
        }
        public static string Change_Symbols(string expression)
        {
            string new_expression = "";
            for (int i = 0; i < expression.Length; i++)
            {
                switch (expression[i])
                {
                    case '%':
                        new_expression += "/100";
                        break;
                    case '÷':
                        new_expression += '/';
                        break;
                    case '×':
                        new_expression += '*';
                        break;
                    case 'π':
                        new_expression += Math.PI.ToString().Replace(',', '.');
                        if (i > 0 && (numbers.Contains(expression[i - 1].ToString()) || "eπ)".Contains(expression[i - 1])))
                        {
                            new_expression = new_expression.Insert(new_expression.Length - Math.PI.ToString().Length, "*");
                            break;
                        }
                        if (numbers.Contains(expression[i + 1].ToString()))
                        {
                            new_expression += '*';
                            break;
                        }
                        break;
                    case 'e':
                        new_expression += Math.Exp(1).ToString().Replace(',', '.');
                        if (i > 0 && (numbers.Contains(expression[i - 1].ToString())|| "eπ".Contains(expression[i - 1])))
                        {
                            new_expression = new_expression.Insert(new_expression.Length - Math.Exp(1).ToString().Length, "*");
                            break;
                        }
                        if (numbers.Contains(expression[i + 1].ToString()))
                        {
                            new_expression += '*';
                            break;
                        }
                        break;
                    default:
                        new_expression += expression[i];
                        break;
                }
            }
            return new_expression;
        }
        public static double Factorial(int fact)
        {
            double result = 1;
            for (int i = 2; i <= fact; i++)
            {
                result *= i;
            }
            return result;
        }
        public static string Fact_str(string expression)
        {
            try
            {
                while (expression.Contains('!'))
                {
                    int i = expression.IndexOf('!') - 1;
                    string exp = "";
                    string res = "";

                    int count_f = 0;
                    int count_l = 1;
                    while (count_f != count_l)
                    {
                        i--;
                        if (expression[i] == ')')
                        {
                            count_l++;
                        }
                        if (expression[i] == '(')
                        {
                            count_f++;
                        }
                        exp = exp.Insert(0, expression[i].ToString());
                    }
                    exp += ")";
                    exp = Add_Fl(exp);
                    res = Evaluate(Change_Symbols(exp)).ToString().Replace(',', '.');
                    Exception ex = ExceptionFunctions.Ex_Int(res, "Факториал", 0, 170);
                    Exception ex_d = ExceptionFunctions.Ex_Double(res.Replace('.', ','), "Факториал");
                    if (ex == null)
                    {
                        int fact = Convert.ToInt32(res);
                        expression = expression.Remove(i, expression.IndexOf('!') - i + 1);
                        expression = expression.Insert(i, Factorial(fact).ToString().Replace(',', '.'));
                    }
                    else if (ex_d == null)
                    {
                        throw new Exception("Факториал: Ошибка! Неверный тип данных! Введите целое число!");
                    }
                    else
                    {
                        throw ex;
                    }
                }
                return expression;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string Sqrt_str(string expression)
        {
            try
            {
                while (expression.Contains('√'))
                {
                    int i = expression.IndexOf('√') + 1;
                    int j;
                    int index = expression.IndexOf('√');
                    string exp = "";
                    string res = "";

                    int count_f = 1;
                    int count_l = 0;

                    while (count_f != count_l)
                    {
                        i++;
                        if (expression[i] == ')')
                        {
                            count_l++;
                        }
                        if (expression[i] == '(')
                        {
                            count_f++;
                        }
                    }
                    j = i;
                    while (expression[i] != '√')
                    {
                        exp = exp.Insert(0, expression[i].ToString());
                        if (expression[i] == '(')
                        {
                            count_f--;
                        }
                        i--;
                    }
                    exp = Add_Fl(exp);
                    res = Evaluate(Change_Symbols(exp)).ToString();

                    double sqr = Convert.ToDouble(res);
                    expression = expression.Remove(index, j - index + 1);
                    expression = expression.Insert(index, Math.Sqrt(sqr).ToString().Replace(',', '.'));

                    expression = Add_Fl(expression);
                }
                return expression;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string Add_Fl(string expression)
        {
            for (int i = 0; i < expression.Length; i++)
            {
                if (numbers.Contains(expression[i].ToString()))
                {
                    string res = "";
                    string ful_res = "";

                    while (numbers.Contains(expression[i].ToString()))
                    {
                        res += expression[i];
                        i++;
                    }
                    ful_res = res;
                    while (numbers.Contains(expression[i].ToString()) || expression[i] == '.')
                    {
                        ful_res += expression[i];
                        i++;
                    }

                    if (expression[i] != 'E')
                    {
                        string num = Convert.ToDouble(res.Replace('.', ',')).ToString("E10").Replace(',', '.');
                        int ind = num.Length;
                        num = num.Remove(ind - 3, 3).Insert(ind - 3, Convert.ToInt32(num.Substring(ind - 3, 3)).ToString());
                        string full = ful_res;
                        int index = expression.IndexOf(full, i - full.Length);
                        if (ful_res.Contains('.'))
                        {
                            ful_res = ful_res.Remove(0, ful_res.IndexOf('.'));
                            expression = expression.Remove(index, full.Length);
                            expression = expression.Insert(index, "(" + num + "+0" + ful_res + ")");
                            i -= full.Length;
                            i += num.Length + ful_res.Length + 4;
                        }
                        else
                        {
                            expression = expression.Remove(index, full.Length);
                            expression = expression.Insert(index, num);
                            i -= full.Length;
                            i += num.Length;
                        }
                    }
                    if (expression[i] == 'E')
                    {
                        i += 2;
                        while (numbers.Contains(expression[i].ToString()))
                        {
                            i++;
                        }
                    }
                }
            }
            return expression;
        }
        public static string Calculate_Out(string expression)
        {
            string old_expression = expression;
            try
            {
                if (expression.Where(x => x == '(').Count() != expression.Where(x => x == ')').Count())
                {
                    throw new Exception("Количество открывающих и закрывающих скобок не совпадает");
                }
                Exception ex;

                expression += " ";

                expression = Change_Symbols(expression);

                expression = Sqrt_str(expression);

                expression = Fact_str(expression);

                if (!expression.ToLower().Contains("ошибка"))
                {
                    expression = Evaluate(Add_Fl(Change_Symbols(expression.Replace(',', '.')))).ToString();
                }
                else
                {
                    throw new Exception(expression);
                }
                ex = ExceptionFunctions.Ex_Double(expression, "Результат");
                if (ex == null)
                {
                    return Convert.ToDouble(expression).ToString().Replace(',', '.');
                }
                else
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(new CalculateEventArgs($"Ошибка: {ex.Message} \nНекорректное выражение: \n{old_expression}"));
                return "Некорректное выражение";
            }
        }
    }
}
