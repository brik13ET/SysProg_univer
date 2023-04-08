using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SysProg_univer
{
	internal class Lexer
	{
		private class Transition
		{
			readonly public State From;
			readonly public State To;
			readonly public Char On;

			public Transition(State from, State to, char on)
			{
				From = from;
				To = to;
				On = on;
			}
		}

		private Transition findTransition(char ch, State from)
		{
			foreach (Transition tr in transitions)
			{
				bool correspond_type = false;
				if (tr.On == 'V') correspond_type = Char.IsLetter(ch) || ch == '_' || ch == '$' || Char.IsDigit(ch);
				else if (tr.On == 'v') correspond_type = Char.IsLetter(ch) || ch == '_' || ch == '$';
				else if (tr.On == 'D') correspond_type = Char.IsDigit(ch);
				else correspond_type = tr.On == ch;
				if (tr.From == from && correspond_type)
					return tr;
			}
			return null;
		}

		enum State
		{
			Start,
			do_1,
			do_2,
			w2,
			cb_body1,
			w3,
			stmt_beg,
			stmt_end,
			w4,
			op_assign1,
			op_sub_assign,
			op_add_assign,
			w5,
			imm_conts1,
			var_name2,
			w6,
			cb_body2,
			op_add,
			op_sub,
			while1,
			while2,
			while3,
			while4,
			while5,
			w8,
			b_expr1,
			var_name3,
			imm_const2,
			w7,
			compr_less,
			compr_more,
			compr_neq1,
			compr_eq1,
			compr_eq2,
			b_expr2,
			end
		}

		// V - [a-zA-Z0-9_$]
		// v - [a-zA-Z_$]
		// D - [0-9]
		// ' ' - [\t\n\r\s]

		readonly Transition[] transitions = new Transition[]
			{
				new Transition(State.Start, State.Start,' '),
				new Transition(State.Start, State.do_1,'d'),
				new Transition(State.do_1, State.do_2,'o'),
				new Transition(State.do_2, State.w2,' '),
				new Transition(State.w2, State.w2,' '),
				new Transition(State.w2, State.cb_body1,'{'),
				new Transition(State.do_2, State.cb_body1,'{'),
				new Transition(State.cb_body1, State.w3,' '),
				new Transition(State.w3, State.w3,' '),
				new Transition(State.w3, State.stmt_beg,'v'),
				new Transition(State.cb_body1, State.stmt_beg,'v'),
				new Transition(State.stmt_beg, State.stmt_beg,'V'),
				new Transition(State.stmt_beg, State.op_assign1,'='),
				new Transition(State.stmt_beg, State.op_add_assign,'+'),
				new Transition(State.stmt_beg, State.op_sub_assign,'-'),
				new Transition(State.stmt_beg, State.w4,' '),
				new Transition(State.stmt_end, State.stmt_beg,'v'),
				new Transition(State.w4, State.op_assign1,'='),
				new Transition(State.w4, State.op_add_assign,'+'),
				new Transition(State.w4, State.op_sub_assign,'-'),
				new Transition(State.w4, State.w4,' '),
				new Transition(State.op_assign1, State.w5,' '),
				new Transition(State.op_sub_assign, State.op_assign1,'='),
				new Transition(State.op_add_assign, State.op_assign1,'='),
				new Transition(State.w5, State.w5,' '),
				new Transition(State.w5, State.imm_conts1,'D'),
				new Transition(State.w5, State.var_name2,'v'),
				new Transition(State.op_assign1, State.imm_conts1,'D'),
				new Transition(State.op_assign1, State.var_name2,'v'),
				new Transition(State.imm_conts1, State.imm_conts1,'D'),
				new Transition(State.var_name2, State.var_name2,'V'),
				new Transition(State.imm_conts1, State.w6,' '),
				new Transition(State.imm_conts1, State.stmt_end,';'),
				new Transition(State.var_name2, State.w6,' '),
				new Transition(State.w6, State.w6,' '),
				new Transition(State.w6, State.op_sub,'-'),
				new Transition(State.w6, State.op_add,'+'),
				new Transition(State.imm_conts1, State.op_sub,'-'),
				new Transition(State.imm_conts1, State.op_add,'+'),
				new Transition(State.var_name2, State.op_sub,'-'),
				new Transition(State.var_name2, State.op_add,'+'),
				new Transition(State.var_name2, State.stmt_end,';'),
				new Transition(State.w6, State.stmt_end,';'),
				new Transition(State.stmt_end, State.stmt_end,' '),
				new Transition(State.stmt_end, State.cb_body2,'}'),
				new Transition(State.cb_body2, State.cb_body2,' '),
				new Transition(State.cb_body2, State.while1, 'w'),
				new Transition(State.op_add, State.w5,' '),
				new Transition(State.op_sub, State.w5,' '),
				new Transition(State.op_add, State.imm_conts1,'D'),
				new Transition(State.op_sub, State.imm_conts1,'D'),
				new Transition(State.op_add, State.var_name2,'V'),
				new Transition(State.op_sub, State.var_name2,'V'),
				new Transition(State.while1, State.while2,'h'),
				new Transition(State.while2, State.while3,'i'),
				new Transition(State.while3, State.while4,'l'),
				new Transition(State.while4, State.while5,'e'),
				new Transition(State.while5, State.w8,' '),
				new Transition(State.w8, State.w8,' '),
				new Transition(State.w8, State.b_expr1,'('),
				new Transition(State.while5, State.b_expr1,'('),
				new Transition(State.b_expr1, State.var_name3,'v'),
				new Transition(State.b_expr1, State.imm_const2,'D'),
				new Transition(State.var_name3, State.var_name3,'V'),
				new Transition(State.imm_const2, State.imm_const2,'D'),
				new Transition(State.b_expr1, State.b_expr1,' '),
				new Transition(State.var_name3, State.w7,' '),
				new Transition(State.imm_const2, State.w7,' '),
				new Transition(State.w7, State.w7,' '),
				new Transition(State.w7, State.compr_less,'<'),
				new Transition(State.w7, State.compr_more,'>'),
				new Transition(State.w7, State.compr_neq1,'!'),
				new Transition(State.w7, State.compr_eq1,'='),
				new Transition(State.w7, State.b_expr2,')'),
				new Transition(State.var_name3, State.compr_less,'<'),
				new Transition(State.var_name3, State.compr_neq1,'!'),
				new Transition(State.var_name3, State.compr_eq1,'='),
				new Transition(State.var_name3, State.b_expr2 ,' '),
				new Transition(State.imm_const2, State.compr_more,'>'),
				new Transition(State.imm_const2, State.compr_neq1,'!'),
				new Transition(State.imm_const2, State.compr_eq1,'='),
				new Transition(State.compr_less, State.compr_less,' '),
				new Transition(State.compr_more, State.compr_more,' '),
				new Transition(State.compr_neq1, State.compr_eq2,'='),
				new Transition(State.compr_eq1, State.compr_eq2,'='),
				new Transition(State.compr_less, State.var_name3,'v'),
				new Transition(State.compr_more, State.var_name3,'v'),
				new Transition(State.compr_eq2, State.var_name3,'v'),
				new Transition(State.compr_less, State.imm_const2,'D'),
				new Transition(State.compr_more, State.imm_const2,'D'),
				new Transition(State.compr_eq2, State.imm_const2,'D'),
				new Transition(State.compr_eq2, State.compr_eq2,' '),
				new Transition(State.b_expr2, State.b_expr2,' '),
				new Transition(State.b_expr2, State.end,';'),
				new Transition(State.end, State.end,' ')
			};

		private readonly List<string> vars = new List<string>();

		public Lexer()
		{
		}

		internal string[] get_depen_vars(string var_name)
		{
			throw new NotImplementedException();
		}

		internal string[] get_variable_names()
		{
			return vars.ToArray();
		}

		private string msgs;

		internal void analyze(string text)
		{
			vars.Clear();
			vars.Add("");
			msgs = "";
			State current = State.Start;
			int linenum = 0;
			int symb = 0;
			string line_view = "";
			int i = 0;
			while (i < text.Length && text[i] != '\n')
			{
				line_view += text[i];
				i++;
			}
			for (i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (c == '\n')
				{
					linenum++;
					symb = 0;
					line_view = "";
					int j = i + 1;
					while (j < text.Length && text[i] != '\n')
					{
						line_view += text[j];
						j++;
					}
					c = ' ';
				}
				if (c == '\t' || c == '\r')
					c = ' ';

				symb++;
				Transition tr =  findTransition(c, current);
				if (tr == null)
				{
					msgs = 
						String.Format(
							"Bad Syntax {3} get `{4}` at {0}:{1}\t`{2}`",
							linenum, symb, line_view,
							current.ToString(), c);
					break;
				}
				current = tr.To;
				if (current == State.var_name2 || current == State.var_name3 || current == State.stmt_beg)
				{
					vars[vars.Count - 1] = vars[vars.Count - 1] + c;
				}
				else if (vars[vars.Count - 1].Length != 0)
				{
					for (int j = 0; j < vars.Count - 1; j++)
					{
						if ( vars[j].Equals(vars[vars.Count-1]) )
							vars.RemoveAt(j);
					}
					vars.Add("");
				}
			}
			if (current == State.end)
				msgs += "Syntax OK";
			else
				msgs +=
						String.Format(
							"Bad Syntax {3} get EOF at {0}:{1}\t`{2}`",
							linenum, symb, line_view,
							current.ToString());

		}

		internal string result()
		{
			string ret = msgs + "\n\n";
			foreach (string variable in vars)
			{
				ret += variable + '\n';
			}
			return ret;
		}
	}
}
