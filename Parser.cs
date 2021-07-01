namespace spo
{
	public class Parser
	{
		string _input;
		Token token;

		public Parser(string input)
		{
			_input = input;
		}

		Token GetToken()
		{
			return token = Lexer.GetToken();
		}

		public bool Parse()
		{
			Lexer.input = _input;

			GetToken();
			return Goal();
		}

		bool Goal()
		{
			return Vars() && Instructions();
		}

		bool Vars()
		{
			if (token.Value == "var")
			{
				GetToken();
				if (List_Vars())
					return true;
			}
			return false;
		}

		bool List_Vars()
		{
			if (ID(token)
				&& token.Value == ":"
				&& GetToken().Type == Lex_type.DataType)
			{
				GetToken();
				if (List_Vars1())
					return true;
			}

			return false;
		}

		bool List_Vars1()
		{
			if (ID(token))
			{
				if (token.Value == ":")
					if(GetToken().Type == Lex_type.DataType)
				{
					GetToken();
					if (List_Vars1())
					{
						return true;
					}
				}
				return false;
			}
			else
				return true;
		}

		bool Instructions (Token _token = null)
		{
			if (token.Value == "begin")
			{
				GetToken();
				if (List_Instructions() && token.Value == "end")
					return true;
			}
			return false;
		}

		bool List_Instructions()
		{
			if (token.Type == Lex_type.Identifier)
			{
				GetToken();
				return Assign();
			}
			else if (token.Value == "read")
			{
				GetToken();
				return Read();
			}
			else if (token.Value == "write")
			{
				GetToken();
				return Write();
			}
			else if (token.Value == "If")
			{
				GetToken();
				return If();
			}
			else return true;
		}

		bool Assign()
		{
			if (token.Type == Lex_type.Assign && Expr(GetToken()))
			{
				return List_Instructions();
			}
			else
				return false;
		}

		bool Expr(Token _token)
		{
			return Term(_token) && Expr1();
		}

		bool Expr1()
		{
			if (token.Value == "+")
			{
				return Expr(GetToken());
			}
			else if (token.Value == "-")
			{
				return Expr(GetToken());
			}
			else
				return true;
		}

		bool Term(Token _token)
		{
			return Factor(_token) && Term1(token);
		}

		bool Term1(Token _token)
		{
			if (_token.Value == "*")
			{
				return Term(GetToken());
			}
			else if (_token.Value == "/")
			{
				return Term(GetToken());
			}
			else
				return true;
		}

		bool Factor(Token _token)
		{
			if (_token.Type == Lex_type.Identifier || _token.Type == Lex_type.Number)
			{
				GetToken();
				return true;
			}
			else if (_token.Value == "round")
			{
				if (GetToken().Value == "(" && Exp(GetToken()) && GetToken().Value == ")")
				{
					GetToken();
					return true;
				}
			}
			else if (_token.Value == "(")
			{
				if (Expr(GetToken()) && token.Value == ")")
				{
					GetToken();
					return true;
				}
			}
			return false;
		}

		bool Exp(Token _token)
		{
			return _token.Type == Lex_type.Identifier || _token.Type == Lex_type.Number;
		}

		bool Read()
		{
			if (token.Value == "(" && ID(GetToken()) && token.Value == ")")
			{
				GetToken();
				return List_Instructions();
			}
			return false;
		}

		bool Write()
		{
			if (token.Value == "(" && Write1(GetToken()) && token.Value == ")")
			{
				GetToken();
				return List_Instructions();
			}
			else
				return false;
		}

		bool Write1(Token _token)
		{
			return ID(_token) || String(_token);
		}

		bool If()
		{
			if (token.Value == "(" && Condition(GetToken()) && GetToken().Value == ")" && GetToken().Value == "Then" && Instructions(GetToken()) && Else(GetToken()))
			{
				return List_Instructions();
			}
			return false;
		}

		bool Else(Token _token)
		{
			if (_token.Value == "Else")
			{
				if (Instructions(GetToken()))
				{
					GetToken();
					return true;
				}
				else
					return false;
			}
			else
				return true;
		}

		bool Condition(Token _token)
		{
			if (token.Value == "(")
			{
				return Condition(GetToken()) && GetToken().Value == ")" && Slm(GetToken()) && Sc(GetToken());
			}
			else if (Exp(token) && Reo(GetToken()))
			{
				return true;
			}
			return false;
		}

		bool Sc(Token _token)
		{
			if (token.Value == "or")
			{
				return Condition(GetToken());
			}
			else
				return true;
		}

		bool Conj()
		{
			return Lm() && Slm(GetToken());
		}

		bool Slm(Token _token)
		{
			if (token.Value == "and")
			{
				GetToken();
				return Conj();
			}
			else
				return true;
		}

		bool Lm()
		{
			return token.Value == "(" && Condition(GetToken()) && GetToken().Value == ")";
		}

		bool Reo(Token _token)
		{
			if (token.Value == "<"
				|| token.Value == ">"
				|| token.Value == "=")
			{
				return Exp(GetToken());
			}
			return false;
		}


		bool ID(Token _token)
		{
			if (_token.Type == Lex_type.Identifier)
			{
				GetToken();
				return true;
			}
			return false;
		}

		bool String(Token _token)
		{
			if (_token.Type == Lex_type.String)
			{
				GetToken();
				return true;
			}
			return false;
		}
	}
}
