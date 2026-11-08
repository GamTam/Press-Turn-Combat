# Game Description

This game aims to re-creates the Press Turn turn-based combat system from the Shin Megami Tensei games, where two teams taking turns attacking each other. How many actions (referred to as "press turns") each team gets depends of various factors.
Generally, the amount of press turns a team gets per turn is the amount of fighters on their team that are alive at the start of the turn. If a fighter is hit by an element that they are weak to, then half of a press turn is taken up, allowing more actions on the opposing side. Conversely, if a fighter is hit by an element they resist, then two press turns are taken up. Finally, if a fighter is hit by an element they null, reflect, or absorb, then all press turns are used immediately.

When the application is opened, the player gets a view of a battle scene. The player team is on the right, and the enemy team is on the left. They have four options: Fight, Skill, Pass, and Guard. Fight and Skill use different actions based on which party member's action it is. Pass takes up half a press turn, and passes to the next party member. Guard takes up an entire press turn, but damage received is reduced and if a guarded party member is hit by an element their weak against, an entire press turn is taken up.

## Affinity Cheat Sheet
**Element Types**
|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Strike.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Slash.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Pierce.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Fire.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Ice.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Elec.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Wind.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Light.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Dark.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Almighty.webp)|
|-----|----|----|----|----|----|----|---|---|---|
|Strike|Slash|Pierce|Fire|Ice|Elec|Wind|Light|Dark|Almighty|

<br><br>

### Player Team Affinities

<br>

**Yosuke Hanamura**
|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Strike.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Slash.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Pierce.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Fire.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Ice.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Elec.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Wind.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Light.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Dark.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Almighty.webp)|
|-----|----|----|----|----|----|----|---|---|---|
||||||WEAK|RESIST||||

|Moves|Element|
|---|---|
|Basic Attack|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Slash.webp)|
|Garu|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Wind.webp)|

<br>

**Ryoji Mochizuki**
|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Strike.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Slash.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Pierce.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Fire.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Ice.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Elec.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Wind.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Light.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Dark.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Almighty.webp)|
|-----|----|----|----|----|----|----|---|---|---|
|RESIST|RESIST||||||WEAK|ABSORB||

|Moves|Element|
|---|---|
|Basic Attack|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Pierce.webp)|
|Eiha|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Dark.webp)|

<br>

**Will McDonathan**
|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Strike.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Slash.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Pierce.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Fire.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Ice.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Elec.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Wind.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Light.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Dark.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Almighty.webp)|
|-----|----|----|----|----|----|----|---|---|---|
||||WEAK|||RESIST||RESIST||

|Moves|Element|
|---|---|
|Basic Attack|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Strike.webp)|
|Bufu|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Ice.webp)|

<br><br>

### Enemy Team Affinities

<br>

**Tohru Adachi**
|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Strike.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Slash.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Pierce.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Fire.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Ice.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Elec.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Wind.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Light.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Dark.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Almighty.webp)|
|-----|----|----|----|----|----|----|---|---|---|
||||||RESIST|WEAK||NULL||

|Moves|Element|
|---|---|
|Basic Attack|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Pierce.webp)|
|Zio|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Elec.webp)|

<br>

**Louis Guiabern**
|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Strike.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Slash.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Pierce.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Fire.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Ice.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Elec.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Wind.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Light.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Dark.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Almighty.webp)|
|-----|----|----|----|----|----|----|---|---|---|
|NULL|NULL|NULL||WEAK||||||

|Moves|Element|
|---|---|
|Basic Attack|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Slash.webp)|
|Cleave|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Slash.webp)|
|Agi|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Fire.webp)|
|Dia|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Recovery.webp)|

<br>

**Goro Akechi**
|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Strike.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Slash.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Pierce.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Fire.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Ice.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Elec.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Wind.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Light.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Dark.webp)|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Almighty.webp)|
|-----|----|----|----|----|----|----|---|---|---|
||||||||WEAK|NULL||

|Moves|Element|
|---|---|
|Basic Attack|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Strike.webp)|
|Eiha|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Dark.webp)|
|Getsu-ei|![](https://github.com/GamTam/Press-Turn-Combat/blob/main/Affinity%20Icons/P3R_Icon_Slash.webp)|
