[![wakatime](https://wakatime.com/badge/github/dermrvn-code/bobo-bluetooth-app.svg)](https://wakatime.com/badge/github/dermrvn-code/bobo-bluetooth-app)

# Bobo Bluetooth App
Dieses Projekt enthält eine App über die ein *Pololu 32U4 Zumo* mit Bluetooth gesteuert werden kann. Der Code in diesem Projekt steht in direkter Verbindung zu einem dafür erstellten **Arduino Code** ([*Zumo-Bluetooth*](https://github.com/dermrvn-code/zumo-bluetooth)).

Diese App integriert die Möglichkeit die Motoren zu steuern, die Sensordaten des Zumos auszulesen und den Buzzer eine Melodie abspielen lassen.

Natürlich kann dieser Code beliebig um weitere Funktionen erweitert werden.

## Inhaltsverzeichnis

1. [Aufbau der App](#aufbau-der-app)

2. [Installation](#installation)

3. [Codeübersicht](#codeübersicht)

4. [Schlusswort](#schlusswort)

## Aufbau der App
![App UI](https://raw.githubusercontent.com/dermrvn-code/zumo-bluetooth-app/main/UI.png)
Nachdem die App gestartet ist, sieht man die im Bild zu sehende UI. Wenn man bereits mit dem Bluetooth Adapter des Zumos gekoppelt ist, sollte sich die App zum Start mit dem Zumo verbinden. Falls dies nicht der Dall ist, kann die Verbindung durch das Klicken auf den roten Text hergestellt werden.
  
Der Joystick kontrolliert die Richtung in welcher der Zumo fährt. Möchte man, dass der Zumo schneller fährt, so kann man den "Speed Knopf" drücken. Wenn dieser rot gefärbt ist, fährt der Zumo mit doppelter Geschwindigkeit. Durch erneutes Drücken fährt der Zumo wieder mit normaler Geschwindigkeit.
  
Das Radar in der Mitte zeigt die Sensordaten der beiden vorderen und der beiden seitlichen Sensoren an. Da der Zumo Sensorwerte zwischen 0 u. 6 schickt, zeigt das Radar je nach Wert einen Punkt auf dem Radar an.
  
Der Star Wars Knopf spielt sobald man ihn drückt die Star Wars "Imperial March" Melodie.
  
Wenn man die App im Debug Mode laufen lässt, wird in der oberen rechten Ecke ein Log Text erscheinen.

## Installation



## Codeübersicht

Im Folgenden gebe ich eine grobe Übersicht über den Code und seine allgemeine Funktionsweise. Ich werde nicht jedes Statement erklären, sondern nur die Wichtigsten Blöcke im Überblick.

Für die Kommunikation über Bluetooth habe ich folgendes Package benutzt: [Blue Unity by bentalebahmed](https://github.com/bentalebahmed/BlueUnity)

Auf den Code des Packages werde ich nicht eingehen. Ich werde mich auf die Klasse des BluetoothControllers fokussieren, da diese die meiste Arbeit übernimmt. Zur Vollständigkeit gehe ich zu Beginn über die restlichen beiden Klassen, welche das Radar steuern.

### RadarSpriteManager.cs [[zur Datei]](/Assets/Scripts/RadarSpriteManager.cs)

Der Sprite Manager ist eine Klasse, welches auf den vier Bildern des Radars liegt, welche die vier verschiedenen Sensordaten anzeigen.

Die Klasse hält die 6 verschiedenen Sprites, welcher die Richtung an Daten empfangen kann.

Die einzige Methode der Klasse ist die `SetState(int state)` Methode. Sie nimmt als Parameter einen int bzw. im Kontext den empfangenen Sensorwert.
Wenn dieser Wert zwischen 1 und 6 liegt, wird das Bild auf das für den Sensorwert richtigen Sprite gesetzt. Falls der Wert 0 oder ungültig ist, wird der Punkt dieses Bildes komplett ausgeblendet.

### RadarController.cs [[zur Datei]](/Assets/Scripts/RadarController.cs)

Der Radar Controller ist ein Platzhalter Objekt innerhalb des Radars. Die BluetoothController Klasse greift darauf zu und updated über diese Klasse das Radar anhand der empfangenen Sensordaten.
 
Die Klasse speichert zunächst alle vier Bilder des Radars und holt sich in der `void Start()` die SpriteManager Skripte, welche auf diesen Bildern liegen. Das Skript speichert diese dann für jeden Sensor.
 
Die für alle sichtbare Methode `SetScanner(int fl, int fr, int sl, int sr)` nimmt die vier Sensordaten als Paramter und ruft auf dem entsprechendem Bild die `SetState(int state)` Methode auf, welche dann das Radar an die Sensordaten anpasst.

### BluetoothController.cs [[zur Datei]](/Assets/Scripts/BluetoothController.cs)

#### Kopf des Codes

Im Kopf der Klasse werden zunächst einige Variablen definiert. Alle als `public` gekennzeichneten Variablen sind im Unity Editor editierbar. Dies ist für die Konfiguration des Bluetooth Adapter Namens oder das Aktivieren des Debug Modes interessant, da man so die App anpassen kann, ohne den Code zu ändern.
  
Wir speichern uns zunächst das Joysrick Objekt, den Speed Knopf, die Hupe bzw. den Star Wars Knopf und den Reconnect Knopf als Objekte, um sie später nutzen zu können.

Ebenso speichern wir uns den RadarController, den Connection Text innerhalb des Reconnect Knopfes und den Debug Log Text.

Wir erstellen ebenfalls die Variablen für den Debug Modus, für den Connection Status und die Variable worauf wir das RadarController Skript speichern.
  
Ich starte zunächst mit der [`Start()`](#start) Methode, danach mache ich weiter mit der [`Connect()`](#connect) Methode und im Anschluss erkläre ich alle weiteren Methoden:

- [`Update()`](#update)
- [`UpdateConnection()`](#updateconnection)
- [`ChangeSpeed()`](#changespeed)
- [`Honk()`](#honk)
- [`DebugLog()`](#debuglog)


#### Start()

Zur Beginn der Start Methode deaktivieren wir das automatische Timeout des Handys.

Danach speichern wir das radarController Skript, welches auf dem dafür vorgesehenem Objekt liegt.

Dann werden dem Reconnect, dem Speed und dem Hupen bzw. Star War Knop die jeweiligen Listener angehängt. Die Methoden dieser Listener werden später erläutert.

Zu guter Letzt wird die [`Connect()`](#connect) Methode aufgerufen und der DebugText wird je nach Debug Mode aktiviert oder deaktiviert.


#### Connect()

Diese Methode wird sowohl beim Start der App, als auch beim drücken des Reconnect Knopfes aufgerufen.

Sie erstellt zunächst ein Bluetooth Objekt, über welches im weiteren per Bluetooth kommuniziert wird.

Danach wird eine Bluetooth Verbindung über den Namen des Gerätes gestartet. Diese Funktion liefert einen Boolean je nach Erfolg der Verbindung zurück. Diesen Wert speichern wir in unserer `isConnected` Variablen.

Danach rufen wir die [`UpdateConnection()`](#updateconnection) Methode auf, welche den Text im Reconnect Button je nach Status der Verbindung updated.

Zum Schluss loggen wir nun den Erfolg bzw. Misserfolg des Verbindungsaufbaus.

#### UpdateConnection()

Diese Methode updated lediglich den Text innerhalb des Reconnect Buttons je nachdem ob eine Verbindung besteht oder nicht. Die Methode ändert ebenfalls die Textfarbe dementsprechend.

#### Update()
#### ChangeSpeed()

Diese Methode wird beim Drücken des Speed Knopfes aufgerufen.
Sie wechselt je nach aktuellem Status der `speedMultiplier` Variable den Wert (2 <--> 1). Sie ändert dementsprechend auch die Farbe des Speed Knopfes und loggt den neuen Werte der `speedMultiplier` Variable.

#### Honk()

Diese Methode wird aufgerufen, sobald die Hupe bzw. der Star Wars Knopf gedrückt wird.

Sie erstellt zuerst den Befehl `"h;"`, welcher per Bluetooth gesendet wird. Diesen Befehl loggt die Methode und versucht dann den Befehl per Bluetooth zu senden.

Falls das Senden nicht gelingt, wird die Fehlernachricht geloggt und der Verbindungsstatus auf nicht verbunden gesetzt.

#### DebugLog()

Diese Methode updated, wenn der `debugMode` aktiv ist, den Text innerhalb des DebugLogTextes, indem er den über den Paramter erhaltenen String anhängt.

## Schlusswort

Vorschläge und Verbesserungen sind gerne gesehen. Der Code ist nicht perfekt, aber tut was er soll. Wenn jemand Fehler entdeckt oder eine bessere Lösung für Dinge hat, bin ich offen dafür, diese in das Projekt zu integrieren.

Ich hoffe der Code kann gut genutzt werden oder zumindest als Inspiration für jemanden dienen.
