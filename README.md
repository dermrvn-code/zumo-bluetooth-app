[![wakatime](https://wakatime.com/badge/github/dermrvn-code/bobo-bluetooth-app.svg)](https://wakatime.com/badge/github/dermrvn-code/bobo-bluetooth-app)

# Bobo Bluetooth App
Dieses Projekt enthält eine App über die ein *Pololu 32U4 Zumo* mit Bluetooth gesteuert werden kann. Der Code in diesem Projekt steht in direkter Verbindung zu einem dafür erstellten **Arduino Code** ([*Zumo-Bluetooth*](https://github.com/dermrvn-code/zumo-bluetooth)).

Diese App integriert die Möglichkeit die Motoren zu steuern, die Sensordaten des Zumos auszulesen und den Buzzer eine Melodie abspielen lassen.

Natürlich kann dieser Code beliebig um weitere Funktionen erweitert werden.

## Inhaltsverzeichnis

1. [Aufbau der App](#aufbau-der-app)

2. [Installation](#installation)

3. [Codeübersicht](#codeübersicht)

4. [Code erweitern](#code-erweitern)

5. [Schlusswort](#schlusswort)

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

Auf den Code des Packages werde ich nicht eingehen. Ich werde mich auf die Klasse des [BluetoothControllers]() fokussieren, da diese die meiste Arbeit übernimmt. Zur Vollständigkeit gehe ich zu Beginn über die restlichen beiden Klassen, welche das Radar steuern.

### RadarSpriteManager.cs [[zur Datei]](/Assets/Scripts/RadarSpriteManager.cs)

#### Kopf des Codes

Auf die dann definierten Funktionen gehe ich zum Schluss ein. Zunächst beschäftigen wir uns mit der [`void setup()`](#setup) und der [`void loop()`](#loop).

#### start()



#### update()



#### splitCommands()

`void splitCommands(String command, String *cmd, String *arg0, String *arg1, String *arg2)`

Diese Funktion macht nichts anderes, als einen `String command` zu nehmen und diesen in vier Strings zu teilen.

Dies macht diese Funktion, indem sie den Hauptstring an den Leerzeichen teilt. Der erste Teil wird dann über einen Pointer dem `String *cmd` übergeben. Der zweite, dritte und vierte Teil wird, ebenfalls über Pointer, den Strings der Argumente `arg0, arg1, arg2` übergeben.

Da C++ bzw. Arduino keine einfache split Methode zur Verfügung stellt, habe ich mich für diese Methode des Teilens entschieden.

<sub>*Verbesserungsvorschläge sind gerne gesehen*</sub>

#### commands()

`void commands(String command)`

Diese Funktion bearbeitet die Befehle, welche per Bluetooth hineinkommen und führt je nach Befehl die Aktion durch.

Dafür werden zunächst die vier Strings angelegt, die in die [`splitCommands()`](#splitcommands) Funktion gefüttert werden.

Einer dieser Strings (`cmd`) enhält den Befehl, welcher auszuführen ist. Dieser String wird dann in einer if-Abfrage überprüft. Da der Zumo bisher nur den *move* und den *honk* Befehl annimmt (Keyword *m* und Keyword *h*), wird nur nach diesem gesucht. Dabei wird zur Aussortierung von fehlerhaften Befehlen überprüft, ob der ursprüngliche Befehlsstring die richtige Länge hat und nicht zu wenig oder zu viele Zeichen gesendet hat.

Wenn dies erfüllt ist, werden in dem mover-Block Argumente in Zahlen umgewandelt und die Werte werden zurück ins Negative umgewandelt.
Bei dem honk-Block wird dann die vorher definierte StarWars Melodie gespielt.

Zur Vereinfachung schickt die App keine negativen Werte im move-Block. Der Wert 300 entspricht einer 0. Dementsprechend ist die 0 = -300 und die 600 = 300. Diese Umwandelung wird gemeinsam mit der Konvertierung zur Zahl durchgeführt.

Diese Integer werden nun einfach an den Motor des Zumos gesendet.

#### sendSensorData()

`void sendSensorData(int frontLeft, int frontRight, int sideLeft, int sideRight)`

Diese Funktion sendet die als Parameter angegebenen SensorDaten über Bluetooth an die App.

Da die App die Sensordaten über das Keyword *sd*(sendData) empfängt, wird dieses an den Anfang gesetzt. Danach werden die Sensordaten angehängt, sodass der Befehl nach dem Schema `sd <frontLeft> <frontRight <sideLeft> <sideRight>` aufgebaut ist.

Dieser String wird nun einfach über den Bluetooth Serial gesendet.

## Code erweitern

Im Folgenden werde ich erklären, wie der Code um mehr Funktionen erweitert werden kann.

Die aktuellen Befehle der App sehen wie folgt aus:

**Ausgehend**

`m <leftSpeed> <rightSpeed>`

Motordaten zwischen 0-600

`h`
  
 Keine Argumente  

**Eingehend**

`sd <frontLeft> <frontRight> <sideLeft> <sideRight>`

Sensordaten zwischen 0-6

Nun kann man sich natürlich weitere Befehle ausdenken.

Als Beispiel für einen einkommenden Befehl könnte man nun den Buzzer des Zumos über direkte Töne ansteuern.

Dafür würde man sich einen Befehl ausdenken.

Nehmen wir nun einfach mal folgenden Aufbau:

`b <note> <duration> <volume>`

Diese Syntax orientiert sich an den drei Parametern, die die [`playNote()`](https://pololu.github.io/pololu-buzzer-arduino/class_pololu_buzzer.html#a989d410dd6cdb7abfa136c3734040fb5) Funktion des Buzzers nimmt.

  

Dieser Befehl muss nun natürlich auch in der App integriert werden. Darauf gehe ich jedoch in der Dokumentation der [**Zumo-Bluetooth-App**](https://github.com/dermrvn-code/zumo-bluetooth-app) ein. Hier beschränken wir uns nun erstmal nur auf den Zumo Code.

Nun wäre der erste Schritt in die [`commands()`](#commands) Funktion zu gehen und eine weitere Abfrage nach dem `if (cmd == "m")` zu machen.

In unserem Falle wäre dies `else if(cmd == "b")`.

Nun kann man je nach Einheitlichkeit des Befehls noch Abfragen, ob der Befehl der konventionellen Länge entspricht. Da ich aber hier nicht definieren werde, in welchem Format z.B. die Note in dem Befehl geschickt wird, gebe ich hier keinen Wert an. Diese Abfrage kann also weggelassen oder durch andere bessere Sicherungen ersetzt werden.

Nun müssen unsere drei Befehlsargumente nur noch in die von [`playNote()`](https://pololu.github.io/pololu-buzzer-arduino/class_pololu_buzzer.html#a989d410dd6cdb7abfa136c3734040fb5) geforderten Parameter konvertiert und dann in die Funktion eingegeben werden.

Da diese Konvertierung abhängig von dem gewählten Format der Note, der Dauer und der Lautstärke ist, gebe ich hierfür kein Beispiel.

Zu beachten bei eingehenden Befehlen ist jedoch, dass man neben dem Befehlswort mit dem aktuellen Code nur drei Argumente zur Verfügung hat. Dies kann man natürlich erweitern, jedoch werde ich darauf hier nicht eingehen.

Nun kann man natürlich ähnlich auch ausgehende Befehle erstellen.

Beim Zumo könnte man nun die Liniensensoren auslesen und sie an die App schicken.

Hierfür denkt man sich nun wieder einen Befehl aus.

`sld <left> <middle> <right>`

Nach diesem Schema kann man sich nun einfach eine Funktion schreiben, die der [`sendSensorData`](#sendsensordata) Funktion ähnelt. Nur muss man hier den Befehl mit `sld` (*sendLineData*) starten und dann die Liniendaten anhängen. Dieses Kommando sendet man dann einfach in den Bluetooth Serial.

Diese Funktion wird dann einfach nach der [`sendSensorData`](#sendsensordata) Funktion aufgerufen und fertig ist die Ausgabe der Liniensensordaten.

Natürlich muss die App dann auch um die entsprechenden Funktionen erweitert werden. Darauf gehe ich aber, wie bereits erwähnt, in der Dokumentation der [**Zumo-Bluetooth-App**](https://github.com/dermrvn-code/zumo-bluetooth-app) ein.

Mit dieser Grundidee lässt sich dieser Code (und der der App) nun beliebig erweitern. Natürlich lässt sich dieser Code auch so weit umgestalten, dass nicht unbedingt ein Zumo, sondern irgendein Arduino darüber gesteuert werden kann.  

## Schlusswort

Vorschläge und Verbesserungen sind gerne gesehen. Der Code ist nicht perfekt, aber tut was er soll. Wenn jemand Fehler entdeckt oder eine bessere Lösung für Dinge hat, bin ich offen dafür, diese in das Projekt zu integrieren.

Ich hoffe der Code kann gut genutzt werden oder zumindest als Inspiration für jemanden dienen.
