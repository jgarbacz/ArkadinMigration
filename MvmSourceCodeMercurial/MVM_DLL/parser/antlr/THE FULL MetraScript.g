grammar MetraScript;

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
	MODULE;
	MODULE_NAME;
	MODULE_BODY;
	ATTRIBUTE;
	ATTR_NAME;
	ATTR_VALUE;
	LVALUES;
	EXPR;
	VAR;
	DEREF;
	DEREF_ARRAY;
	DEREF_MAP;
	DEREF_FIELD;
	DEREF_METHOD;
	DEREF_STRUCT;
	ID;
	ASSIGN;
	LVAL;
	RVAL;
	ATTRIBUTES;
	STATMENTS;
	FUNCTION_ARGS;
}

// uncomment when generating csharp
@header {
using ParserExensionsNameSpace;
}


////////////////////////////  PARSER /////////////////////////

scriptFile
	: modules EOF!
	;
modules
	:(module ';'?)+ -> module+ //top level modules have optional trailing semicolons
	;
module
	: moduleName '()' -> ^(MODULE ^(MODULE_NAME moduleName))
	| moduleName '(' moduleBody ')' -> ^(MODULE ^(MODULE_NAME moduleName) moduleBody)
	;
moduleName
	: Id 
	;
moduleBody
	:	
	attributes? statements? functionArgs?
	;
functionArgs
	: expr (',' expr)* -> ^(FUNCTION_ARGS expr*)
	;
statements
	:
	statement+ -> ^(STATMENTS statement+)
	;
statement
	: expression ';'!
	;
attributes
	:
	attribute+ -> ^(ATTRIBUTES attribute+)
	;
attribute
	: attributeName ':' statement ->^(ATTRIBUTE ^(ATTR_NAME attributeName) ^(ATTR_VALUE statement))
	;	
attributeName
	: Id
	;
lvalue	
	: unit+ -> ^(LVALUES unit+)
	;
expr
	: conditionalExpression
	;	  
unit	
	: literal
	| variable
	;
variable
	: module variablePart* -> ^(VAR module variablePart*)
	| Id variablePart* -> ^(VAR Id variablePart*)
	;	
variablePart
	: '->' Id variablePart? -> ^(DEREF_STRUCT Id variablePart?)
	| '[' expr ']' variablePart? -> ^(DEREF_ARRAY expr variablePart?)
	| '{' expr '}' variablePart? -> ^(DEREF_MAP expr variablePart?)
	| '.' module variablePart? -> ^(DEREF_METHOD module variablePart?)
	| '.' Id variablePart? -> ^(DEREF_FIELD Id variablePart?)
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


// EXPRESSIONS

parExpression
	: '(' expression ')' -> expression
	;
expression
	: conditionalExpression (assignmentOperator^ expression)?
	;
assignmentOperator
	: '='
	| '+='
	| '-='
	| '*='
	| '/='
	| '&='
	| '|='
	| '^='
	| '%='
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
	: conditionalAndExpression ( '||'^ conditionalAndExpression )*
	;
conditionalAndExpression
	: inclusiveOrExpression ( '&&'^ inclusiveOrExpression )*
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
	: instanceOfExpression ( ('==' | '!=')^ instanceOfExpression )*
	;
instanceOfExpression
	//: relationalExpression ('instanceof' type)?
	: relationalExpression //('instanceof' type)?
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
	//| primary selector* ('++'|'--')?
	| primary ('++'|'--')^  
	| primary
	;
castExpression
	//:  '(' primitiveType ')' unaryExpression
	//|  '(' (type | expression) ')' unaryExpressionNotPlusMinus
	:  '(' Id ')' unaryExpression
	|  '(' Id ')' unaryExpressionNotPlusMinus
	;
primary
	: parExpression
	//| 'this' ('.' Identifier)* identifierSuffix?
	//| 'super' superSuffix
	//| literal
	//| 'new' creator
	//| Identifier ('.' Identifier)* identifierSuffix?
	//| primitiveType ('[' ']')* '.' 'class'
	//| 'void' '.' 'class'
	| unit //rjp
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
	: '"' ( EscapeSequence | ~('\\'|'"') )* '"'
	| '\'' ( EscapeSequence | ~('\\'|'\'') )* '\''
	;
fragment
EscapeSequence
	: '\\' ('b'|'t'|'n'|'f'|'r'|'\"'|'\''|'\\')
	| UnicodeEscape
	| OctalEscape
	;
fragment
OctalEscape
	: '\\' ('0'..'3') ('0'..'7') ('0'..'7')
	| '\\' ('0'..'7') ('0'..'7')
	| '\\' ('0'..'7')
	;
fragment
UnicodeEscape
	: '\\' 'u' HexDigit HexDigit HexDigit HexDigit
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
