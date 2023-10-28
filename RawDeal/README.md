# RawDeal E3 - Franco Anfossi

El programa en general funciona con exito y y se pasan casi todos los tests solo faltaron 9 de EffectsPart1

Ademas he intentado utilizar la mayor cantidad de clean code que haya podido ver, ya que no se donde mas es posible aplicarlo, respete lo de la identacion, intente que todos los metodos sean menor a 10 lineas y que cada uno haga solo una cosa, tambien que los nombres de las variables y metodos sean lo mas descriptivas posibles y que sean sustantivos y verbos respectivamente. Tambien evité y reduje lo mas que pude la duplicacion de codigo. Ademas aplique los boundaries necesarios, que cada clase haga solo una cosa y que no hayan mas de 120 lineas por final.  

Es importante que al correr el programa (`Program.cs`) todos los archivos esten en la que misma carpeta (en RawDeal).

Esta parte esta desactualizada, ahora hay una gran cantidad de Data Structures y muchos archivos de effectos, constructores de cosas necesarias y tambien algunos de los archivos que estan abajo fueron divididos en mas de un archivo para que cada clase tenga una sola funcionalidad.

Ademas se han agregado excepciones y algunas interfaces.


Estos son los archivos que estaban en la E2, ahora hay muchos mas.
Los archivos son los siguientes:
- `Carta.cs`: Se encuenta la clase Carta que tiene los atributos necesarios para el json de las cartas. (En un futuro es probable que se haga abstracta).

- `Superstar.cs`: Se encuenta la clase abstracta Superstar que tiene los atributos necesarios para el json de los superstars y tambien los metodos necesarios para que se pueda jugar el juego ya que un superstar es un jugador.

- `Game.cs`: Es la clase en donde se hace lo necesario para que se simule el juego.

- `ConjuntoCartas.cs`: Una clase en donde se tienen todas las cartas y todos los superstars sin repetición.

- `Mazo.cs`: Es la clase en donde se crea cada mazo de los archivos txt.

- `MazoValidatos.cs`: Clase estatica con la cual se valida cada una de las condiciones para el mazo.

- `Utils.cs`: Clase estatica en donde se encuentran algunos metodos funcionales para revolver el mazo, abrir los archivos json y txt y formatear las cartas.

- `EleccionesJugarCarta.cs`: Es la clase en donde se manejan las deciones de jugar o no una carta.

- `EleccionesVerCartas.cs`: Es la clase en donde se manejan las deciones de ver los distintos mazos posibles en el flujo del juego.

- `Habilidades Superstars`: Es una carpeta en donde se encuentran las clases de los superstars, en las cuales se especifica principalmente el uso de las habilidades especiales de cada tipo de superstar.