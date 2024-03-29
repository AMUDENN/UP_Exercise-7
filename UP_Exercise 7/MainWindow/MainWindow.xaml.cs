﻿using System;
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
        public static string[] values = { "(", ")", "%", "←", "CE", "(x)!", "7", "8", "9", "÷", "√(x)", "4", "5", "6", "×", "π", "1", "2", "3", "-", "e", "0", ".", "=", "+" };
        public static string[] numbers = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        public MainWindow()
        {
            InitializeComponent();
            SetButtons();
            ChangeSelStart();
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
                if (!"0123456789*/+-.()E".Contains(args.Text))
                {
                    args.Handled = true;
                }
                else if (MainTextBox.Text.Split("×÷+-()".ToCharArray()).Last().Count(x => ".".Contains(x)) == 1 && ".".Contains(args.Text))
                {
                    args.Handled = true;
                }
                if (args.Text == "*")
                {
                    MainTextBox.Text = MainTextBox.Text.Trim('*');
                    MainTextBox.Text += "×";
                    args.Handled = true;
                }
                if (args.Text == "/")
                {
                    MainTextBox.Text = MainTextBox.Text.Trim('/');
                    MainTextBox.Text += "÷";
                    args.Handled = true;
                }
            }
            else
            {
                MainTextBox.Text += args.Text;
                args.Handled = true;
            }
            ChangeSelStart();
        }
        public void ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string symbol = ((Button)sender).Content.ToString();
                if (symbol == "CE")
                {
                    HistoryTextBox.Clear();
                    MainTextBox.Clear();
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
                    if (!(MainTextBox.Text.Split("×÷+-()".ToCharArray()).Last().Count(x => ".".Contains(x)) == 1))
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
                    if (MainTextBox.Text.Length > 0 && !"%×÷+-(".Contains(MainTextBox.Text[MainTextBox.Text.Length - 1]))
                    {
                        MainTextBox.Text += "×";
                    }
                    MainTextBox.Text += "(";
                }
                else if (symbol == "√(x)")
                {
                    if (MainTextBox.Text.Length > 0 && !"(%×÷+-".Contains(MainTextBox.Text[MainTextBox.Text.Length - 1]))
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
                else if (symbol == "(x)!")
                {
                    if (MainTextBox.Text.Length == 0 || "%×÷+-".Contains(MainTextBox.Text[MainTextBox.Text.Length - 1]))
                    {
                        MainTextBox.Text += "(0)!";
                    }
                    else if (MainTextBox.Text[MainTextBox.Text.Length - 1] == '!')
                    {
                        int i = MainTextBox.Text.IndexOf('!') - 1;
                        string exp = "";

                        int count_f = 0;
                        int count_l = 1;
                        while (count_f != count_l)
                        {
                            i--;
                            if (MainTextBox.Text[i] == ')')
                            {
                                count_l++;
                            }
                            if (MainTextBox.Text[i] == '(')
                            {
                                count_f++;
                            }
                            exp = exp.Insert(0, MainTextBox.Text[i].ToString());
                        }
                        exp += ")!)!";
                        exp = exp.Insert(0, "(");
                        MainTextBox.Text = MainTextBox.Text.Remove(i, MainTextBox.Text.IndexOf('!') - i + 1);
                        MainTextBox.Text = MainTextBox.Text.Insert(i, exp);
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
                        while (i > 0 && !"+-×÷()√".Contains(MainTextBox.Text[i]))
                        {
                            if (numbers.Contains(MainTextBox.Text[i].ToString()) || "πe".Contains(MainTextBox.Text[i]) || MainTextBox.Text[i] == '.')
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
                        if (i == 0 && !"(+-".Contains(MainTextBox.Text[i]))
                        {
                            MainTextBox.Text = MainTextBox.Text.Insert(i, "(");
                        }
                        else if (MainTextBox.Text[i] != '(')
                        {
                            i += MainTextBox.Text.Length - 1 == i ? 0 : 1;
                            MainTextBox.Text = MainTextBox.Text.Insert(i, "(");
                        }
                        MainTextBox.Text += ")!";
                    }
                }
                else if (symbol == "=")
                {
                    if (MainTextBox.Text.Length > 0)
                    {
                        HistoryTextBox.Text = MainTextBox.Text + " =";
                        MainTextBox.Text = Calculating.Calculate_Out(MainTextBox.Text);  
                    }
                }
                else if (numbers.Contains(symbol))
                {
                    if (MainTextBox.Text.Length > 0 && MainTextBox.Text[MainTextBox.Text.Length - 1] == '%')
                    {
                        MainTextBox.Text += '×';
                    }
                    MainTextBox.Text += symbol;
                }
                else
                {
                    MainTextBox.Text += symbol;
                }
                ChangeSelStart();
            }
            catch (Exception ex)
            {
                MainTextBox.Text = ex.Message;
            }
        }
        public void TBKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { 
                HistoryTextBox.Text = MainTextBox.Text + " ="; 
                MainTextBox.Text = Calculating.Calculate_Out(MainTextBox.Text);
                e.Handled = true; 
            }
            if (e.Key == Key.Space) { e.Handled = true; }
        }
        public void ChangeSelStart()
        {
            MainTextBox.Focus();
            MainTextBox.SelectionStart = MainTextBox.Text.Length;
        }
    }
}
