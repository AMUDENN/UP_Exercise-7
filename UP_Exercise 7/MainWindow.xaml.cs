using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UP_Exercise_7
{
    public partial class MainWindow : Window
    {
        static BrushConverter converter = new System.Windows.Media.BrushConverter();
        static string[] values = { "!", "(", ")", "%", "CE", "√", "7", "8", "9", "÷", "^", "4", "5", "6", "×", "π", "1", "2", "3", "-", "e", "0", ".", "=", "+" };
        public MainWindow()
        {
            InitializeComponent();
            SetButtons();
        }
        public void SetButtons()
        {
            foreach (string value in values)
            {
                Button button = new Button
                {
                    Content = value,
                    Style = (Style)Application.Current.Resources["ButtonStyle"],
                };
                button.Click += ButtonClick;
                button.Foreground = value == "=" ? (Brush)converter.ConvertFromString("#fff") : (Brush)converter.ConvertFromString("#202124");
                button.Background = Enumerable.Range(0, 10).Select(x => x.ToString()).Contains(value) || value == "." ? (Brush)converter.ConvertFromString("#f1f3f4") : value == "=" ? (Brush)converter.ConvertFromString("#4285f4") : (Brush)converter.ConvertFromString("#dadce0");
                MainUniformGrid.Children.Add(button);
            }
        }
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
                    case ')':
                        new_expression += ')';
                        Console.WriteLine(new_expression);
                        Console.WriteLine(i);
                        if (Enumerable.Range(0, 10).Select(x => x.ToString()).Contains(expression[i + 1].ToString()))
                        {
                            new_expression += '*';
                            break;
                        }
                        break;
                    case '√':
                        if (i > 0 && Enumerable.Range(0, 10).Select(x => x.ToString()).Contains(expression[i - 1].ToString()))
                        {
                            new_expression += "*";
                        }
                        new_expression += '√';
                        break;
                    case '%':
                        new_expression += "/100.00";
                        break;
                    case '÷':
                        new_expression += '/';
                        break;
                    case '×':
                        new_expression += '*';
                        break;
                    case 'π':
                        new_expression += Math.PI.ToString().Replace(',', '.');
                        if (i > 0 && Enumerable.Range(0, 10).Select(x => x.ToString()).Contains(expression[i - 1].ToString()))
                        {
                            new_expression = new_expression.Insert(new_expression.Length - Math.PI.ToString().Length, "*");
                            break;
                        }
                        if (Enumerable.Range(0, 10).Select(x => x.ToString()).Contains(expression[i + 1].ToString()))
                        {
                            new_expression += '*';
                            break;
                        }
                        break;
                    case 'e':
                        new_expression += Math.Exp(1).ToString().Replace(',', '.');
                        if (i > 0 && Enumerable.Range(0, 10).Select(x => x.ToString()).Contains(expression[i - 1].ToString()))
                        {
                            new_expression = new_expression.Insert(new_expression.Length - Math.Exp(1).ToString().Length, "*");
                            break;
                        }
                        if (Enumerable.Range(0, 10).Select(x => x.ToString()).Contains(expression[i + 1].ToString()))
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
        public double Factorial(int fact)
        {
            double result = 1;
            for (int i = 2; i <= fact; i++)
            {
                result *= i;
            }
            return result;
        }
        public string Fact_str(string expression)
        {
            try
            {
                while (expression.Contains('!'))
                {
                    int i = expression.IndexOf('!');
                    string exp = "";
                    string res = "";
                    if (expression[i - 1] == ')')
                    {
                        while (expression[i - 1] != '(')
                        {
                            exp = exp.Insert(0, expression[i - 1].ToString());
                            i--;
                        }
                        exp = exp.Insert(0, "(");
                        exp = Add_Fl(exp);
                        res = Evaluate(Change_Symbols(exp)).ToString().Replace(',', '.');
                        Exception ex = ExceptionFunctions.Ex_Int(res, "Факториал", 0, 170);
                        if (ex == null)
                        {
                            int fact = Convert.ToInt32(res);
                            expression = expression.Remove(i - 1, expression.IndexOf('!') + 2 - i);
                            expression = expression.Insert(i - 1, Factorial(fact).ToString().Replace(',', '.'));
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                    else
                    {
                        i--;
                        while (i >= 0 && (Enumerable.Range(0, 10).Select(x => x.ToString()).Contains(expression[i].ToString()) || expression[i] == '.'))
                        {
                            exp = exp.Insert(0, expression[i].ToString());
                            i--;
                        }
                        Exception ex = ExceptionFunctions.Ex_Int(exp, "Факториал", 0, 170);
                        if (ex == null)
                        {
                            int fact = Convert.ToInt32(exp);
                            expression = expression.Remove(i + 1, expression.IndexOf('!') - i);
                            expression = expression.Insert(i + 1, Factorial(fact).ToString().Replace(',', '.'));
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                }
                expression = Add_Fl(expression);
                return expression;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string Sqrt_str(string expression)
        {
            try
            {
                while (expression.Contains('√'))
                {
                    int i = expression.IndexOf('√');
                    int j;
                    int index = expression.IndexOf('√');
                    string exp = "";
                    string res = "";

                    while (expression[i] != ')')
                    {
                        i++;
                    }
                    j = i;
                    while (expression[i - 1] != '(')
                    {
                        exp = exp.Insert(0, expression[i - 1].ToString());
                        i--;
                    }
                    exp = exp.Insert(0, "(");
                    exp = exp.Insert(exp.Length, ") ");
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
                if (Enumerable.Range(0, 10).Select(x => x.ToString()).Contains(expression[i].ToString()))
                {
                    while (Enumerable.Range(0, 10).Select(x => x.ToString()).Contains(expression[i].ToString()))
                    {
                        i++;
                    }
                    if (expression[i] == 'E')
                    {
                        i += 2;
                        while (Enumerable.Range(0, 10).Select(x => x.ToString()).Contains(expression[i].ToString()))
                        {
                            i++;
                        }
                    }
                    else if (expression[i] == '.')
                    {
                        i++;
                        int j = 0;
                        while (Enumerable.Range(0, 10).Select(x => x.ToString()).Contains(expression[i].ToString()))
                        {
                            j++;
                            i++;
                        }
                        if (j < 2)
                        {
                            for (int e = 0; e < j; e++)
                            {
                                expression = expression.Insert(i, "0");
                            }
                        }
                    }
                    else if (i > 5 && expression[i - 5] != 'E' && expression[i - 4] != 'E' && expression[i - 3] != 'E')
                    {
                        expression = expression.Insert(i, ".00");
                        i += 2;
                    }
                    else if (expression[i] != '.' && expression[i] != '!' && i <= 5)
                    {
                        expression = expression.Insert(i, ".00");
                        i += 2;
                    }
                }
            }
            for (int i = 0; i < expression.Length; i++)
            {
                if (Enumerable.Range(0, 10).Select(x => x.ToString()).Contains(expression[i].ToString()))
                {
                    string res = "";
                    string ful_res = "";

                    while (Enumerable.Range(0, 10).Select(x => x.ToString()).Contains(expression[i].ToString()))
                    {
                        res += expression[i];
                        i++;
                    }
                    ful_res = res;
                    while (Enumerable.Range(0, 10).Select(x => x.ToString()).Contains(expression[i].ToString()) || expression[i] == '.')
                    {
                        ful_res += expression[i];
                        i++;
                    }
                    if (expression[i + 1] != 'E' && res.Length >= 17)
                    {
                        i -= ful_res.Length;
                        i += Convert.ToDouble(res.Replace('.', ',')).ToString().Length;

                        expression = expression.Replace(ful_res, Convert.ToDouble(res.Replace('.', ',')).ToString());
                    }
                }
            }
            return expression;
        }
        public void Calculate_Out(string expression)
        {
            try
            {
                expression += "  ";

                expression = Change_Symbols(expression);

                expression = Sqrt_str(expression);

                expression = Fact_str(expression);

                expression = Evaluate(Add_Fl(Change_Symbols(expression))).ToString().Replace(',', '.');
                MainTextBox.Text = expression;
            }
            catch (Exception ex)
            {
                MainTextBox.Text = ex.Message;
            }
        }
        public void MainTB(object sender, TextCompositionEventArgs args)
        {
            if (!(values.Contains(args.Text.ToString()) || args.Text == "E"))
            {
                args.Handled = true;
            }
            else if (MainTextBox.Text.Replace("×", "*").Replace("÷", "/").Split("+-*/".ToCharArray()).Last().Where(x => ".".Contains(x)).Count() == 1 && ".".Contains(args.Text))
            {
                args.Handled = true;
            }
        }
        public void ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string symbol = sender.ToString().Split(' ').Last();
                if (symbol == "CE")
                {
                    MainTextBox.Text = "";
                }
                else if (symbol == ".")
                {
                    if (!(MainTextBox.Text.Replace("×", "*").Replace("÷", "/").Split("+-*/".ToCharArray()).Last().Where(x => ".".Contains(x)).Count() == 1 && ".".Contains(".")))
                    {
                        MainTextBox.Text += ".";
                    }
                }
                else if (symbol == "√")
                {
                    MainTextBox.Text += "√(";
                }
                else if (symbol == "=")
                {
                    Calculate_Out(MainTextBox.Text);
                }
                else
                {
                    MainTextBox.Text += symbol;
                }
            }
            catch (Exception ex)
            {
                MainTextBox.Text = ex.Message;
            }
        }
        public void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { Calculate_Out(MainTextBox.Text); }
            else if (e.Key == Key.Escape) { Keyboard.ClearFocus(); }
           
        }
    }
}
