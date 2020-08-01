using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VB_to_Cambridge_Pseudocode_Transpiler
{
	class Parser
	{
		public Parser(string word)
		{
			this.input = word;
		}
		private readonly string input;
		public string Parse()
		{
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
						case "if":
						case "while":
							equalsInCondition = true;
							word = word.ToUpper();
							break;

						case "then":
							equalsInCondition = false;
							word = word.ToUpper();
							break;

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

					}
				}

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
			}

			return output;
		}
	}
}
