using System;
using System.Linq;
using System.Windows;

namespace VB_to_Cambridge_Pseudocode_Transpiler
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            this.WindowStyle = WindowStyle.SingleBorderWindow;
        }
        private void transpileButton_Click(object sender, RoutedEventArgs e)
        {
            string input = inputTextbox.Text;
            outputTextbox.Text = Parse(input);
        }
        public string Parse(string input)
        {
            string previousItem = "";
            bool previousIsSelect = false;
            bool equalsInCondition = false;
            bool inQuotations = false;
            int i = 0;
            string output = "";

            while (i < input.Length)
            {
                int startIndex = i;
                for (; i < input.Length; i++)
                    if (char.IsWhiteSpace(input[i]))
                        break;

                string word = input.Substring(startIndex, i - startIndex);

                if (word == "\"")
                    inQuotations = !inQuotations;

                if (!inQuotations)
                {
                    switch (word.ToLower())
                    {
                        #region if, while, then <needs condition some evaluation>

                        case "if":
                            equalsInCondition = true;
                            word = word.ToUpper();
                            break;

                        case "while":
                            if (previousItem == "END")
                            {
                                word = word.ToUpper();
                            }
                            else
                            {
                                equalsInCondition = true;
                                word = word.ToUpper();
                            }
                            break;

                        case "then":
                            equalsInCondition = false;
                            word = word.ToUpper();
                            break;

                        #endregion

                        #region return toupper

                        case "end":
                        case "else":
                        case "for":
                        case "to":
                        case "next":
                        case "do":
                        case "or":
                        case "and":
                        case "not":
                        case "mod":
                            word = word.ToUpper();
                            break;

                        #endregion

                        #region select case

                        case "select":
                            previousIsSelect = true;
                            word = "CASE";
                            break;

                        case "case":
                            if (previousIsSelect)
                            {
                                word = "";
                                previousIsSelect = false;
                            }
                            else
                                word = "OF";
                            equalsInCondition = false;
                            break;

                        #endregion

                        #region conditional and arithmetic operators

                        case "%":
                            word = "MOD"; break;

                        case "=":
                            if (equalsInCondition)
                            {
                                equalsInCondition = false;
                                break;
                            }
                            else
                                word = "<-";
                            break;

                        case "<>":
                            word = "IS NOT";
                            break;

                        case "+=":
                            word = $"<- {previousItem} +";
                            break;

                        #endregion

                        #region other

                        case "(":
                        case ")":
                        case "dim":
                        case "integer":
                        case "string":
                        case "boolean":
                        case "byte":
                        case "char":
                        case "decimal":
                        case "long":
                        case "short":
                        case "single":
                            word = "";
                            break;

                        case "as":
                            word = "<- 0";
                            break;

                        case "console.readline":
                        case "console.readline()":
                            word = "INPUT";
                            break;

                            #endregion
                    }
                    if (word.ToLower().StartsWith("console.write"))
                    {
                        var text = word.Substring(word.IndexOf("(") + 1);
                        text = text.TrimEnd(')');
                        word = $"OUTPUT {text}";
                    }
                }

                previousItem = word;

                #region combining

                output += word;

                if (i < input.Length)
                    output += input[i];

                label:
                if (++i >= input.Length)
                    break;
                else if (char.IsWhiteSpace(input[i]))
                {
                    output += input[i];
                    goto label;
                }

                #endregion

            }

            output.Replace("  ", " ");
            output.Trim();

            return output;
        }
    }
}// INPUT and OUTPUT from cw and cr
