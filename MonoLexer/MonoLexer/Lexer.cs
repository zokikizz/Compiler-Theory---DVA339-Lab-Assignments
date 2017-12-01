using System;
using System.Text.RegularExpressions;

namespace Lexer
{
	public class Lexer
	{
		private string streamOfInputedChars;

		private int line;
		private int column;

		private string Id;
		private string num;
		private int countOfPassCharachters;

		private string space = " ";
		private string newLineChar = "\\r";
		private string newLineChar1 = "\\n";
		private string operatorPlus = "+";
		private string operatorEqual = ":=";
		private string keyWord = "print";
		private string separator = ";";
		private string separator1 = "(";
		private string separator2 = ")";
		private string separator3 = ",";

		internal Token buff;


		// temp for lexer
		private bool search = true;

		public Lexer(string text)
		{
			this.streamOfInputedChars = text;

			this.line = 1;
			this.column = 1;
			this.countOfPassCharachters = 0;
			this.buff = null;

			this.Id = string.Empty;
			this.num = string.Empty;
		}

		public Token Next()
		{
			Token t = Peek ();
			this.buff = null;
			return t;
		}

		public Token Peek ()
		{
			if (this.buff != null)
				return this.buff;

			// get new token and put it in this.buff
			while (this.search) {

				if (this.streamOfInputedChars.Length <= this.countOfPassCharachters) {
					if (this.checkForEndOfFileId () ) {
						if (this.checkForEndOfFileNum ()) {
							this.buff = new Token (Token.Type.EOF, "", this.line, this.column);
							this.search = false;
						}
					}
				} else {

					if (this.streamOfInputedChars [this.countOfPassCharachters].ToString().Equals (this.space)) {
						//space
						if (this.Check () && this.isThereNumber()) {
							this.column++;
							// this.countOfPassCharachters++;
						}
					} else if ( (this.countOfPassCharachters+1) < this.streamOfInputedChars.Length 
						&& this.streamOfInputedChars [countOfPassCharachters].ToString().Equals ("\r") && 
						this.streamOfInputedChars [countOfPassCharachters+1].ToString().Equals ("\n")) {
						if(this.Check() && this.isThereNumber())
							this.whenIsNewLine (("\r\n").Length);

					} else if (this.streamOfInputedChars [countOfPassCharachters].ToString().Equals ("\r")) {
						if(this.Check() && this.isThereNumber())
							this.whenIsNewLine (("\r").Length);

					}
					else if (this.streamOfInputedChars [countOfPassCharachters].ToString().Equals ("\n")) {
						if(this.Check() && this.isThereNumber())
							this.whenIsNewLine (("\n").Length);
						
					} else if (this.streamOfInputedChars [countOfPassCharachters].Equals ('\\')) { // won't work for \n \r\n and \r add it
						//new line
						if (Check () && this.isThereNumber()) {
							// \\r\\n
							if ( (this.countOfPassCharachters+3) < this.streamOfInputedChars.Length
								&& (this.streamOfInputedChars [countOfPassCharachters].ToString()
								+ this.streamOfInputedChars [countOfPassCharachters + 1].ToString()
								+ this.streamOfInputedChars [countOfPassCharachters + 2].ToString()
								+ this.streamOfInputedChars [countOfPassCharachters + 3].ToString()).Equals (
									this.newLineChar + this.newLineChar1) ) {
								this.whenIsNewLine ((this.newLineChar + this.newLineChar1).Length);
							} else if ((this.countOfPassCharachters+1) < this.streamOfInputedChars.Length
								&& (this.streamOfInputedChars [countOfPassCharachters].ToString()
								+ this.streamOfInputedChars [countOfPassCharachters + 1].ToString())
								.Equals (this.newLineChar)) {
								// \\r
								this.whenIsNewLine ((this.newLineChar1).Length);
							} else if ((this.countOfPassCharachters+1) < this.streamOfInputedChars.Length
								&& (this.streamOfInputedChars [countOfPassCharachters].ToString()
								+ this.streamOfInputedChars [countOfPassCharachters + 1].ToString())
								.Equals (this.newLineChar1)) {
								// \\n
								this.whenIsNewLine ((this.newLineChar1).Length);
							}
						}
					} else if (this.streamOfInputedChars [this.countOfPassCharachters].ToString()
						.Equals (this.operatorPlus)) {
						if (this.Check () && this.isThereNumber()) {
							// +
							this.buff = new Token (Token.Type.OP, this.operatorPlus, line, column);
							this.column += this.operatorPlus.Length;
							this.search = false;
						}
					} else if ( ( (this.countOfPassCharachters+1) < this.streamOfInputedChars.Length ) && 
						( this.streamOfInputedChars [this.countOfPassCharachters].ToString()
							+ this.streamOfInputedChars [this.countOfPassCharachters + 1].ToString()
						).Equals (this.operatorEqual)) {
						// :=
						if (this.Check () && isThereNumber()) {
							this.buff = new Token (Token.Type.OP, this.operatorEqual, line, column);
							this.column += this.operatorEqual.Length;
							this.countOfPassCharachters++;
							this.search = false;
						}
					} else if (this.streamOfInputedChars [this.countOfPassCharachters].ToString().Equals (this.separator)) {
						if (this.Check () && isThereNumber()) {
							this.buff = new Token (Token.Type.SEP, this.separator, line, column);
							this.column += this.separator.Length;
							this.search = false;
						}
					} else if (this.streamOfInputedChars [this.countOfPassCharachters].ToString().Equals (this.separator1)) {
						if (this.Check () && this.isThereNumber()) {
							this.buff = new Token (Token.Type.SEP, this.separator1, line, column);
							this.column += this.separator1.Length;
							this.search = false;
						}
					} else if (this.streamOfInputedChars [this.countOfPassCharachters].ToString().Equals (this.separator2)) {
						if (this.Check () && this.isThereNumber()) {
							this.buff = new Token (Token.Type.SEP, this.separator2, line, column);
							this.column += this.separator2.Length;
							this.search = false;
						}
					} else if (this.streamOfInputedChars [this.countOfPassCharachters].ToString().Equals (this.separator3)) {
						if (this.Check () && isThereNumber()) {
							this.buff = new Token (Token.Type.SEP, this.separator3, line, column);
							this.column += this.separator3.Length;
							this.search = false;
						}
					} else if ((Regex.Match (this.streamOfInputedChars [this.countOfPassCharachters].ToString (), @"\d")).Success) {
						if (this.CheckSpecificForNum ()) {
							this.num +=	this.streamOfInputedChars [this.countOfPassCharachters];
						} else {
							this.Id+=this.streamOfInputedChars [this.countOfPassCharachters];
						}
					} else {
						if (this.isThereNumber ()) {
							if(Check())
								this.Id += this.streamOfInputedChars [this.countOfPassCharachters];
						}
					}


					countOfPassCharachters++;
				}

			
			}

			this.search = true;

			return this.buff;
		}


		public bool Check() {
			

			if (this.Id.Length > 0 && 
				(this.streamOfInputedChars[this.countOfPassCharachters].ToString().Equals("\\") 
					|| this.streamOfInputedChars[this.countOfPassCharachters].ToString().Equals("\n")
					|| this.streamOfInputedChars[this.countOfPassCharachters].ToString().Equals("\r")
					||this.streamOfInputedChars[this.countOfPassCharachters].ToString().Equals(" ") 
					|| this.streamOfInputedChars[this.countOfPassCharachters].ToString().Equals("+")
					|| this.streamOfInputedChars[this.countOfPassCharachters].ToString().Equals(":") 
					|| this.streamOfInputedChars[this.countOfPassCharachters].ToString().Equals(this.separator)
					|| this.streamOfInputedChars[this.countOfPassCharachters].ToString().Equals(this.separator1)
					|| this.streamOfInputedChars[this.countOfPassCharachters].ToString().Equals(this.separator2)
					|| this.streamOfInputedChars[this.countOfPassCharachters].ToString().Equals(this.separator3))
			) {
				if (this.Id.Equals (this.keyWord)) {
					this.buff = new Token (Token.Type.KEYW, this.Id, this.line, this.column);
				} else {
					this.buff = new Token (Token.Type.ID, this.Id, this.line, this.column);
				}
				this.column += this.Id.Length;
				this.Id = string.Empty;
				this.countOfPassCharachters--;
				return this.search = false;
			}

			return true;
			
		}

		public bool CheckSpecificForNum() {

			if (this.Id.Length > 0)
			{
				return false;
			}

			return true;

		}


		public bool isThereNumber() {
			if (this.num.Length > 0) {
				this.buff = new Token (Token.Type.NUM, this.num, this.line, this.column);
				this.column += this.num.Length;
				this.num = string.Empty;
				this.countOfPassCharachters--;
				return this.search = false;
			}
			return true;
		}

		public void whenIsNewLine(int change){
			line++;
			column = 1;
			this.countOfPassCharachters += (change-1);
		}

		public bool checkForEndOfFileId() {
			if( this.Id.Length > 0) {
				if (this.Id.Equals (this.keyWord)) {
					this.buff = new Token (Token.Type.KEYW, this.Id, this.line, this.column);
				} else {
					this.buff = new Token (Token.Type.ID, this.Id, this.line, this.column);
				}
				this.column += this.Id.Length;
				this.Id = string.Empty;
				return this.search = false;
			}
			return true;
		}

		public bool checkForEndOfFileNum() {
			if (this.num.Length > 0) {
				this.buff = new Token (Token.Type.NUM, this.num, this.line, this.column);
				this.column += this.num.Length;
				this.num = string.Empty;
				return this.search = false;
			}
			return true;
		}

	}
}
