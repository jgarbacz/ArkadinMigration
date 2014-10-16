grammar MetraScript;

// NOTE: if the parser hangs when invoked from MVM, make sure that Antlr generated regular non-debug code
// (e.g. using the "Generate Code" command, not the debug button in the Antlr GUI)

//options {
//	output=AST;
//	backtrack=true;
//	memoize=true;
//}

options {
	language=CSharp2;
	output=AST;
	backtrack=true;
	memoize=true;
}


// Tokens we using in building the AST
tokens {
	INT;
	FLOAT;
	STRING;
	BOOL;
	NULL;
	CURRENT_OBJECT;
	OBJECT;
	TEMP;
	GLOBAL;
	PROC;
	THREAD;
	FUNCTION;
	ARGUMENT;
	NAMED_ARGUMENT;
}

////////////// rob make parse errors fatal

// UNCOMMENT FOR Java TARGET //
/*
@lexer::members {
	public void MissingClosingError(String msg,int startLine,int startPosition,String startText,String endText){
    		throw new RuntimeException(msg);
    	}
}
@members {
	Stack paraphrases = new Stack();
	public void PushPassphrase(String phrase){
		paraphrases.push(phrase);
	}
	public void PopPassphrase(){
		paraphrases.pop();
	}
}
*/

// UNCOMMENT FOR C# TARGET //

@lexer::members {
    override public void Recover(RecognitionException re)
    {
        throw re;
    }
    override public void ReportError(RecognitionException e)
    {
        var antlrMsg = base.GetErrorMessage(e, TokenNames);
        var stack = GetRuleInvocationStack(e, this.GetType().FullName);
        throw new MvmScript.MvmScriptRecognitionException(e, stack, antlrMsg);
    }
    public void MissingClosingError(string msg,int startLine,int startPosition,string startText,string endText){
    	throw new MvmScript.MvmScriptMissingClosingException(msg,startLine,startPosition,startText,endText);
    }
}
@header {
	using ParserExensionsNameSpace;
}
@members {
	Stack paraphrases = new Stack();
	public void PushPassphrase(String phrase){
		paraphrases.Push(phrase);
	}
	public void PopPassphrase(){
		paraphrases.Pop();
	}
   override public String GetErrorMessage(RecognitionException e, String[] tokenNames)
        {
            var stack = GetRuleInvocationStack(e, this.GetType().FullName);
            //foreach (var x in stack) Console.WriteLine(x);
            var antlrMsg = base.GetErrorMessage(e, tokenNames);
            string paraphrase=" ";
            if (paraphrases.Count > 0)
            {
                paraphrase += (string)paraphrases.Peek();
            }
            if (e is MismatchedTokenException)
            {
                var mte = e as MismatchedTokenException;
                if (e.Line > 0)
                {
                    antlrMsg = "Expecting " + tokenNames[mte.Expecting] + paraphrase +" but found '" + mte.Token.Text + "' at line="+e.Line+", position="+(e.CharPositionInLine+1);
                }
                else
                {
                    antlrMsg = "Expecting " + tokenNames[mte.Expecting] + paraphrase + " but reached end-of-file";
                }
            }
            else if (e is NoViableAltException)
            {
                var nvae = e as NoViableAltException;
                if (e.Line > 0)
                {
                    antlrMsg = "Not expecting '" + nvae.Token.Text + "' " + paraphrase + " at line=" + e.Line + ", position=" + (e.CharPositionInLine + 1);
                }
                else
                {
                    if (stack.Count == 1 && stack[0].ToString().Equals("terminator"))
                    {
                        antlrMsg = "Reached end-of-file, are you missing a final semicolon?";
                    }
                }
            }
            else
            {
                antlrMsg += paraphrase + " at line=" + e.Line + ", position=" + (e.CharPositionInLine + 1);
            }
            throw new MvmScript.MvmScriptRecognitionException(e, stack, antlrMsg);
        }

    override public String GetTokenErrorDisplay(IToken t)
    {
        return t.ToString();
    }
    
    override protected void Mismatch(IIntStream input, int ttype, BitSet follow)
    {
        throw new MismatchedTokenException(ttype, input);
    }
   
    override public object RecoverFromMismatchedSet(IIntStream input, RecognitionException e, BitSet follow)
    {
        throw e;
    }
    public bool TruePrint(string msg){
    	Console.WriteLine(msg);
    	return true;
    }
}


////////////// ENDrob make parse errors fatal

// uncomment when generating csharp
//@header {
//using ParserExensionsNameSpace;
//}


////////////////////////////  PARSER /////////////////////////

start	:
	expression
	;  

variable
	: 'OBJECT.' Id  -> ^(OBJECT CURRENT_OBJECT Id)
	| 'OBJECT(' expression ').' Id  -> ^(OBJECT expression Id)
	| 'TEMP.' Id  -> ^(TEMP Id)
	| 'GLOBAL.' Id  -> ^(GLOBAL Id)
	| 'THREAD.' Id  -> ^(THREAD Id)
	| 'PROC.' Id  -> ^(PROC Id)
	;

literal 
	: integerLiteral -> ^(INT integerLiteral)
	| FloatingPointLiteral-> ^(FLOAT FloatingPointLiteral)
	| StringLiteral-> ^(STRING StringLiteral)
	| booleanLiteral -> ^(BOOL booleanLiteral)
	| 'null' -> ^(NULL)
	;
integerLiteral
	: HexLiteral
	| OctalLiteral
	| DecimalLiteral
	;
booleanLiteral
	: 'true'
	| 'false'
	;

expression
	: conditionalExpression (assignmentOperator^ expression)?
	;
assignmentOperator
	: '='
	| '~'
	| '+='
	| '-='
	| '*='
	| '/='
	| '&='
	| '|='
	| '^='
	| '%='
	| '~='
	| ('<' '<' '=')=> t1='<' t2='<' t3='=' 
	  { $t1.getLine() == $t2.getLine() &&
	  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() && 
	  $t2.getLine() == $t3.getLine() && 
	  $t2.getCharPositionInLine() + 1 == $t3.getCharPositionInLine() }?
	| ('>' '>' '>' '=')=> t1='>' t2='>' t3='>' t4='='
	  { $t1.getLine() == $t2.getLine() && 
	  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() &&
	  $t2.getLine() == $t3.getLine() && 
	  $t2.getCharPositionInLine() + 1 == $t3.getCharPositionInLine() &&
	  $t3.getLine() == $t4.getLine() && 
	  $t3.getCharPositionInLine() + 1 == $t4.getCharPositionInLine() }?
	| ('>' '>' '=')=> t1='>' t2='>' t3='='
	  { $t1.getLine() == $t2.getLine() && 
	  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() && 
	  $t2.getLine() == $t3.getLine() && 
	  $t2.getCharPositionInLine() + 1 == $t3.getCharPositionInLine() }?
	;
conditionalExpression
	: conditionalOrExpression ( '?'^ expression ':'! expression )?
	;
conditionalOrExpression
	: conditionalAndExpression ( ('||'|'or'|'OR')^ conditionalAndExpression )*
	//| conditionalAndExpression ( 'or'^ conditionalAndExpression )*
	;
conditionalAndExpression
	: inclusiveOrExpression ( ('&&'|'and'|'AND')^ inclusiveOrExpression )*
	//| inclusiveOrExpression ( 'and'^ inclusiveOrExpression )*
	;
inclusiveOrExpression
	: exclusiveOrExpression ( '|'^ exclusiveOrExpression )*
	;
exclusiveOrExpression
	: andExpression ( '^'^ andExpression )*
	;
andExpression
	: equalityExpression ( '&'^ equalityExpression )*
	;
equalityExpression
	: instanceOfExpression ( ('==' | '!=' | 'eq' | 'ne'|'Eq'|'Ne'|'EQ'|'NE'|'eqEQ'|'EqEQ'|'neNE'|'NeNE')^ instanceOfExpression )*
	;
instanceOfExpression
	: relationalExpression 
	;
relationalExpression
	: shiftExpression ( relationalOp^ shiftExpression )*
	;
relationalOp
	: ('<' '=')=> t1='<' t2='=' 
	  { $t1.getLine() == $t2.getLine() && 
	  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() }?
	| ('>' '=')=> t1='>' t2='=' 
	  { $t1.getLine() == $t2.getLine() && 
	  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() }?
	| '<' 
	| '>' 
	| 'gt'
	| 'lt'
	//| 'eq'
	| 'gte'
	| 'lte'
	| 'Gt'
	| 'Lt'
	//| 'Eq'
	| 'Gte'
	| 'Lte'
	| 'GT'
	| 'LT'
	//| 'EQ'
	| 'GTE'
	| 'LTE'
	;
shiftExpression
	: additiveExpression ( shiftOp^ additiveExpression )*
	;
shiftOp
	: ('<' '<')=> t1='<' t2='<' 
	  { $t1.getLine() == $t2.getLine() && 
	  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() }?
	| ('>' '>' '>')=> t1='>' t2='>' t3='>' 
	  { $t1.getLine() == $t2.getLine() && 
	  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() &&
	  $t2.getLine() == $t3.getLine() && 
	  $t2.getCharPositionInLine() + 1 == $t3.getCharPositionInLine() }?
	| ('>' '>')=> t1='>' t2='>'
	  { $t1.getLine() == $t2.getLine() && 
	  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() }?
	;
additiveExpression
	: multiplicativeExpression ( ('+' | '-')^ multiplicativeExpression )*
	;
multiplicativeExpression
	: unaryExpression ( ( '*' | '/' | '%' )^ unaryExpression )*
	;
unaryExpression
	: '+'^ unaryExpression
	| '-'^ unaryExpression
	| '++'^ unaryExpression
	| '--'^ unaryExpression
	| unaryExpressionNotPlusMinus
	;
unaryExpressionNotPlusMinus
	: '~'^ unaryExpression
	| '!'^ unaryExpression
	| castExpression
	| primary ('++'|'--')^  
	| primary
	;
castExpression
	:  '(' Id ')' unaryExpression
	|  '(' Id ')' unaryExpressionNotPlusMinus
	;
	
parExpression
	: '(' expression ')' -> expression
	;
unit	
	: literal
	| variable
	| function
	;	
primary
	: parExpression
	| unit
	;

argument
	: Id '=>' expression -> ^(NAMED_ARGUMENT Id expression)
	| expression -> ^(ARGUMENT expression)
	;

argument_list
	: argument (',' argument)* -> argument+
	;

function
	: Id '(' ')' -> ^(FUNCTION Id)
	| Id '(' argument_list ')' -> ^(FUNCTION Id argument_list)
	;

////////////////////////////  LEXER /////////////////////////


Id
	: ('a'..'z'|'A'..'Z'|'_') ('a'..'z'|'A'..'Z'|'0'..'9'|'_')*
	;
HexLiteral 
	: '0' ('x'|'X') HexDigit+ IntegerTypeSuffix?
	;
DecimalLiteral 
	:
	('0' | '1'..'9' '0'..'9'*) IntegerTypeSuffix? 
	;
OctalLiteral 
	: '0' ('0'..'7')+ IntegerTypeSuffix? 
	;
fragment
HexDigit 
 	: ('0'..'9'|'a'..'f'|'A'..'F') 
 	;
fragment
IntegerTypeSuffix 
	: ('l'|'L')
	;
FloatingPointLiteral
	: ('0'..'9')+ '.' ('0'..'9')* Exponent? FloatTypeSuffix?
	| '.' ('0'..'9')+ Exponent? FloatTypeSuffix?
	| ('0'..'9')+ Exponent FloatTypeSuffix?
	| ('0'..'9')+ FloatTypeSuffix
	;
fragment
Exponent 
	: ('e'|'E') ('+'|'-')? ('0'..'9')+ 
	;
fragment
FloatTypeSuffix 
	: ('f'|'F'|'d'|'D') 
	;
	
StringLiteral
: '"' ( ~('"') )* '"'
| '\'' ( ~('\'') )* '\''
;
	
// COMMENTED THIS OUT WHEN WE STOPPED SUPPORTING ESCAPES INSIDE QUOTES
//
//StringLiteral
//	: '"' ( EscapeSequence | ~('\\'|'"') )* '"'
//	| '\'' ( EscapeSequence | ~('\\'|'\'') )* '\''
//	;
//fragment
//EscapeSequence
//	: '\\' ('b'|'t'|'n'|'f'|'r'|'\"'|'\''|'\\')
//	| UnicodeEscape
//	| OctalEscape
//	;
//fragment
//OctalEscape
//	: '\\' ('0'..'3') ('0'..'7') ('0'..'7')
//	| '\\' ('0'..'7') ('0'..'7')
//	| '\\' ('0'..'7')
//	;
//fragment
//UnicodeEscape
//	: '\\' 'u' HexDigit HexDigit HexDigit HexDigit
//	;



WS  
	:  (' '|'\r'|'\t'|'\u000C'|'\n') {$channel=HIDDEN;}
	;
COMMENT
	: '/*' ( options {greedy=false;} : . )* '*/' {$channel=HIDDEN;}
	;
LINE_COMMENT
	: '//' ~('\n'|'\r')* '\r'? '\n' {$channel=HIDDEN;}
	;
