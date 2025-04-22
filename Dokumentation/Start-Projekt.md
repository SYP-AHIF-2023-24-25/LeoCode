# 🚀 LeoCode – Projektstart-Dokumentation

Diese Anleitung führt dich Schritt für Schritt durch die notwendigen Schritte, um das LeoCode-Projekt lokal zu starten – vom Setup des Backends und Frontends bis hin zum Erstellen der benötigten Docker-Images für die Runner.

## 🧑‍💻 Projektstart

Öffne das Projektverzeichnis in Visual Studio Code, um effizient mit dem Code arbeiten zu können.
### 🔧 Schritt 1: Backend-Templates vorbereiten

Damit der TypeScript-Code später korrekt ausgeführt werden kann, müssen zunächst die Abhängigkeiten in den Templates installiert werden.
✅ Vorgehensweise:

    Navigiere im Terminal in folgendes Verzeichnis:

    LeoCode/backend/languages/Typescript/templates

Öffne innerhalb dieses Ordners einen beliebigen Template-Unterordner. (Hier befinden sich verschiedene Beispiel- oder Projekt-Templates für den TypeScript-Runner.)

Führe nun folgenden Befehl im Terminal aus, um alle benötigten Node-Abhängigkeiten im darüberliegenden Verzeichnis zu installieren:

    npm install --prefix ../../

    💡 Dieser Befehl installiert die Pakete im languages/Typescript-Verzeichnis, auf das die Templates zugreifen.

### 🌐 Schritt 2: Frontend vorbereiten

Damit du die Benutzeroberfläche des Projekts im Browser sehen und testen kannst, muss auch das Frontend eingerichtet und gestartet werden.


    Navigiere ins Frontend-Verzeichnis:

    LeoCode/frontend

Installiere die benötigten Node-Module (Abhängigkeiten), die für Angular benötigt werden:

    npm install

Starte anschließend das Angular-Frontend mit dem folgenden Befehl:

    npx ng serve

    🔍 Nach erfolgreichem Start kannst du die Webanwendung unter http://localhost:4200 im Browser aufrufen.

### 🐳 Schritt 3: Docker-Images für die Runner bauen

Damit die Backend-Komponenten (die sogenannten "Runner", welche Code ausführen) funktionieren, müssen passende Docker-Images gebaut werden. Es gibt einen Runner für TypeScript und einen für C#.

▶️ TypeScript Runner

    Navigiere in den Ordner für den TypeScript-Runner:

    LeoCode/backend/ts-runner

Erstelle das Docker-Image mit folgendem Befehl:

    docker build -t ts-runner .

    📦 Das Image erhält den Namen ts-runner und kann später für Container-basierte Ausführung verwendet werden.

▶️ C# Runner

    Navigiere in den Ordner für den C#-Runner:

    LeoCode/backend/csharp-runner


Baue das Docker-Image mit:

    docker build -t csharp-runner .

⚙️ Dieses Image wird für das Ausführen von C#-Code in einer isolierten Umgebung benötigt.

### 4 LeoCodeBacken Starten
Als nächstes muss das LeoCodeBackend im Visulal Studio gestarten werden um die Runner zu starten.


▶️ TypeScript Runner
![Docker-Setup](assets/swaggerTypescript.png)

▶️ C# Runner
![Docker-Setup](assets/swaggerCsharp.png)


