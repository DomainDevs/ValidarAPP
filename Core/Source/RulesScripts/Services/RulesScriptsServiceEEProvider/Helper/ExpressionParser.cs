using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider
{
	/// <summary>
	/// Summary description for ExpressionParser.
	/// </summary>
	public class ExpressionParser
	{
		public class Expression
		{
			public virtual void GetExpressionString(StringBuilder sb)
			{
			}

		}

		public class Number :
			Expression
		{
			public string Value;

			public Number(string value)
			{
				this.Value = value;
			}

			public override void GetExpressionString(StringBuilder sb)
			{
				sb.Append(this.Value);
			}

		}

		public class Operator :	Expression
		{
			public string Op;
			public Expression LeftExpression;
			public Expression RightExpression;

			public Operator(string op, Expression left, Expression right)
			{
				this.Op = op;
				this.LeftExpression = left;
				this.RightExpression = right;
			}

			public override void GetExpressionString(StringBuilder sb)
			{
				sb.Append("( ");
				this.LeftExpression.GetExpressionString(sb);
				sb.Append(" ");
				sb.Append(this.Op);
				sb.Append(" ");
				this.RightExpression.GetExpressionString(sb);
				sb.Append(" )");
			}

		}

		public class Variable :
			Expression
		{
			public string Name;

			public Variable(string name)
			{
				this.Name = name;
			}

			public override void GetExpressionString(StringBuilder sb)
			{
				sb.Append("[");
				sb.Append(this.Name);
				sb.Append("]");
			}

		}

		private int _parenthesisCounter = 0;
		private ListDictionary _parenthesis = new ListDictionary();
		private Regex _re = new Regex(@"(<op1>\d+|\w+)(<op2>\s+(<op3>[-\+\*/])\s+(<op4>\d+|\w+))*", RegexOptions.Multiline | RegexOptions.ExplicitCapture);

		public ExpressionParser()
		{
			string number = @"(\d+)((?<!\d{4,})(,\d{3}))*(\.\d+)?";
			string concept = @"[A-Za-z_](\w|\d)+";
			string concept2 = @"\[(?>.+?((?<!\\)\]))";
			string @operator = @"[-\+\*\/]";
			string operand = number + "|" + concept + "|" + concept2;

			string rexp = @"^(\s*(?<op1>" + operand + @"))(\s*(?<op>" + @operator + @")\s*(?<op2>" + operand + @"))*\s*$";
			this._re = new Regex(rexp, RegexOptions.ExplicitCapture);
		}

		private string GetNewParenthesisName()
		{
			this._parenthesisCounter++;
			return "__par_" + this._parenthesisCounter.ToString();
		}

		public Expression Parse(string exp)
		{
			int to;
			string s = this.ReplaceParenthesis(" " + exp, 0, out to);
			Expression e = this.ParseExpression(s);

			e = this.ReplaceParenthesis(e);

			return e;
		}

		private Expression ReplaceParenthesis(Expression exp)
		{
			if(exp is Variable)
			{
				string name = ((Variable)exp).Name;
				if(name.StartsWith("__par_"))
				{
					return this.ReplaceParenthesis((Expression)this._parenthesis[name]);
				}

				return exp;
			}

			if(exp is Operator)
			{
				Operator op = (Operator)exp;
				op.LeftExpression = this.ReplaceParenthesis(op.LeftExpression);
				op.RightExpression = this.ReplaceParenthesis(op.RightExpression);

				return op;
			}

			return exp;
		}

		private string ReplaceParenthesis(string s, int from, out int to)
		{
			char c;
			StringBuilder sb = new StringBuilder();
			int last;
			if(s[from] == '(')
			{
				last = from + 1;
			}
			else
			{
				last = from;
			}

			int p = last;
			bool inCor = false;
			while(p < s.Length && ((c = s[p]) != ')' || inCor))
			{
				if(c == '(')
				{
					if(!inCor)
					{
						sb.Append(s.Substring(last, p - last));
						sb.Append(this.ReplaceParenthesis(s, p, out p));

						last = p + 1;
					}
				}
				else if(c == '[')
				{
					inCor = true;
				}
				else if(c == ']')
				{
					if(p > 0 && s[p - 1] != '\\')
					{
						inCor = false;
					}
				}

				p++;
			}

			if(p >= s.Length && s[from] == '(')
			{
				throw new Exception("Se esperaba ')'.");
			}

			sb.Append(s.Substring(last, p - last));

			string res = sb.ToString().Trim();
			if(res.Equals(string.Empty))
			{
				to = p;
				return string.Empty;
			}

			Expression exp = this.ParseExpression(res);
			string name = this.GetNewParenthesisName();
			this._parenthesis.Add(name, exp);
			to = p;

			return name;
		}

		private Expression ParseExpression(string s)
		{
			Match m = this._re.Match(s);
			if(!m.Success)
			{
				throw new Exception("Invalid expression.");
			}

			Group g = m.Groups["op1"];
			string val = g.Value;

			Expression exp;
			if(val.StartsWith("["))
			{
				exp = new Variable(val.Substring(1, val.Length - 2).Replace("\\]", "]"));
			}
			else if(!char.IsDigit(val[0]))
			{
				exp = new Variable(val);
			}
			else
			{
				exp = new Number(val);
			}

			g = m.Groups["op2"];
			if(g.Success)
			{
				Group opg = m.Groups["op"];
				int i = 0;
				foreach(Capture c in g.Captures)
				{
					Expression right;
					val = c.Value;
					if(val.StartsWith("["))
					{
						right = new Variable(val.Substring(1, val.Length - 2).Replace("\\]", "]"));
					}
					else if(!char.IsDigit(val[0]))
					{
						right = new Variable(val);
					}
					else
					{
						right = new Number(val);
					}

					exp = new Operator(opg.Captures[i].Value, exp, right);
					i++;
				}
			}

			return exp;
		}
	}
}
