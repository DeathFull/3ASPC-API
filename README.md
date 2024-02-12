Installation du projet 3ASPC-API

Prérequis

Pour pouvoir travailler sur ce projet, assurez-vous d'avoir les outils suivants installés :

- Docker
- SDK 8.0.101

Création du conteneur Docker

1. Ouvrez un terminal et naviguez jusqu'au dossier contenant le fichier docker-compose :

   ```
   cd 3ASPC-API
   ```

2. Exécutez la commande suivante dans le terminal :

   ```
   docker compose up -d
   ```

   Cette commande permettra de créer le conteneur SQL Server nécessaire au projet.

Connexion à la base de données

Une fois le conteneur créé, vous pouvez vous connecter à SQL Server à l'aide de votre IDE préféré en utilisant les informations suivantes :

- Nom d'utilisateur : sa
- Mot de passe : monsupermotdepasse

Hôte et port SQL Server :

- Hôte : localhost
- Port : 1433

Après avoir établi la connexion, exécutez le script Launch.sql sur la branche localhost/master.

Remarque : Assurez-vous que SQL Server n'est pas déjà en cours d'exécution ailleurs que dans Docker pour éviter les conflits de port.

Il ne vous reste plus qu'à compiler le projet.

Vous pouvez tester toutes les routes à l'aide de Swagger.
