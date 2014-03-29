# Pathfinding con A\*

## Analisi e Progettazione

### L'area di ricerca

L'area di ricerca rappresenta l'area nella quale i personaggi (giocanti e non) del videogame si muovono. I personaggi non giocanti utilizzano l'area di ricerca per trovare il percorso per spostarsi da un punto di partenza ad un punto di arrivo. Semplificare l'area di ricerca è il primo passo nell'implementazione di un algoritmo di pathfinding.

Fra i vari metodi esistenti per la rappresentazione dell'area di ricerca, ho deciso di utilizzare la griglia. Questo metodo riduce l'area di ricerca ad un semplice array bidimensionale. Ogni elemento in questo array rappresenta un quadrato della griglia. Il percorso viene trovato estraendo dall'array i quadrati che dovrebbero essere percorsi per spostarsi da un punto A ad un punto B. Una volta trovato il percorso, il personaggio non giocante, si muove dal centro di un quadrato al centro del quadrato successivo finchè l'obiettivo non viene raggiunto.

Il centro di un quadrato viene chiamato nodo.

### La ricerca

In generale la ricerca viene effettuata partendo dal punto di partenza (A), controllando tutti i nodi ad esso adiacenti, e cercando verso l'esterno finchè non viene raggiunto il punto finale (B).

Nel dettaglio, vengono seguiti i seguenti passi:

1. Si inizia dal punto di partenza A e lo si aggiunge ad una particolare lista, chiamata Open List, di nodi da considerare. Tale lista contiene i nodi che potrebbero far parte del percorso da seguire.
2. Si prendono tutti i nodi adiacenti al nodo A e li si aggiunge alla lista aperta. Ad ognuno di questi nodi viene assegnato il nodo A come nodo genitore. Il nodo genitore rappresenta il nodo dal quale si è arrivati al nodo che si sta esaminando, e viene utilizzato alla fine dell'algoritmo.
3. Si estrae il nodo A dalla Open List e lo si aggiunge ad un'altra lista, la Close List. Tale lista contiene i nodi che non devono essere ulteriormente esaminati.

A questo punto viene estratto un nodo dalla Open List, e si procede iterando i 3 passi precedenti. Il nodo che deve essere estratto dalla Open List è il nodo con minor costo F.


### I costi dei nodi

L'equazione per determinare il costo F di un nodo è la seguente

  ```
  F = G + H
  ```

dove:

G è il costo del movimento per muoversi dal nodo A, ad un dato nodo, seguendo il percorso generato per arrivarci;

H è una stima del costo del movimento per muoversi da un nodo dato, al nodo finale B. Questo costo è un costo euristico poiché rappresenta solo una stima e non il costo effettivo.

Per quanto riguarda G ho scelto di assegnare un costo 10 per i movimenti orizzontali e verticali, e 14 per i movimenti diagonali. Poichè G viene calcolato come il costo per muoversi dal punto di parteza A ad un dato nodo, G avrà come valore il costo G del nodo genitore più 10 o 14 (a seconda che si tratti di un movimento ortogonale o diagonale).

Per quanto riguarda H ho deciso di usare un metodo noto come Distanza Diagonale. Tale metodo è adatto per griglie che consentono lo spostamento in 8 direzioni. Nella griglia è possibile infatti muoversi in 8 direzioni dal centro di un quadrato al centro di uno degli 8 quadrati adiacenti (4 movimenti ortogonali e 4 diagonali). Il calcolo della Distanza Diagonale viene effettuato in 3 passi:

Viene calcolato il numero di passi che si possono percorrere lungo la diagonale
Viene calcolato il numero di passi orizzontali e verticali necessari per raggiungere il punto finale (metodo di Manhattan)
I due risultati vengono combinati considerando i 2 costi differenti per i movimenti ortogonali e diagonali

In formule:

  ```
  h_diagonal(node) = min(abs(node.x – goal.x), abs(node.y – goal.y))
  h_straight(node) = (abs(node.x – goal.x) + abs(node.y – goal.y))
  h(node) = oc * h_diagonal + dc * (h_straight(node) – 2 * h_diagonal(node))
  ```

dove:

oc = Costo dei movimenti ortogonali; dc = Costo dei movimenti diagonali

Trattandosi di una griglia di quadrati senza variazioni di tipo di terreno, l'algoritmo A\* potrebbe esplorare tutti i percorsi che hanno lo stesso valore F, invece di esplorarne uno solo. Per risolvere questo problema bisogna modificare il risultato di H applicando una tecnica nota come Tie-breaking. Questa tecnica modifica il valore di H in maniera da far prediligere ad A\* i percorsi che seguono una linea retta dal punto di partenza.

In formule:

  ```
  dx1 = node.x – goal.x
  dy1 = node.y – goal.y
  dx2 = start.x – goal.x
  dy2 = start.y – goal.y
  cross = abs(dx1 * dy2 – dx2 * dy1)
  H += cross * 0.001
  ```

Il costo F quindi, viene calcolato sommando i valori dei costi G ed H.

### Continuare la ricerca

Per continuare la ricerca, si estrae dalla Open List un nodo con minore costo F e si eseguono le seguenti operazioni sul nodo corrente:

1. Lo si inserisce nella Close List. Se tale nodo è il nodo finale, la ricerca è terminata.
2. Si prendono tutti i nodi adiacenti. Ignorando i nodi non percorribili e che non sono presenti nella Close List, li si aggiunge alla Open List se non sono già presenti. Ad ognuno di questi nodi viene assegnato il nodo corrente come nodo genitore.
3. Se un nodo adiacente è già nella Open List, si controlla se questo percorso per raggiungerlo è migliore rispetto al precedente (tramite il costo G). Se si, si assegna al nodo nella Open List il nodo corrente come nodo genitore, e si ricalcolano G ed F.

Questo processo viene ripetuto finchè il nodo finale non viene aggiunto alla lista chiusa.

Quando ciò si verifica, per determinare il percorso, ci si muove dal nodo finale nella lista chiusa, fino al nodo iniziale seguendo i nodi genitore di ogni nodo. Invertendo l'ordine dei nodi si ottiene il percorso da seguire.

### Algoritmo

1. Aggiungere il nodo di partenza A alla lista aperta
2. Ripetere i seguenti passi:
  1. Cercare il nodo con costo F minore nella Open List. Assegnare tale nodo al nodo corrente
  2. Inserire il nodo corrente nella Close List
  3. Per ognuno degli 8 nodi adiacenti al nodo corrente:
Se non è percorribile o è presente nella Close List, ignorarlo. Altrimenti...
Se non è presente nella Open List, aggiungerlo alla Open List. Assegnargli il nodo corrente come nodo genitore. Calcolare i suoi costi F, G ed H
Se è presente nella Open List, controllare se questo percorso per raggiungerlo è migliore, utilizzando il costo G come parametro. Ad un minore costo G corrisponde un percorso migliore. Se è così, cambiare il nodo genitore, assegnandogli il nodo corrente, e ricalcorare i costi F e G.
  4. Fermarsi quando:
Viene aggiungo il nodo finale alla lista chiusa. In tal caso il percorso è stato trovato, oppure...
Non viene trovato il nodo finale e la Open List è vuota. In tal caso, il percorso non esiste.
3. Salvare il percorso. Muoversi all'indietro dal nodo finale, passando per ogni nodo genitore, finchè non si raggiunge il nodo di partenza, trovando così il percorso.

## Strutture Dati

Gli elementi per l'implementazione dell'algoritmo di pathfinding sono i seguenti:

Nodo
Griglia
Open List
Close List

### Nodo

Memorizza le coordinate del nodo nell'ambiente di gioco (x, y), lo stato del nodo (percorribile o non percorribile), ed i suoi costi F, G ed H. Fornisce, inoltre, funzioni per l'accesso ai suoi dati sia in lettura che in scrittura.

### Griglia

Contiene una rappresentazione bidimensionale della griglia (una matrice). Ogni elemento della matrice è un nodo. Fornisce una funzione per la scansione automatica dell'ambiente di gioco ai fini di costruire la matrice di nodi. Fornisce, inoltre, funzioni per la lettura della griglia e per l'estrazione di nodi adiacenti ad un nodo dato.

### Open List

Poichè l'operazione principale da effettuare su questa lista è l'operazione di estrazione del nodo con costo F minore, è utile implementare tale lista come una struttura che mantenga sempre ordinati i dati al suo interno. Così facendo, il nodo con costo F minore sarà sempre in prima posizione. Inoltre, ogni volta che un nodo viene aggiunto, esso verrà posizionato, all'interno della lista, in base al suo costo F.

### Close List

E' una semplice lista contenente nodi.

## Fonti:

Per l'algoritmo A\*
http://www.policyalmanac.org/games/aStarTutorial.htm

Per il calcolo dell'Euristica (H) e delle tecniche di Tie-Breaking
http://theory.stanford.edu/~amitp/GameProgramming/Heuristics.html

