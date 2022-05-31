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
        public MainWindow()
        {
            InitializeComponent();
            SetButtons();
        }
        public void SetButtons()
        {
            string[] values = { "!", "(", ")", "%", "CE", "√", "7", "8", "9", "÷", "^", "4", "5", "6", "×", "π", "1", "2", "3", "-", "e", "0", ".", "=", "+" };
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
        public string Change_Symbols(string expression)
        {
            string new_expression = "";
            for (int i = 0; i < expression.Length; i++)
            {
                switch (expression[i])
                {
                    case '÷':
                        new_expression += '/';
                        break;
                    case '×':
                        new_expression += '*';
                        break;
                    case 'π':
                        new_expression += Math.PI.ToString().Replace(',', '.');
                        break;
                    case 'e':
                        new_expression += Math.Exp(1).ToString().Replace(',', '.');
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
        public string Add_Fl(string expression)
        {
            for (int i = 0; i < expression.Length; i++)
            {
                if(Enumerable.Range(0, 10).Select(x => x.ToString()).Contains(expression[i].ToString()))
                {
                    while(Enumerable.Range(0, 10).Select(x => x.ToString()).Contains(expression[i].ToString()))
                    {
                        i++;
                    }
                    if(expression[i] != '.')
                    {
                        expression = expression.Insert(i, ".00");
                        i += 2;
                    }
                }
            }
            return expression;
        }
        public void Calculate_Out(string expression)
        {
            expression += " ";
            string result = Change_Symbols(expression);
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
                        while (Enumerable.Range(0, 10).Select(x => x.ToString()).Contains(expression[i].ToString()) || expression[i] == '.')
                        {
                            exp = exp.Insert(0, expression[i].ToString());
                            i--;
                            if (i < 0)
                            {
                                break;
                            }
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
                //expression = Add_Fl(expression);
                result = Evaluate(Change_Symbols(expression)).ToString().Replace(',', '.');
                MainTextBox.Text = result;
            }
            catch (Exception ex)
            {
                MainTextBox.Text = ex.Message;
            }
        }
        public void ButtonClick(object sender, RoutedEventArgs e)
        {
            string symbol = sender.ToString().Split(' ').Last();
            if (symbol == "CE")
            {
                MainTextBox.Text = "";
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
        public void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { Calculate_Out(MainTextBox.Text); }
            else if (e.Key == Key.Escape) { Keyboard.ClearFocus(); }
           
        }
    }
}
