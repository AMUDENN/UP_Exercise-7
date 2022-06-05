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
        public static BrushConverter converter = new System.Windows.Media.BrushConverter();
        public static string[] values = { "(", ")", "%", "←", "CE", "!", "7", "8", "9", "÷", "√", "4", "5", "6", "×", "π", "1", "2", "3", "-", "e", "0", ".", "=", "+" };
        public static string[] numbers = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        public MainWindow()
        {
            InitializeComponent();
            SetButtons();
            Calculating.ErrorMessage += ShowError;
        }
        public void ShowError(CalculateEventArgs e)
        {
            MessageBox.Show(e.Message);
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
                button.Background = numbers.Contains(value) || value == "." ? (Brush)converter.ConvertFromString("#f1f3f4") : value == "=" ? (Brush)converter.ConvertFromString("#4285f4") : (Brush)converter.ConvertFromString("#dadce0");
                MainUniformGrid.Children.Add(button);
            }
        }
        public void TextChangedMainTB(object sender, TextCompositionEventArgs args)
        {
            if (MainTextBox.SelectionStart == MainTextBox.Text.Length)
            {
                if (!("0123456789*/+-.()".Contains(args.Text) || args.Text == "E"))
                {
                    args.Handled = true;
                }
                else if (MainTextBox.Text.Split("×÷+-()".ToCharArray()).Last().Where(x => ".".Contains(x)).Count() == 1 && ".".Contains(args.Text))
                {
                    args.Handled = true;
                }
                if (args.Text == "*")
                {
                    MainTextBox.Text = MainTextBox.Text.Trim('*');
                    MainTextBox.Text += "×";
                    MainTextBox.Focus();
                    MainTextBox.SelectionStart = MainTextBox.Text.Length;
                    args.Handled = true;
                }
                if (args.Text == "/")
                {
                    MainTextBox.Text = MainTextBox.Text.Trim('/');
                    MainTextBox.Text += "÷";
                    MainTextBox.Focus();
                    MainTextBox.SelectionStart = MainTextBox.Text.Length;
                    args.Handled = true;
                }
            }
            else
            {
                MainTextBox.Focus();
                MainTextBox.SelectionStart = MainTextBox.Text.Length;
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
                else if (MainTextBox.Text.Length > 0 && MainTextBox.Text[MainTextBox.Text.Length - 1] == ')' && (numbers.Contains(symbol) || symbol == "√"))
                {
                    MainTextBox.Text += "×";
                    MainTextBox.Text += symbol;
                    if (symbol == "√")
                    {
                        MainTextBox.Text += "(";
                    }
                }
                else if (symbol == ".")
                {
                    if (!(MainTextBox.Text.Split("×÷+-()".ToCharArray()).Last().Where(x => ".".Contains(x)).Count() == 1 && ".".Contains(".")))
                    {
                        if (MainTextBox.Text.Length == 0 || MainTextBox.Text.Length > 0 && !numbers.Contains(MainTextBox.Text[MainTextBox.Text.Length - 1].ToString()))
                        {
                            MainTextBox.Text += "0";
                        }
                        MainTextBox.Text += ".";
                    }
                }
                else if (symbol == "(")
                {
                    if(MainTextBox.Text.Length > 0 && !"×÷+-(".Contains(MainTextBox.Text[MainTextBox.Text.Length - 1]))
                    {
                        MainTextBox.Text += "×";
                    }
                    MainTextBox.Text += "(";
                }
                else if (symbol == "√")
                {
                    if (MainTextBox.Text.Length > 0 && !"×÷+-".Contains(MainTextBox.Text[MainTextBox.Text.Length - 1]))
                    {
                        MainTextBox.Text += "×";
                    }
                    MainTextBox.Text += "√(";
                }
                else if (symbol == "←")
                {
                    if (MainTextBox.Text.Length > 0)
                    {
                        MainTextBox.Text = MainTextBox.Text.Substring(0, MainTextBox.Text.Length - 1);
                    }
                }
                else if (symbol == "!")
                {
                    if (MainTextBox.Text.Length == 0 || "+-×÷".Contains(MainTextBox.Text[MainTextBox.Text.Length - 1]))
                    {
                        MainTextBox.Text += "(0)!";
                    }
                    else if (MainTextBox.Text[MainTextBox.Text.Length - 1] == '(')
                    {
                        if (MainTextBox.Text.Length >= 2 && MainTextBox.Text[MainTextBox.Text.Length - 2] == '√')
                        {
                            MainTextBox.Text = MainTextBox.Text.Insert(MainTextBox.Text.Length - 2, "(");
                            MainTextBox.Text += "0))!";
                        }
                        else
                        {
                            MainTextBox.Text += "0)!";
                        }
                    }
                    else if (MainTextBox.Text[MainTextBox.Text.Length - 1] == ')')
                    {
                        MainTextBox.Text += "!";
                    }
                    else 
                    {
                        int i = MainTextBox.Text.Length - 1;
                        while (i > 0 && !"+-×÷()√^".Contains(MainTextBox.Text[i]))
                        {
                            if (numbers.Contains(MainTextBox.Text[i].ToString()) || MainTextBox.Text[i] == '.')
                            {
                                i--;
                            }
                            if ("+-".Contains(MainTextBox.Text[i].ToString()) && i > 0)
                            {
                                if (MainTextBox.Text[i - 1].ToString() == "E")
                                {
                                    i -= 2;
                                }
                            }
                        }
                        if (i == 0)
                        {
                            MainTextBox.Text = MainTextBox.Text.Insert(i, "(");
                        }
                        else
                        {
                            i += MainTextBox.Text.Length - 1 == i ? 0 : 1;
                            MainTextBox.Text = MainTextBox.Text.Insert(i, "(");
                        }
                        MainTextBox.Text += ")!";
                    }
                }
                else if (symbol == "=")
                {
                    if(MainTextBox.Text.Length > 0)
                    {
                       MainTextBox.Text = Calculating.Calculate_Out(MainTextBox.Text);
                    }
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
        public void TBKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { MainTextBox.Text = Calculating.Calculate_Out(MainTextBox.Text); }
            if (e.Key == Key.Escape) { Keyboard.ClearFocus(); }
            if (e.Key == Key.Space) { e.Handled = true; }
        }
    }
}
