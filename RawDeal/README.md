# RawDeal E2 - Franco Anfossi

El programa funciona con exito y sin problemas que yo haya visto, ademas se logran pasar todos los tests sin hardcode.

Ademas he intentado utilizar la mayor cantidad de clean code que haya podido ver, ya que no se donde mas es posible aplicarlo, respete lo de la identacion, intente que todos los metodos sean menor a 10 lineas y que cada uno haga solo una cosa, tambien que los nombres de las variables y metodos sean lo mas descriptivas posibles y que sean sustantivos y verbos respectivamente. Tambien evité y reduje lo mas que pude la duplicacion de codigo.  

Es importante que al correr el programa (`Program.cs`) todos los archivos esten en la que misma carpeta (en RawDeal).

Los archivos son los siguientes:
- `Carta.cs`: Se encuenta la clase Carta que tiene los atributos necesarios para el json de las cartas. (En un futuro es probable que se haga abstracta).
- `Superstar.cs`: Se encuenta la clase abstracta Superstar que tiene los atributos necesarios para el json de los superstars y tambien los metodos necesarios para que se pueda jugar el juego ya que un superstar es un jugador.
- `Game.cs`: Es la clase en donde se hace lo necesario para que se simule el juego.
- `ConjuntoCartas.cs`: Una clase en donde se tienen todas las cartas y todos los superstars sin repetición.
- `Mazo.cs`: Es la clase en donde se crea cada mazo de los archivos txt.
- `MazoValidatos.cs`: Clase con la cual se valida cada una de las condiciones para el mazo.
- `Utils`: Algunos metodos funcionales para revolver el mazo, abrir los archivos json y txt y formatear las cartas.
- `EleccionesJugarCarta.cs`: Es la clase en donde se manejan las deciones de jugar o no una carta.
- `EleccionesVerCartas.cs`: Es la clase en donde se manejan las deciones de ver los distintos mazos posibles en el flujo del juego.
- `Habilidades Superstars`: Es una carpeta en donde se encuentran las clases de los superstars, en las cuales se especifica principalmente el uso de las habilidades especiales de cada tipo de superstar.