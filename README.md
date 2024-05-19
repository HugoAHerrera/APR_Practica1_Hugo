# Practica 1: Pathfinding

### 1. Objetivo
  El objetivo del jugador es sobrevivir 120 segundos durante el nivel mientras los enemigos le persiguen. Si le tocan antes de ese tiempo, pierde la partida
  
### 2. Contenido
  El juego cuando con un menú básico donde aparece el título, el botón de jugar y un texto informativo
  El nivel está compuesto por varias obstáculos donde hay rampas y escaleras.
  Cuando con 4 OffMeshLinks, 2 NavMeshSurface, 2 NavMeshModifier, 2 NavMeshModifierVolume, 2 NavMeshLink, 2 agentes (una el jugador y otra el enemigo) con su respectiva área.
  La diferencia entre los dos agentes está en que los enemigos no pueden ni usar los OffMeshLinks ni los NavMeshlinks, y tampoco pueden subir escaleras.

### 3. Comunicación con el usuario
  El nivel de juego cuenta con una UI que la muestra al usuario el tiempo restante que debe aguantar, además, cuando termina el tiempo, se muestra por consola el mensaje "Sobreviviste"

# Practica 2: Consultas SQL
### Dos dificultades:
  Ahora el jugador puede elegir distinta dificultad. Normal = spawnea un enemigo cada 10 segundos. Difícil = cada 5 segundos
### Tablas:
  La base de datos cuenta con 3 tablas. La primera es la de Jugadores, cuenta con los atributos id y nombre del jugador que este lo puede elegir el jugador.
  La segunda es Partidas, estas son las partidas que ha muerto o ha ganado, en esta se contabilizan todas las partidas que hacen todos los jugadores, donde se almacenan datos como la dificultad seleccionada, hora de juego y tiempo sobrevivido.
  Por último, los Intentos, esta tabla sirve como control global de todas las veces que han iniciado partida los jugadores y han ganado o perdido.
### Insert y update:
  Estas consultas se hacen para registrar a los nuevos jugadores o actualizar sus estadísticas en las partidas e intentos.
### Delete:
  El jugador puede borar el historial de su cuenta introduciendo su nombre de usuario y pulsando en el botón borrar.
### JSON y XML:
  A los jugadores se les permite guardar los marcadores de todo el juego en formato XML y JSON, aquí se realiza una búsqueda que implica las 3 tablas.

# Hecho por: Hugo Andrés Herrera de Miguel
