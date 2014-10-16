grammar EntityGrammar;

// This file defines the grammar we use to map MVM Entitites
//
// Here are some samples that should be supported:
// ENTITY.x=PARENT.y
// PARENT.x=ENTITY.y
// CHILD.x=ENTITY.y
// CHILD.x,CHILD.y,CHILD.z=split(',',PARENT.x,3)
// CHILD.lower_x=lower(PARENT.upper_x)


// Use these options when testing grammar in antlrworks
// because antlrworks only suppports Java
//options {
//	output=AST;
//}
// Use these options to generate c#
options {
	output=AST;	
	language=CSharp2; 
}


// Tokens we using in building the AST
tokens {
	FUNCTION_NAME;
	FUNCTION;
	FUNCTION_PARAMS;
	ENTITY;
	CHILD;
	PARENT;
	LITERAL;
	INT;
	FLOAT;
	STRING;
	LEFT;
	RIGHT;
}

////////////////////////////  PARSER /////////////////////////

	
start
	:
	assignment
	;  
literal 
	: INT -> ^(LITERAL INT ^(INT))
	| FLOAT -> ^(LITERAL FLOAT ^(FLOAT))
	| STRING -> ^(LITERAL STRING ^(STRING))
	;
variable
	: 'ENTITY.' ID  -> ^(ENTITY ID)
	| 'CHILD.' ID -> ^(CHILD ID)
	| 'PARENT.' ID  -> ^(PARENT ID)
	;
function
	: ID'('atom_list')' -> ^(FUNCTION ^(FUNCTION_NAME ID) ^(FUNCTION_PARAMS atom_list))
	;
atom	
	: literal
	| variable
	| function
	;	
atom_list
	: atom (','! atom)* 
	;
		
assignment
	: atom_list '=' atom -> ^(LEFT atom_list) ^(RIGHT atom)
	;
	
////////////////////////////  LEXER /////////////////////////

ID  :	('a'..'z'|'A'..'Z'|'_') ('a'..'z'|'A'..'Z'|'0'..'9'|'_')*
    ;

INT :	'0'..'9'+
    ;

FLOAT
    :   ('0'..'9')+ '.' ('0'..'9')* EXPONENT?
    |   '.' ('0'..'9')+ EXPONENT?
    |   ('0'..'9')+ EXPONENT
    ;

COMMENT
    :   '//' ~('\n'|'\r')* '\r'? '\n' {$channel=HIDDEN;}
    |   '/*' ( options {greedy=false;} : . )* '*/' {$channel=HIDDEN;}
    ;

WS  :   ( ' '
        | '\t'
        | '\r'
        | '\n'
        ) {$channel=HIDDEN;}
    ;

STRING
    :  '"' ( ESC_SEQ | ~('\\'|'"') )* '"'
    |  '\'' ( ESC_SEQ | ~('\\'|'\'') )* '\''
    ;

fragment
EXPONENT : ('e'|'E') ('+'|'-')? ('0'..'9')+ ;

fragment
HEX_DIGIT : ('0'..'9'|'a'..'f'|'A'..'F') ;

fragment
ESC_SEQ
    :   '\\' ('b'|'t'|'n'|'f'|'r'|'\"'|'\''|'\\')
    |   UNICODE_ESC
    |   OCTAL_ESC
    ;

fragment
OCTAL_ESC
    :   '\\' ('0'..'3') ('0'..'7') ('0'..'7')
    |   '\\' ('0'..'7') ('0'..'7')
    |   '\\' ('0'..'7')
    ;

fragment
UNICODE_ESC
    :   '\\' 'u' HEX_DIGIT HEX_DIGIT HEX_DIGIT HEX_DIGIT
    ;
