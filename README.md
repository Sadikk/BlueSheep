# BlueSheep

> **DISCONTINUED**
> Ce  bot ne suit plus les mises à jour de Dofus depuis 2015.

BlueSheep est un bot pour le MMORPG [Dofus](https://fr.wikipedia.org/wiki/Dofus) développé en C# et rendu public de Septembre 2014 à Avril 2015.

> *Qu'est-ce qu'un bot ?*

Un bot est un logiciel permettant d'**émuler** un client de jeu réel dans le but **d'automatiser** certaines tâches en jeu. Ici, BlueSheep était un bot 100% en mode 'socket', ce qui signifie qu'aucune fenêtre du jeu Dofus n'avait besoin d'être ouverte pour que le bot fonctionne. 


## Historique

Suite à la fermeture du populaire bot dofus RedoxBot, le projet BlueSheep a été lancé ayant pour vocation de rendre de nouveau disponible un bot Dofus fonctionnel pour le grand public.
Il devient très vite populaire malgré son instabilité, faute de meilleure alternative.

Son développement est toutefois entravé par le développement de mesures anti-bot par la maison mère du jeu, Ankama Games. (Qui est d'ailleurs la raison de la fermeture de RedoxBot).

La situation est illustrée de manière humoristique dans cette vidéo : https://www.youtube.com/watch?v=HLfhWIYAauk

Le projet est rendu Open-Source en février 2015 afin d'attirer de potentiels développeurs sur le projet.
Cela ne fonctionne qu'un temps et je marque le projet en "discontinued" en Avril 2015, pour voquer à de nouveaux horizons.

## Fonctionnalités

![Capture d'écran du bot à sa sortie](https://i.imgur.com/mpEsIxj.png)*Capture d'écran de la première version du bot*

Le bot répliquait le protocole utilisé par Dofus pour effectuer diverses actions en jeu sans intervention de l'utilisateur, notamment : 

 - Connexion en jeu
 - Déplacement du personnage
 - Combat contre les monstres
 - Récolte des ressources

Afin de permettre une utilisation lucrative par les utilisateurs, différentes fonctionnalités d'automatisation étaient intégrées :

 - Système de trajet configurable, permettant de déclarer une route finie ou non (boucle) que doit suivre le personnage et d'effectuer des actions particulières selon sa position (combat, récolte, déplacement). Cela passait par des fichiers textes dans un format défini de "script de trajet", aisément compréhensible pour permettre l'édition, et qui étaient interprétées par le bot.
 - Gestion de la banque : lorsque le personnage est "plein" (inventaire complet de ressources par exemple), il change en mode "banque" pour suivre un autre trajet pour aller déposer ses ressources dans une banque ou un coffre personnel de maison
 - etc..

### Syntaxe des trajets 

<details>
  <summary>Exemple d'un trajet de récolte à bonta</summary>
  
```
@NAME      [Artisanat] Full Blé + Craft
@LOCATION  Bonta
@TYPE      Artisanat & Récolte
@VERSION   1.0
@AUTHOR    Sadik

<Lost>
[-26,-41] 
</Lost>

<Move>
2885641 424
2884617 424
2883593 424
[-31,-54] droite
[-30,-54] droite
[-29,-54] bas
[-29,-53] bas
[-29,-52] bas
[-29,-51] bas
[-29,-50] bas
[-29,-49] bas
[-29,-48] bas
[-29,-47] bas
[-29,-46] bas
[-29,-45] bas
[-29,-44] bas
[-29,-43] droite
[-28,-43] droite
[-25,-43] bas
[-30,-42] droite
[-29,-42] droite
[-28,-42] droite
[-27,-42] droite
[-26,-42] bas
[-30,-41] haut
[-29,-41] bas
[-25,-41] bas
[-23,-41] gauche
[-29,-40] bas
[-27,-40] haut
[-26,-40] gauche
[-30,-39] haut
144681 use(464698)
39845888 craft
</Move>

<Gather>
[-27,-43] droite
[-26,-43] droite
[-25,-42] droite
[-24,-42] droite
[-23,-42] bas
[-28,-41] gauche
[-27,-41] gauche
[-24,-41] gauche
[-30,-40] haut
[-25,-40] gauche
[-29,-39] gauche
</Gather>

<Bank>
39845888 465
[-30,-54] gauche
[-29,-54] gauche
[-29,-53] haut
[-29,-52] haut
[-29,-51] haut
[-29,-50] haut
[-29,-49] haut
[-29,-48] haut
[-29,-47] haut
[-29,-46] haut
[-29,-45] haut
[-29,-44] haut
[-29,-43] haut
[-28,-43] gauche
[-27,-43] gauche
[-26,-43] gauche
[-25,-43] gauche
[-30,-42] droite
[-29,-42] haut
[-28,-42] gauche
[-27,-42] gauche
[-26,-42] gauche
[-25,-42] gauche
[-24,-42] gauche
[-23,-42] gauche
[-30,-41] droite
[-29,-41] haut
[-28,-41] gauche
[-27,-41] gauche
[-26,-41] gauche
[-25,-41] gauche
[-24,-41] gauche
[-23,-41] gauche
[-30,-40] droite
[-29,-40] haut
[-28,-40] gauche
[-27,-40] gauche
[-26,-40] gauche
[-25,-40] gauche
[-30,-39] droite
[-29,-39] haut
147254 use(433935)
2885641 npc
</Bank>
```

</details>

### Protocol

BlueSheep suit le protocole de Dofus 2.X, basé sus un système de messages écrits en BigEndian.

Exemple d'échange :

```
[18:09:47:456] [ServerConnection] [SND] > GameMapMovementCancelMessage @70
[18:09:47:476] [RoleplayMovementFrame] Discarding a movement path that begins and ends on the same cell (478).
[18:09:47:751] [ServerConnection] [SND] > GameMapMovementRequestMessage @72
[18:09:47:785] [ServerConnection] [RCV] GameMapMovementMessage @73
[18:09:48:311] [ServerConnection] [SND] > GameMapMovementConfirmMessage @75
[18:09:48:770] [ServerConnection] [SND] > GameMapMovementRequestMessage @77
[18:09:48:785] [ServerConnection] [RCV] GameMapMovementMessage @78
[18:09:49:350] [ServerConnection] [SND] > GameMapMovementConfirmMessage @80
[18:09:49:352] [ServerConnection] [SND] > GameRolePlayAttackMonsterRequestMessage @81
[18:09:49:413] [ServerConnection] [RCV] GameContextRemoveElementMessage @83
[18:09:49:427] [ServerConnection] [RCV] LifePointsRegenEndMessage @84
[18:09:49:427] [ServerConnection] [RCV] GameContextDestroyMessage @85
```

## Communauté

Le projet est supporté par une communauté d'utilisateurs qui pouvaient échanger sur le forum de BlueSheep.

![Forum BlueSheep](https://i.imgur.com/5ROAxk5.png)
Le forum comptabilisait près de sa fermeture + de **1300 membres** et + de **6000 messages**.

### En bref, une super aventure !

![](https://i.imgur.com/jdQcZEG.jpg)
