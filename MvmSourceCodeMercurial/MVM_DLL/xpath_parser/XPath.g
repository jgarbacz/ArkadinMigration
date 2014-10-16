grammar XPath;

/*
XPath 1.0 grammar. Should conform to the official spec at
http://www.w3.org/TR/1999/REC-xpath-19991116. The grammar
rules have been kept as close as possible to those in the
spec, but some adjustmewnts were unavoidable. These were
mainly removing left recursion (spec seems to be based on
LR), and to deal with the double nature of the '*' token
(node wildcard and multiplication operator). See also
section 3.7 in the spec. These rule changes should make
no difference to the strings accepted by the grammar.

Written by Jan-Willem van den Broek
Version 1.0

You may consider this work to be in the public domain.

Rob Parks add rewrite rules to build an AST used to 
evaluate xpath against ANTLR ITrees. This is a partial
implementation.


*/

// Use these options when testing grammar in antlrworks
// because antlrworks only suppports Java
options {
	output=AST;
}
// Use these options to generate c#
/*
options {
	output=AST;	
	language=CSharp2; 
}
*/

tokens {
  // Tokens add for the AST
  XPATH;
  ABSOLUTE_PATH;
  RELATIVE_PATH;
  ROOT_PATH;
  PREDICATE;
  RECURSIVE_MATCH;
  TRAVERSE_UP;
  CURRENT_NODE;
  MATCH;
  

  PATHSEP  =  '/';
  ABRPATH  =  '//';
  LPAR  =  '(';
  RPAR  =  ')';
  LBRAC  =  '[';
  RBRAC  =  ']';
  MINUS  =  '-';
  PLUS  =  '+';
  DOT  =  '.';
  MUL  =  '*';
  DOTDOT  =  '..';
  AT  =  '@';
  COMMA  =  ',';
  PIPE  =  '|';
  LESS  =  '<';
  MORE  =  '>';
  LE  =  '<=';
  GE  =  '>=';
  COLON  =  ':';
  CC  =  '::';
  APOS  =  '\'';
  QUOT  =  '\"';
}

main  :  expr -> ^(XPATH expr)
  ;

locationPath
  :  relativeLocationPath
  |  absoluteLocationPathNoroot
  ;

absoluteLocationPathNoroot
  : '/..' (traverse)* ->^(ABSOLUTE_PATH ^(TRAVERSE_UP) (traverse)*)
  | '/.' (traverse)* ->^(ABSOLUTE_PATH ^(CURRENT_NODE) (traverse)*)
  //:  '/' relativeLocationPath
  | '/'  step (traverse)* ->^(ABSOLUTE_PATH ^(MATCH step) (traverse)*)
  //|  '//' relativeLocationPath
  | '//'  step (traverse)* ->^(ABSOLUTE_PATH ^(RECURSIVE_MATCH step) (traverse)*)
  ;

relativeLocationPath
  : '..' (traverse)* ->^(RELATIVE_PATH ^(TRAVERSE_UP) (traverse)*)
  | '.' (traverse)* ->^(RELATIVE_PATH ^(CURRENT_NODE) (traverse)*)
  
  //:  step (('/'|'//') step)* ->^(RELATIVE_PATH ('/'|'//') step)+
    |  step (traverse)* ->^(RELATIVE_PATH ^(MATCH step) (traverse)*)
  ;
  
traverse
	: '/..' -> ^(TRAVERSE_UP)
	| '/.' -> ^(CURRENT_NODE)
	| '/' step -> ^(MATCH step)
	| '//' step-> ^(RECURSIVE_MATCH step)
	;

step  
  :  axisSpecifier nodeTest predicate*
  //|  abbreviatedStep
  ;

axisSpecifier
  :  AxisName '::'
  |  '@'?
  ;

nodeTest:  nameTest
  |  NodeType '(' ')'
  |  'processing-instruction' '(' Literal ')'
  ;

predicate
  :  '[' expr ']' ->^(PREDICATE expr)
  ;

abbreviatedStep
  :  '.'
  |  '..'
  ;

expr  :  orExpr
  ;

primaryExpr
  :  variableReference
  |  '(' expr ')'
  |  Literal
  |  Number  
  |  functionCall
  ;

functionCall
  :  functionName '(' ( expr ( ',' expr )* )? ')'
  ;

unionExprNoRoot
  :  pathExprNoRoot ('|'^ unionExprNoRoot)?
  |  '/' '|'^ unionExprNoRoot
  ;

pathExprNoRoot
  :  locationPath
  |  filterExpr (('/'|'//') relativeLocationPath)?
  ;

filterExpr
  :  primaryExpr predicate*
  ;

orExpr  :  andExpr ('or'^ andExpr)*
  ;

andExpr  :  equalityExpr ('and'^ equalityExpr)*
  ;

equalityExpr
  :  relationalExpr (('='|'!=')^ relationalExpr)*
  ;

relationalExpr
  :  additiveExpr (('<'|'>'|'<='|'>=')^ additiveExpr)*
  ;

additiveExpr
  :  multiplicativeExpr (('+'|'-')^ multiplicativeExpr)*
  ;

multiplicativeExpr
  :  unaryExprNoRoot (('*'|'div'|'mod')^ multiplicativeExpr)?
  |  '/' (('div'|'mod')^ multiplicativeExpr)?
  ;

unaryExprNoRoot
  :  '-'* unionExprNoRoot
  ;

qName  :  nCName (':' nCName)?
  ;

functionName
  :  qName  // Does not match nodeType, as per spec.
  ;
 
variableReference
  :  '$' qName
  ;

nameTest:  '*'
  |  nCName ':' '*'
  |  qName
  ;

nCName  :  NCName
  |  AxisName
  ;

NodeType:  'comment'
  |  'text'
  |  'processing-instruction'
  |  'node'
  ;
 
Number  :  Digits ('.' Digits?)?
  |  '.' Digits
  ;

fragment
Digits  :  ('0'..'9')+
  ;

AxisName:  'ancestor'
  |  'ancestor-or-self'
  |  'attribute'
  |  'child'
  |  'descendant'
  |  'descendant-or-self'
  |  'following'
  |  'following-sibling'
  |  'namespace'
  |  'parent'
  |  'preceding'
  |  'preceding-sibling'
  |  'self'
  ;

Literal  :  '"' ~'"'* '"'
  |  '\'' ~'\''* '\''
  ;

Whitespace
  :  (' '|'\t'|'\n'|'\r')+ {$channel = HIDDEN;}
  ;

NCName  :  NCNameStartChar NCNameChar*
  ;

fragment
NCNameStartChar
  :  'A'..'Z'
  |   '_'
  |  'a'..'z'
  |  '\u00C0'..'\u00D6'
  |  '\u00D8'..'\u00F6'
  |  '\u00F8'..'\u02FF'
  |  '\u0370'..'\u037D'
  |  '\u037F'..'\u1FFF'
  |  '\u200C'..'\u200D'
  |  '\u2070'..'\u218F'
  |  '\u2C00'..'\u2FEF'
  |  '\u3001'..'\uD7FF'
  |  '\uF900'..'\uFDCF'
  |  '\uFDF0'..'\uFFFD'
// Unfortunately, java escapes can't handle this conveniently,
// as they're limited to 4 hex digits. TODO.
//  |  '\U010000'..'\U0EFFFF'
  ;

fragment
NCNameChar
  :  NCNameStartChar | '-' | '.' | '0'..'9'
  |  '\u00B7' | '\u0300'..'\u036F'
  |  '\u203F'..'\u2040'
  ;
