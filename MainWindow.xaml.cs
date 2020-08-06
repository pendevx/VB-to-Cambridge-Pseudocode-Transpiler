using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace VB_to_Cambridge_Pseudocode_Transpiler
{
	public partial class MainWindow : Window
	{
		private string filePath;
		public MainWindow()
		{
			InitializeComponent();
			Title = "Untitled - VB.Net to Pseudocode Transpiler";
			WindowState = WindowState.Maximized;
			WindowStyle = WindowStyle.SingleBorderWindow;
		}
		private void transpileButton_Click(object sender, RoutedEventArgs e) => outputTextbox.Text = Parse(inputTextbox.Text);
		public string Parse(string input)
		{
			// declarations
			string previousItem = "";
			bool previousIsSelect = false;
			bool equalsInCondition = false;
			bool inQuotationOrComment = false;
			bool inOutput = false;
			int i = 0;
			string output = "";

			// where magic happens
			while (i < input.Length)
			{
				int startIndex = i;
				for (; i < input.Length; i++)
				{
					if (char.IsWhiteSpace(input[i]))
						break;
				}

				// getting word to be evaluated
				string word = input.Substring(startIndex, i - startIndex);

				// checking when the word starts and ends with quotation
				if (word == "\"")
					inQuotationOrComment = !inQuotationOrComment;

				// selecting keywords to convert
				if (!inQuotationOrComment)
				{
					switch (word.ToLower())
					{
						#region conditional statements

						case "if":
							equalsInCondition = true;
							word = word.ToUpper();
							break;

						case "while":
							if (previousItem != "END")
								equalsInCondition = true;
							word = word.ToUpper();
							break;

						case "do":
							word = "REPEAT";
							break;
						case "until":
							word = word.ToUpper();
							equalsInCondition = true;
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
						case "or":
						case "and":
						case "not":
						case "mod":
						case "loop":
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
						int iParen = word.IndexOf("(");
						word = $"OUTPUT {word.Substring(iParen + 1)}";
						inOutput = true;
					}
					if (word.ToLower().EndsWith(")") && inOutput == true)
						word = word.Substring(0, word.Length - 1);
					if (word.StartsWith("'"))
					{
						inQuotationOrComment = true;
						string comment = word.Substring(1, word.Length - 1);
						word = $"//{comment}";
					}
				}

				previousItem = word;
				output += word;

				if (i < input.Length)
					output += input[i];

				while (++i < input.Length && char.IsWhiteSpace(input[i]))
				{
					if (input[i] == '\n')
					{
						inQuotationOrComment = false;
						inOutput = false;
					}

					output += input[i];
				}
			}

			return output;
		}
		private void fileNew_Click(object sender, RoutedEventArgs e)
		{
			outputTextbox.Clear();
			inputTextbox.Clear();
			filePath = "";
			this.Title = "Untitled - VB.Net to Pseudocode Transpiler";
		}
		private void fileOpen_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Title = "Open file";
			openFileDialog.Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*|Visual Basic Project (*.vb)|*.vb*";
			openFileDialog.Multiselect = false;

			if (openFileDialog.ShowDialog() == true)
			{
				filePath = openFileDialog.FileName;
				inputTextbox.Text = File.ReadAllText(filePath);
				Title = $"{filePath} - VB.Net to Pseudocode Transpiler";
			}
		}
		private void fileSave_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			if (filePath == "" || filePath == null)
			{
				saveFileDialog.Title = "Save as";
				saveFileDialog.Filter = "Text File (*.txt)|*.txt";

				if (saveFileDialog.ShowDialog() == true)
				{
					filePath = saveFileDialog.FileName;
					File.WriteAllText(filePath, outputTextbox.Text);
					Title = $"{filePath} - VB.Net to Pseudocode Transpiler";
				}
				else
					return;
			}
			else
				File.WriteAllText(filePath, outputTextbox.Text);
		}
		private void fileSaveAs_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();

			saveFileDialog.Title = "Save as";
			saveFileDialog.Filter = "Text File (*.txt)|*.txt";

			if (saveFileDialog.ShowDialog() == true)
			{
				filePath = saveFileDialog.FileName;
				File.WriteAllText(filePath, outputTextbox.Text);
				Title = $"{filePath} - VB.Net to Pseudocode Transpiler";
			}
			else
				return;
		}
	}
}
