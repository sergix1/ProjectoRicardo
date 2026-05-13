# Pràctica 2.3 - ML Agents Laberint

## Autor

Sergi Martínez Vayá

## Descripció

En aquesta pràctica he utilitzat Unity ML-Agents per entrenar un agent capaç de trobar l’eixida d’un laberint utilitzant RayPerceptionSensor3D.

L’agent aprén a moure’s pel laberint evitant les parets fins arribar a l’objectiu final.

Al principi l’agent es quedava bloquejat, xocava moltes vegades amb les parets o esgotava els passos màxims. Després de diverses proves canviant recompenses, passos màxims i simplificant algunes parts del laberint, finalment va començar a completar-lo correctament en la majoria d’intents.

## Escena

L’escena està formada per diferents parets creant un laberint, un objectiu final i un agent amb Rigidbody i RayPerceptionSensor3D.

El laberint es va simplificar respecte a les primeres proves perquè l’agent tenia dificultats per aprendre en un entorn massa gran i amb recorreguts molt llargs.

## RayPerceptionSensor3D

Configuració utilitzada:

- Detectable Tags:
  - Wall
  - Objective

- Rays Per Direction: 3
- Max Ray Degrees: 70
- Ray Length: 6
- Sphere Cast Radius: 0.5
- Stacked Raycasts: 3

## Accions de l’agent

L’agent utilitza accions discretes:

- 0 → No fer res
- 1 → Avançar
- 2 → Girar esquerra
- 3 → Girar dreta

També es va implementar el mètode Heuristic() per poder provar manualment el moviment amb el teclat.

## Recompenses i penalitzacions

### Recompenses

Arribar a l’objectiu:

```csharp
AddReward(50f);
```

Acostar-se a l’objectiu:

```csharp
AddReward(distanceDelta * 0.02f);
```

### Penalitzacions

Xocar amb una paret:

```csharp
AddReward(-0.02f);
```

Penalització per temps:

```csharp
AddReward(-0.0001f);
```

Caure del mapa:

```csharp
AddReward(-0.3f);
```

## Entrenament

Durant l’entrenament es van ajustar diferents valors:

- La grandària del laberint
- El valor de MaxStep
- Les recompenses
- Les penalitzacions

Finalment l’agent va aprendre a completar el laberint de forma bastant estable.

Per comprovar l’aprenentatge es va utilitzar TensorBoard.
