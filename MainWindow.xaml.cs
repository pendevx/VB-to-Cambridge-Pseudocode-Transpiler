using System.Windows;

namespace VB_to_Cambridge_Pseudocode_Transpiler
{
	public partial class MainWindow : Window
	{
		private string filePath;

		public MainWindow()
		{
			InitializeComponent();
			this.Title = "Untitled - VB.Net to Pseudocode Transpiler";
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
			bool inQuotationOrComment = false;
			int i = 0;
			string output = "";

			while (i < input.Length)
			{
				int startIndex = i;
				for (; i < input.Length; i++)
				{
					if (char.IsWhiteSpace(input[i]))
						break;
				}

				string word = input.Substring(startIndex, i - startIndex);

				//if (word == "\n")
				//	inQuotationOrComment = false;

				if (word == "\"")
					inQuotationOrComment = !inQuotationOrComment;

				if (!inQuotationOrComment)
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
						int iParen = word.IndexOf("(") + 1;
						word = $"OUTPUT {word.Substring(iParen, word.LastIndexOf(")") - iParen)}";
					}
					if (word.StartsWith("'"))
					{
						inQuotationOrComment = true;
						string comment = word.Substring(1);
						word = $"// {comment}";
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
					if (input[i] == '\n')
						inQuotationOrComment = false;
					output += input[i];
					goto label;
				}

				#endregion

			}

			return output;
		}
		private void fileNew_Click(object sender, RoutedEventArgs e)
		{
			outputTextbox.Clear();
			inputTextbox.Clear();
			this.Title = "Untitled - VB.Net to Pseudocode Transpiler";
		}
	}
}
