# RobotPlanet

## Valitud rakenduse eesmärk

Valisin maailma Robot Planet. Rakenduses saab luua erinevaid roboteid, kellel on erinevad tööülesanded ja crazyactionid. Projekti eesmärk oli harjutada objektorienteeritud programmeerimist, kasutades pärilust, liideseid, polümorfismi ja WPF kasutajaliidest.

## Robotite tüübid:

- CleanerBot
- ExplorerBot
- RepairBot
- GuardBot
- BuilderBot (Kaasüliõpilase lisatud)

## Käivitamisjuhend

1. Ava Visual Studio
2. Veendu, et WPF oleks startup projekt.
3. Käivita Projekt (F5)
4. Sisesta roboti nimi.
5. Vali roboti tüüp
6. Vajuta "Add Robot"
7. Vali Robot nimekirjast
8. Kasuta erinevaid tegevusnuppe, et erinevaid tegevusi teha.

## Klassihierarhia ja liidesed

### Robot
- CleanerBot
- ExplorerBot
- RepairBot
- GuardBot
- BuilderBot

### Liidesed

**Iscan**
- Laseb Robotitel skaneerida.
- Rakendub ExplorerBotile ja GuardBotile

**Irepair**
- Võimaldab robotil parandada.
- Rakendub ainult RepairBotile

**Ichargeable**
- Võimaldab robotitel laadida.
- Rakendub kõikidele robotitele.

## CrazyAction tegevused ja olekureeglid

**CleanerBot**
- Koristab terve planeedi ära.

**ExplorerBot**
- Uurib kogu planeedi läbi.

**RepairBot**
- Kogemata uuendab iseennast.

**GuardBot**
- Jääb tööl magama (Kui Akut ei ole)
- Arreteerib müügiautomaadi (Kui akut on)

**BuilderBot**
- Ehitab banaanikujuliselossi kuule.

### Olekureeglid

- Roboti nimi ei tohi olla tühi
- Aku väärtus peab olema vahemikus 0-100
- Aku muutub ainult roboti tegevuse kaudu, crazyaction seda ei võta.
- Vigased sisendid ei muuda olemasolevat olekut.

## Kontrollitud kasutusjuhud
1. Uue roboti lisamine nime ja tüübi valimisega.
2. Roboti töötegevuse käivitamine nupuga TASK
3. Roboti laadimine nupuga CHARGE.
4. Roboti Crazy Actioni tegevuse käivitamine.
5. Roboti eemaldamine.
6. Skaneerimine robotitega, kes toetavad Iscan liidest.
7. Parandamine robotitega, kes toetavad Irepair liidest.
8. Kontrollisin, et robotid saaksid kasutada ainult neid tegevusi, mille jaoks neil vastav liides olemas on.

## Valitud WPF UI element

**Valisin ProgressBari.**

ProgressBar näitab valitud roboti aku taset visuaalselt ning teeb aku muutuste jälgimise lihtsamaks.

## Kaasüliõpilase panus

**Kaasüliõpilane:** Alger Bronzov

**Issue:** https://github.com/janlaende-hue/RobotPlanet/issues/1#issue-5762212153

**Pull Request:** https://github.com/janlaende-hue/RobotPlanet/pull/2#issue-5784117946

## AI kasutamine

Kasutasin projekti tegemisel CoPilotit. Ma kasutasin AId peamiselt errorite parandamisel ning mõnede Githubi funktsioonide õppimisel. Koodi kontrollisin ise, tegin vajalikud muudatused ning testisin rakendust enne lõplikku esitamist.
