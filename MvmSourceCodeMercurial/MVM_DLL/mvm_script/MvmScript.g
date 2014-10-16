grammar MvmScript;

// UNCOMMENT FOR JAVA TARGET //
/*
options {  
    output=AST;
}
*/


// UNCOMMENT FOR C# TARGET //

options {
	language=CSharp2;
	output=AST;
}



tokens {
	// Since we cannot easily build our tree the way we want to on the parsing
	// pass. we resolve primary/secondary after the fact.
	Ast_Primary;
	Ast_Secondary;
	// There are two types of AstNodes: Ast_Element and Ast_Value.
	// These nodes are optionally named by a parent Ast_NodeNamer
	// Ast_Elements have an Ast_ElementName and child node under
	// either or both Ast_Attributes and Ast_children
	Ast_NodeNamer;
		// 'nodeName' (the node name goes in between the Ast_NodeNamer and the Ast_Element antlr trees
				Ast_Element;
					Ast_ElementName;
					Ast_Dot;
					Ast_Parameters;
					Ast_TypeParameters;
					Ast_Children;
					Ast_Brace;
					Ast_Bracket;
				Ast_Value;
				
	// These tokens represent literal values in the AST
	Syn_LiteralInt;
	Syn_LiteralFloat;
	Syn_literalString;
	Syn_LiteralBool;
	Syn_LiteralNull;
	
	// support array initializers
	Syn_Array;
	
	// These tokens support 3P object and arrays construction
	Syn_NewClassInst;
	Syn_DataType;
	Syn_Lvalue;
	Syn_Initializer;
	Syn_IsArray;
	Syn_TypeArgs;
	Syn_Args;
	
	// These tokens support syntactic sugar
	Syn_Proc;
		Syn_ProcName;
		Syn_ProcArguments;
		Syn_ProcReturns;
		Syn_ProcArgType;
		Syn_ProcArgMode;
	Syn_If;
		Syn_IfCondition;
		Syn_IfThen;
		Syn_IfElse;
	Syn_While;
		Syn_WhileCondition;
	Syn_DoWhile;
		Syn_DoWhileCondition;
	Syn_Block;
	Syn_For;
		Syn_ForInitialize;
		Syn_ForCondition;
		Syn_ForStep;
	Syn_Foreach;
		Syn_ForeachItem;
		Syn_ForeachList;
	Syn_Label;
	Syn_PreIncrement;
	Syn_PreDecrement;
	Syn_PostIncrement;
	Syn_PostDecrement;
	Syn_Try;
	Syn_StaticType;
}

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


start	
	: statements
	;
	
///////////////////////////////////////////////////////
//	Expression Section
///////////////////////////////////////////////////////

expression_alias
	:expression
	;

node_name
	: Id
	| StringLiteral
	;
	
arrayExpression
	: x='[' expression_list ']'
	->^(Ast_Element ^(Ast_ElementName Syn_Array[$x,"array"]) 
		^(Ast_Parameters  
			expression_list
		)
	)
	;

expression
@init { PushPassphrase("in expression"); }
@after { PopPassphrase(); }
	// new x.y.z(a,b,c)
	: new_object
	// array initializer (suitable as an lvalue or an rvalue)
	// [a,b,c]=['A','B','C']
	|(aa=arrayExpression->$aa)
                (    (assignmentOp)=>assignmentOp b=expression_alias
                      -> ^(Ast_Element ^(Ast_ElementName assignmentOp) ^(Ast_Parameters $expression $b))  
                )*
		
	// named_block=>{x;y;}
	|(node_name '=>' '{') => node_name '=>' x='{' statement* '}'
	->^(Ast_NodeNamer 
		^(node_name 
			^(Ast_Element ^(Ast_ElementName Syn_Block[$x,"brace"]) 
				^(Ast_Brace  
					statement*
				)
			)
		)
	)
	// named_expression=>'abc'
	|(node_name '=>') => node_name '=>' expression
	->^(Ast_NodeNamer 
		^(node_name expression)
	)
	// some_element{x;y;}
	//| (Id '{') => Id braces
	//->^(Ast_Primary 
	//	^(Ast_Element ^(Ast_ElementName Id))
	//)
	//^(Ast_Secondary braces)
	// anonymous block
	| ('{') => compound_statement
	// unnamed exression
	|(a=conditionalExpression->$a)
                (    (assignmentOp)=>assignmentOp b=expression_alias
                      -> ^(Ast_Element ^(Ast_ElementName assignmentOp) ^(Ast_Parameters $expression $b))  
                )*
	;
assignmentOp
	: '='
	| '+='
	| '-='
	| '*='
	| '/='
	| '&='
	| '|='
	| '^='
	| '%='
	| '~='
	| '<<='
	| '>' '>' '=' //must break into multiple tokens to support generics types
	;


conditionalExpression
	: conditionalOrExpression ( '?'^ expression ':'! expression )?
	;
conditionalOrExpression
	:	(a=conditionalAndExpression->$a) 
                (    conditionalOrOp b=conditionalAndExpression
                     -> ^(Ast_Element ^(Ast_ElementName conditionalOrOp) ^(Ast_Parameters $conditionalOrExpression $b))
                )*
	;
conditionalOrOp
	:'||'
	|'or'
	|'OR'
	;
conditionalAndExpression
	:	(a=inclusiveOrExpression->$a) 
                (    conditionalAndOp b=inclusiveOrExpression
                     -> ^(Ast_Element ^(Ast_ElementName conditionalAndOp) ^(Ast_Parameters $conditionalAndExpression $b))
                )*
	;
conditionalAndOp
	:'&&'
	|'and'
	|'AND'
	;
inclusiveOrExpression
	:	(a=exclusiveOrExpression->$a) 
                (    '|' b=exclusiveOrExpression
                     -> ^(Ast_Element ^(Ast_ElementName '|') ^(Ast_Parameters $inclusiveOrExpression $b))
                )*
	;
exclusiveOrExpression
	:	(a=andExpression->$a) 
                (    '^' b=andExpression
                     -> ^(Ast_Element ^(Ast_ElementName '^') ^(Ast_Parameters $exclusiveOrExpression $b))
                )*
	;
andExpression
	:	(a=equalityExpression->$a) 
                (    '&' b=equalityExpression
                     -> ^(Ast_Element ^(Ast_ElementName '&') ^(Ast_Parameters $andExpression $b))
                )*
	;
equalityExpression
	:	(a=instanceOfExpression->$a)
                (    equalityOp b=instanceOfExpression
                     -> ^(Ast_Element ^(Ast_ElementName equalityOp) ^(Ast_Parameters $equalityExpression $b))
                )*
	;
equalityOp
	:'==' 
	|'!=' 
	|'eq' 
	|'ne'
	|'Eq'
	|'Ne'
	|'EQ'
	|'NE'
	|'eqEQ'
	|'EqEQ'
	|'neNE'
	|'NeNE';
instanceOfExpression
	: relationalExpression 
	;
	
// had to do it this way because antlr would not allow different rules
relationalIsAsOp
	: relationalOp
	| 'is'
	| 'as'
	;
	
relationalExpression
	: (a=shiftExpression->$a)
                (    
                     	('is'|'as')=>relationalIsAsOp bb=datatype
                     		-> ^(Ast_Element ^(Ast_ElementName relationalIsAsOp) ^(Ast_Parameters $relationalExpression $bb))
                     	|
                   	relationalIsAsOp b=shiftExpression
                     		-> ^(Ast_Element ^(Ast_ElementName relationalIsAsOp) ^(Ast_Parameters $relationalExpression $b))
             )*
	;
relationalOp
	: '<='
	| '>='
	| '<' 
	| '>' 
	| 'gt'
	| 'lt'
	| 'gte'
	| 'lte'
	| 'Gt'
	| 'Lt'
	| 'Gte'
	| 'Lte'
	| 'GT'
	| 'LT'
	| 'GTE'
	| 'LTE'
	;
shiftExpression
	:	(a=additiveExpression->$a) 
                (    shiftOp b=additiveExpression
                     -> ^(Ast_Element ^(Ast_ElementName shiftOp) ^(Ast_Parameters $shiftExpression $b)) 
                )*
	;
shiftOp
	: ('<<'|'>' '>') // intensionally as separate tokens for generic type support
	;
additiveExpression
	: (a=multiplicativeExpression->$a)
                (    additiveOp b=multiplicativeExpression
                      -> ^(Ast_Element ^(Ast_ElementName additiveOp) ^(Ast_Parameters $additiveExpression $b))
                )*
	;
additiveOp
	:'+'
	|'-'
	|'~' // put here b/c perl does it that way, consider moving it
	;
multiplicativeExpression
	: (a=arrowExpression->$a)
                (    multiplicativeOp b=arrowExpression
                     -> ^(Ast_Element ^(Ast_ElementName multiplicativeOp) ^(Ast_Parameters $multiplicativeExpression $b))
                )*
	;
multiplicativeOp
	: '*' 
	| '/' 
	| '%'
	;
arrowExpression
	: (a=cast_expression->$a)
                (    '->' b=cast_expression
                     -> ^(Ast_Element ^(Ast_ElementName '->') ^(Ast_Parameters $arrowExpression $b))
                )*
	;
cast_expression
	: unary_expression //support cast w/ 'as' not (cast) so skip rule
	;
unary_expression
	: postfix_expression
	| x='++' unary_expression ->^(Ast_Element ^(Ast_ElementName Syn_PreIncrement[$x,"pre_increment"]) ^(Ast_Parameters  unary_expression))
	| x='--' unary_expression ->^(Ast_Element ^(Ast_ElementName Syn_PreDecrement[$x,"pre_decrement"]) ^(Ast_Parameters  unary_expression))
	| unary_operator cast_expression
	;
postfix_expression
	: primary_expression
	;
unary_operator
	: '&'
	| '*'
	| '+'
	| '-'
	| '~'
	| '!'
	;	
parExpression
	: '(' expression ')' -> expression
	;
elementAttributesList
	: expression_list 
	;
elementChildrenList
	: expression_list 
	;	
primary_expression 
	:  primary_expression_start  primary_expression_part* 
	->^(Ast_Primary primary_expression_start  primary_expression_part*)
	;
primary_expression_start   
	: identifier->^(Ast_Element ^(Ast_ElementName identifier))
	| paren_expression
	| literal
	;
primary_expression_part
	 : dot_id
	 | brackets
	 | arguments
	 | post_incr
	 | post_decr
	 ;
post_incr
	: x='++' -> ^(Ast_Dot[$x,"Ast_Dot"] ^(Ast_Element ^(Ast_ElementName Syn_PostIncrement[$x]) ) )
	;
post_decr
	: x='--' -> ^(Ast_Dot[$x,"Ast_Dot"] ^(Ast_Element ^(Ast_ElementName Syn_PostDecrement[$x]) ) )
	;
dot_id	
	: x='.' identifier-> ^(Ast_Dot[$x,"Ast_Dot"] ^(Ast_Element ^(Ast_ElementName identifier) ) )
 	;
braces	
	:x='{'  statements? '}'-> ^(Ast_Brace[$x,"Ast_Brace"] statements? ) 
	;
brackets
	:x='[' expression_list? ']' 
	-> ^(Ast_Bracket[$x,"Ast_Bracket"] 
		^(Ast_Element ^(Ast_ElementName $x)
			^(Ast_Parameters[$x,"Ast_Parameters"] expression_list? ) 
		)
	)
	;
arguments
	: x='(' expression_list?   ')' -> ^(Ast_Parameters[$x,"Ast_Parameters"] expression_list? )
	;
paren_expression
	:'(' expression ')' -> expression
	;
expression_list
	:expression  (','   expression)* ->expression+
	;
	
	
// T h i r d   P a r t y   O b j e c t s

new_object
	:x='new' datatypeInst
	-> ^(Ast_Element ^(Ast_ElementName Syn_NewClassInst[$x]) 
    		^(Ast_Parameters
    			datatypeInst
    		)
    	)
	;
datatypeInst
	:  namespace_or_type_name brackets* arguments?
	->^(Ast_Primary namespace_or_type_name brackets* arguments?)
	;
datatype
	:  namespace_or_type_name brackets*
	->^(Ast_Primary namespace_or_type_name brackets*)
	;
namespace_or_type_name:
	 datatype_start ('.'!  type_or_generic)* 
	 ;
type_or_generic
	:(identifier '<') => identifier typeArguments 
		->^(Ast_Dot["Ast_Dot"] 
			^(Ast_Element ^(Ast_ElementName identifier) ) 
		) 
		typeArguments
	| identifier  
		->^(Ast_Dot["Ast_Dot"] ^(Ast_Element ^(Ast_ElementName identifier) ) )
	;
datatype_start   
	: (identifier '<') => identifier typeArguments 
		->^(Ast_Element ^(Ast_ElementName identifier))
		typeArguments
	| identifier  
		->^(Ast_Element ^(Ast_ElementName identifier))
	;
typeArguments 
    	: x='<' datatype (',' datatype  )* '>'->^(Ast_TypeParameters datatype+)
    	;
        
///////////////////////////////////////////////////////
//	Statements Section
///////////////////////////////////////////////////////

statement
@init
{
 PushPassphrase("in statement"); 
 //int startPos = input.CharPositionInLine+1;
 //int startLine = input.Line;
}
@after { PopPassphrase(); }

	: (Id ':')=>labeled_statement
	| ('{')=>compound_statement
	| proc_statement
	| selection_statement
	| iteration_statement
	| jump_statement
	| try_block
	| expression_statement
	;
expression_statement
	: ';'!
	| expression terminator
	;
terminator
	: ('{')=>braces ->^(Ast_Secondary braces)
	| (';')=>';'! 
	;
labeled_statement
	: (Id ':')=>x=Id ':' statement
	-> ^(Ast_NodeNamer ^(Syn_Label[$x] statement))
	;
compound_statement
	: x='{' statement* '}' 
	-> ^(Ast_Element ^(Ast_ElementName Syn_Block[$x,"brace"]) 
		^(Ast_Brace  
			statement*
		)
	)
	;
selection_statement
	: x='if' '(' ifcond=expression ')' thenexp=body_statement (('else')=>'else' elseexp=body_statement)?
	-> ^(Ast_Element ^(Ast_ElementName Syn_If[$x])
		^(Ast_Parameters  
			^(Ast_Element ^(Ast_ElementName Syn_IfCondition["condition"])
				^(Ast_Parameters  
					$ifcond
				)
			)
			^(Ast_Element ^(Ast_ElementName Syn_IfCondition["then"])
				$thenexp
			)
			^(Ast_Element ^(Ast_ElementName Syn_IfCondition["else"])
				$elseexp
			)?
		)
	)
	;
statements
	: statement+
	;

proc_arg_mode
	: 'in' 
	| 'out' 
	| 'inout'
	;
proc_arg
	: proc_arg_mode? identifier ('as' datatype)? 
	->^(Ast_Element ^(Ast_ElementName identifier)
		^(Ast_Parameters
			^(Ast_NodeNamer 
				^(Syn_ProcArgType["type"] 
					datatype
				)
			)?
			^(Ast_NodeNamer 
				^(Syn_ProcArgMode["mode"] 
					^(Ast_Element ^(Ast_ElementName proc_arg_mode))
				)
			)?
		)
	)
	;
proc_arguments
@init { PushPassphrase("in proc arguments"); }
@after { PopPassphrase(); }
	: x='(' proc_arg_list?   ')' -> ^(Ast_Parameters[$x,"Ast_Parameters"] proc_arg_list? )
	;
proc_arg_list
	:proc_arg  (',' proc_arg)* ->proc_arg+
	;
proc_statement
	: my_proc='proc'  my_name=identifier proc_arguments ('returns' datatype)? my_body=braces
	-> ^(Ast_Element ^(Ast_ElementName Syn_Proc[$my_proc]) 
		^(Ast_Parameters 
			^(Ast_Element ^(Ast_ElementName Syn_ProcName["name"] )
				^(Ast_Parameters
					 ^(Ast_Element ^(Ast_ElementName $my_name) )
				)
			)
			^(Ast_Element ^(Ast_ElementName Syn_ProcArguments["arguments"] )
				proc_arguments
			)
			^(Ast_Element ^(Ast_ElementName Syn_ProcReturns["returns"] )
				^(Ast_Parameters
					datatype
				)
			)?
		)
		$my_body
	)
	;
// this is either a block (eats the braces), or a one liner.
body_statement
	: ('{')=>'{' statements? '}'->^(Ast_Brace statements?)
	| statement ->^(Ast_Brace statement?)
	;
iteration_statement
	: x='while' '(' while_cond=expression ')' while_body=body_statement
	-> ^(Ast_Element ^(Ast_ElementName Syn_While[$x]) 
		^(Ast_Parameters 
			^(Ast_NodeNamer ^(Syn_WhileCondition["condition"] $while_cond) )
		)
		$while_body
	)
	| 'do' do_while_body=body_statement x='while' '(' do_while_cond=expression ')' ';'
	-> ^(Ast_Element ^(Ast_ElementName Syn_DoWhile[$x,"do_while"]) 
		^(Ast_Parameters 
			^(Ast_NodeNamer ^(Syn_DoWhileCondition["condition"] $do_while_cond) )
		)
		$do_while_body
	)
	//| 'for' '('  expression_statement expression_statement expression? ')' statement
	| x='for' '('for_init=expression ';' for_cond=expression ';' for_incr=expression? ')' for_body=body_statement
	-> ^(Ast_Element ^(Ast_ElementName Syn_For[$x]) 
		^(Ast_Parameters 
			^(Ast_NodeNamer ^(Syn_ForInitialize["initialize"] $for_init) )
			^(Ast_NodeNamer ^(Syn_ForCondition["condition"] $for_cond) )
			^(Ast_NodeNamer ^(Syn_ForStep["step"] $for_incr) )?
		)
		$for_body
	)
	| x='foreach' '('foreach_elem=expression 'in' foreach_list=expression ')' foreach_body=body_statement
	-> ^(Ast_Element ^(Ast_ElementName Syn_Foreach[$x])
		^(Ast_Parameters
			^(Ast_NodeNamer ^(Syn_ForeachItem["item"] $foreach_elem) )
			^(Ast_NodeNamer ^(Syn_ForeachList["list"] $foreach_list) )
		)
		$foreach_body
	)
	;
label
	: Id -> ^(Ast_Value Id)
	; 
jump_statement
	: 'continue' '(' label ')' ';'
	-> ^(Ast_Element ^(Ast_ElementName 'continue') 
		^(Ast_Parameters  
			label
		)
	)
	| 'continue' label ';'
	-> ^(Ast_Element ^(Ast_ElementName 'continue') 
		^(Ast_Parameters  
			label
		)
	)
	| 'continue' ';'
	-> ^(Ast_Element ^(Ast_ElementName 'continue')
		^(Ast_Parameters)
	)
	| 'break' '(' label ')' ';'
	-> ^(Ast_Element ^(Ast_ElementName 'break') 
		^(Ast_Parameters  
			label
		)
	)
	| 'break' label ';'
	-> ^(Ast_Element ^(Ast_ElementName 'break') 
		^(Ast_Parameters  
			label
		)
	)
	| 'break' ';'
	-> ^(Ast_Element ^(Ast_ElementName 'break')
		^(Ast_Parameters)
	)
	| 'return' ';'
	-> ^(Ast_Element ^(Ast_ElementName 'return')
		^(Ast_Parameters)
	) 
	| 'return' expression ';'
	-> ^(Ast_Element ^(Ast_ElementName 'return') 
		^(Ast_Parameters  
			expression
		)
	)
	;
try_block
	:x='try' try_body=compound_statement
	handler*
	(finally_block=finally_handler )?
	-> ^(Ast_Element ^(Ast_ElementName Syn_Try[$x]) 
		^(Ast_Parameters  
			$try_body
			handler*
			$finally_block?
		)
	)
	;
handler
	:'catch' '(' expression_list ')' compound_statement
	-> ^(Ast_Element ^(Ast_ElementName 'catch') 
		^(Ast_Parameters  
			expression_list
			compound_statement
		)
	)
	;

finally_handler
    	: 'finally' compound_statement
    	-> ^(Ast_Element ^(Ast_ElementName 'finally') 
		^(Ast_Parameters  
			compound_statement
		)
	)
    	;



///////////////////////////////////////////////////////
//	Lexar Section
///////////////////////////////////////////////////////



literal 
	: integerLiteral 
	-> ^(Ast_Element ^(Ast_ElementName Syn_LiteralInt) 
		^(Ast_Parameters  
			^(Ast_Value integerLiteral) 
		) 
	)
	| DecimalLiteral 
	-> ^(Ast_Element ^(Ast_ElementName Syn_LiteralFloat) 
		^(Ast_Parameters 
			^(Ast_Value DecimalLiteral) 
		) 
	)
	| StringLiteral 
	-> ^(Ast_Element ^(Ast_ElementName Syn_literalString) 
		^(Ast_Parameters 
			^(Ast_Value StringLiteral) 
		)
	)
	| booleanLiteral 
	-> ^(Ast_Element ^(Ast_ElementName Syn_LiteralBool) 
		^(Ast_Parameters 
			^(Ast_Value booleanLiteral)
		) 
	)
	| nullLiteral 
	-> ^(Ast_Element ^(Ast_ElementName Syn_LiteralNull) 
		^(Ast_Parameters 
			^(Ast_Value nullLiteral) 
		) 
	)
	;
integerLiteral
	: HexLiteral
	| OctalLiteral
	| IntegerLiteral
	;
booleanLiteral
	: 'true'
	| 'false'
	;
nullLiteral
	: 'null'
	| 'NULL'
	;
	
identifier
	:Id
 	;
///////////////////////////////////////////////////////

Id
	: ('a'..'z'|'A'..'Z'|'_') ('a'..'z'|'A'..'Z'|'0'..'9'|'_')*
	;
HexLiteral 
	: '0' ('x'|'X') HexDigit+ IntegerTypeSuffix?
	;
IntegerLiteral 
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
DecimalLiteral
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
@init
{
 int startPos = input.CharPositionInLine+1;
 int startLine = input.Line;
}
: '"' ( ~('"') )* ('"'| { MissingClosingError("Unterminated [\"] starting on line="+ startLine+", position="+ startPos,	startLine,startPos,"\"","\""); }) 
| '\'' ( ~('\'') )* ('\''| { MissingClosingError("Unterminated [\'] starting on line="+ startLine+", position="+ startPos,startLine,startPos,"\'","\'"); }) 
;
WS  
	:  (' '|'\r'|'\t'|'\u000C'|'\n') {$channel=HIDDEN;}
	;
COMMENT
	: '/*' ( options {greedy=false;} : . )* '*/' {$channel=HIDDEN;}
	;
LINE_COMMENT
	: '//' ~('\n'|'\r')* '\r'? '\n' {$channel=HIDDEN;}
	;