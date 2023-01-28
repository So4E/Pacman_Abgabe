# Pacman3D
 
## Spiel bitte über Scene: "main_menu" starten 
Szene startet dann automatisch im Main Screen und öffnet die game-Szene bei Klick auf 'Start Game'. Wenn diese direkt geöffnet wird, sind einige Funktionalitäten wie Persistierung des Scores nicht verfügbar.

## Steuerung
Pacman kann über A und D oder die rechte und linke Pfeiltaste gesteuert werden
Beim Drücken der Leerzeichen-Taste ändert sich die Ansicht zu einer Top-View, die solange verfügbar ist, wie der Balken am unteren Bildschirmrand angezeigt wird. Ist dieser leer, ändert sich die View automatisch wieder und man muss warten, bis der Balken sich wieder aufgeladen hat. (Faktor ist hier die Zeit)

### Pacman 
Hat drei Leben.

### Geister
Trifft ein Geist Pacman, verliert er ein Leben.

### Kirschen
Frisst Pacman eine Kirsche kann er Geister für eine bestimmte Zeit fressen

### Dots 
Pacman sollte so viele Dots wie möglich sammeln -> geben Punkte

### Portale (grün)
Über Portale kann sich Pacman in einen neu generierten Raum/ eine neue Welt teleportieren. Die Punkte und Leben bleiben erhalten.
 
 

## Funktion Weltgenerator: World Generation V2 Skript
 
### RootNode
Das Gameobject, welches die Welt als Kind beherbergen wird

###Levelsize
Die Größe der Spielwelt (oben 31x31)

### FloorCount
Die Anzahl der Ebenen die übereinander generiert werden und mit Treppen/Rampen verbunden werden

### Openess
Bestimmt wie verschachtelt das Level ist, bei einem Wert von 1 gibt es keine Wände und Hinderniss mehr; bei einem Wert von 0 gibt es die maximale Anzahl an Wänden/Hindernissen die möglich ist ohne das die Welt unbespielbar wird.

### GlobalScale
Scaliert die gesamte Welt so das Sie auf die Größe von Items/Pacman angepasst werden kann

### XY_offset
Ermöglicht das verschiedene Dimension von Bodenkacheln(In verschiedenen Tilesets) genutzt werden können, der Offset bestimmt den Abstand vom Center einer Kachel zur nächsten

### Z_offset
Abstand zwischen den Ebenen

### PathToWorldGenerationPrefab
Der Pfad zu dem Ordner in dem sich die Prefabs befinden die zur Generierung der Welt genutzt werden sollen, so lässt sich die Erscheinung der Welt schnell ändern und ist modular austauschbar.
 
 ## Quellen

### Unity Dokumentation: 
https://docs.unity3d.com/Manual/index.html 

### Jimmy Vegas HOW TO SWITCH CAMERAS ON KEY PRESS WITH C# UNITY TUTORIAL 
https://www.youtube.com/watch?v=wWTOuggRvgc 


### DevDuck EASY Unity Progress Bar Tutorial + Particles! [2019]
https://www.youtube.com/watch?v=UCAo-uyb94c

### Antwort 3 by "Oen44" zur Frage "How can I get all prefabs from a Assets folder? Getting not valid cast exception in the editor"
https://stackoverflow.com/questions/53968958/how-can-i-get-all-prefabs-from-a-assets-folder-getting-not-valid-cast-exception


### Breadth First Search grid shortest path | Graph Theory by WilliamFiset
https://www.youtube.com/watch?v=KiCBXu4P-2Y


### Unity api:
https://docs.unity3d.com/Manual/index.html
### Unity scripting api: 
https://docs.unity3d.com/ScriptReference/index.html
https://www.youtube.com/watch?v=y7RPVvwjrsA (20.12.22)

### Grid System:
https://www.youtube.com/watch?v=HbKbxN6Oo6I (12.12.22)
https://www.youtube.com/watch?v=rKp9fWvmIww (12.12.22)
https://www.youtube.com/watch?v=G4aAUodsU3o (12.12.22)
https://www.youtube.com/watch?v=ulFc6p3hQzQ (12.12.22)
https://www.youtube.com/watch?v=64NblGkAabk (12.12.22)

### Level Generator;
https://www.youtube.com/watch?v=NtY_R0g8L8E (12.12.22)
https://www.youtube.com/watch?v=gembmFnhiUs (lighting: 19.12.22)

### Menü:
https://www.youtube.com/watch?v=KXFLp3EMBeI&t=2s (20.12.22)
https://stackoverflow.com/questions/31426997/applying-an-image-to-a-unity-ui-panel (06.01.22)

### Camera:
https://www.youtube.com/watch?v=qnjKoTmko3Q (20.12.22)
https://answers.unity.com/questions/21909/rounding-rotation-to-nearest-90-degrees.html (20.12.22)

### time
https://www.youtube.com/watch?v=x-C95TuQtf0&t=323s (20.12.22)

### Collider:
https://stackoverflow.com/questions/64089586/how-to-handle-non-convex-meshcollider-with-non-kinematic-rigidbody-is-no-longer (15.12.22)

### change Pivot Pont of GameObjects
https://forum.unity.com/threads/change-an-objects-pivot-point.22885/ (17.12.22)
https://answers.unity.com/questions/673355/cannot-rotate-an-object-around-the-center.html (20.12.22)

### Button issues
https://answers.unity.com/questions/1697183/button-onclick-properties-are-missing-after-loadin.html (20.12.22)
https://forum.unity.com/threads/changing-textmeshpro-text-from-ui-via-script.462250/ (20.12.22)
https://answers.unity.com/questions/627750/the-variable-has-not-been-assigned-but-it-has.html (20.12.22)
https://answers.unity.com/questions/1236751/button-onclick-after-scene-restart-is-missing-obje.html (20.12.22)
https://forum.unity.com/threads/missing-reference-after-reloading-a-scene-but-the-referencing-object-still-exist.383648/ (21.12.22)

### save Game Data
https://www.youtube.com/watch?v=y7RPVvwjrsA (20.12.22)
