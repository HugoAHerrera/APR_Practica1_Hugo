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

# Hecho por: Hugo Andrés Herrera de Miguel
