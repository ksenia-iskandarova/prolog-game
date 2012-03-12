/*****************************************************************************

		Copyright (c) My Company

 Project:  FINAL
 FileName: FINAL.PRO
 Purpose: No description
 Written by: Visual Prolog
 Comments:
******************************************************************************/

include "final.inc"

global domains 
	%halfcell = unsigned16
	cell = h(unsigned16,unsigned16).%halfcell*
	cells = cell*.

%отобразить: (клетки).
%final()
global predicates
	%win(integer, integer, cells, cells) - nondeterm (i,i,i,o).
	%step(cells, cells, cell) -  nondeterm (i,o,o) (i,o,i).
	%shift(cell, cell) -  nondeterm (i,o) (i,i).
	%way(integer, cells) - nondeterm (i,i). %  language stdcall as "way".  %ход
	member(cell,cells) -  nondeterm (i,i).
	reverse(cells,cells,cells) -  nondeterm (i,i,o).
	append(cells,cells,cells) - nondeterm (i,i,o).
	procedure say_hello (integer,integer) - (i,o) language stdcall
	procedure getway(integer,integer,integer) - (i,o,o) language stdcall
	
clauses
	say_hello(X,Y) :-  Y=X+1.
	member(X,[X|_]).
  	member(X,[_|H]):-member(X,H).
  	reverse([H|T],S,L):- reverse(T,[H|S],L). 
        reverse([],L,L). 
  	append([],B,B). %esli pervii spisok pustoi, to rezultat - vtotoi spisok
	append([H|Tail],B,[H|NewTail]):-append(Tail,B,NewTail). %esli ne pustoi, to rekursivno obiedinyaem hvost i vtoroi spisok
  	
  	getway(X,Y,1):-say_hello(X,Y),Y=5.%:-Line=[X,Y],way(Player,Line).
  	getway(X,Y,0):-say_hello(X,Y),Y<>5.
  	
	%win(Player1, Player2, Line, Line1):-step(Line, Line1, Cell),member(Cell, Line).
    	%win(Player1, Player2, Line, Line1):-step(Line, Line1, _),not(win(Player2, Player1, Line1, _)).

	%way(2, Line):-step(Line, Line1, Cell),member(Cell, Line),!,write("Компьютер 1 выиграл!\n\n", Line1).
    	%way(2, Line):-win(2, 1, Line, Line1),!,way(1, Line1).
    	%way(2, Line):-step(Line, Line1, _),!,way(1, Line1).
    	
    	%way(1, Line):-step(Line, Line1, X), !. %if member(X, Line) then R=1 else  way(2, Line1) end if.
    	%way(_, _).
    	
    	%step([A, B | Line], [X, A, B | Line], X):-shift(A, X),not(X = B).
    	%step(Line, Line1, X):-reverse(Line,[],Line2),Line2 = [A, B | _],shift(A, X),not(X = B),append(Line, [X], Line1).
    	
    	%shift(h(X,Y),h(X-1,Y)):- X > 1.
    	%shift(h(X,Y),h(X+1,Y)):- X < 6.        
    	%shift(h(X,Y),h(X,Y-1)):- Y > 1.    
    	%shift(h(X,Y),h(X,Y+1)):- Y < 5.        
goal
  true.
