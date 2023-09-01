# RawDeal E1 - Franco Anfossi

El programa funciona con exito y sin problemas que yo haya visto, ademas se logran pasar todos los tests sin hardcode, intentando separar en clases por las funciones que cada una realiza.

Es importante que al correr el programa (`Program.cs`) todos los archivos esten en la que misma carpeta (en RawDeal).

Los archivos son los siguientes:
- `Carta.cs`: Se encuenta la clase Carta que tiene los atributos necesarios para el json de las cartas. (En un futuro es probable que se haga abstracta).
- `Superstar.cs`: Se encuenta la clase Superstar que tiene los atributos necesarios para el json de los superstars.
- `Jugador.cs`: Es la clase jugador, que tiene los atributos necesarios y metodos para sacar cartas.
- `Game.cs`: Es la clase en donde se hace lo necesario para que se simule el juego.
- `ConjuntoCartas.cs`: Una clase en donde se tienen todas las cartas y todos los superstars sin repetición.
- `Mazo.cs`: Es la clase en donde se crea cada mazo de los archivos txt.
- `MazoValidatos.cs`: Clase con la cual se valida cada una de las condiciones para el mazo.
- `Utils`: Algunos metodos funcionales para revolver el mazo y abrir los archivos json y txt.