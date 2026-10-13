# P7CreateRestApi

API REST en **C# / .NET 6** pour la gestion de six entités métier (offres, points de courbe, notations, règles, transactions et utilisateurs), sécurisée par **JWT**.

Projet 7 de la formation *Développeur back-end .NET* (OpenClassrooms), réalisé à partir d'un dépôt de départ fourni par l'école.

---

## Sommaire

1. [Fonctionnalités](#fonctionnalités)
2. [Architecture](#architecture)
3. [Prérequis](#prérequis)
4. [Installation](#installation)
5. [Utilisation](#utilisation)
6. [Routes de l'API](#routes-de-lapi)
7. [Sécurité](#sécurité)
8. [Validation des données](#validation-des-données)
9. [Logs](#logs)
10. [Tests](#tests)
11. [Choix techniques](#choix-techniques)

---

## Fonctionnalités

- CRUD complet sur les tables **BidList**, **CurvePoint**, **Rating**, **RuleName**, **Trade** et **User**
- Authentification par **token JWT** et protection de toutes les routes métier
- Mots de passe **hashés** (jamais stockés en clair)
- Validation des champs (obligatoires, intervalles numériques, règles de mot de passe) avec **messages d'erreur en français**
- **Journalisation** de chaque appel d'endpoint (URL, méthode, utilisateur, résultat)
- Tests unitaires sur les services et sur la validation des données

## Architecture

```
P7CreateRestApi/
├── Controllers/     Routage HTTP, délègue au Service
├── Service/         Logique métier, validation, mapping Model → Domain
│   └── Interfaces/
├── Repositories/    Accès aux données (CRUD pur)
│   └── Interfaces/
├── Domain/          Entités mappées sur les tables (Entity Framework)
├── Models/          Objets reçus par l'API (formulaires), dont Models/UserModel
├── Data/            LocalDbContext
├── Middleware/      LogEndpointMiddleware
├── Attribute/       PasswordValidAttribute (règles de mot de passe)
└── Ressource/       Messages d'erreur (.resx en français)

Test-P7CreateRestApi/  Tests unitaires (xUnit)
```

Le flux d'une requête est toujours : **Controller → Service → Repository → Base de données**.

| Couche | Rôle |
|---|---|
| Controller | Reçoit la requête HTTP, renvoie la réponse |
| Service | Valide, applique la logique, prépare l'entité à enregistrer |
| Repository | Lit et écrit en base, sans aucune logique métier |

## Prérequis

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- SQL Server (Developer ou Express) et, au choix, SSMS pour consulter la base
- Visual Studio 2022 ou plus récent (recommandé pour la console du gestionnaire de packages)

## Installation

### 1. Récupérer le projet

```bash
git clone <url-du-depot>
```

### 2. Configurer la base de données

Ouvrir `P7CreateRestApi/appsettings.json` et adapter la chaîne de connexion à votre machine :

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=P7CreateRestApiDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

- `Server=.\\SQLEXPRESS` convient à une installation **SQL Server Express**.
- Pour une instance par défaut (SQL Server Standard / Developer), utiliser `Server=.`.
- Le nom de la base (`Database=`) est libre, elle sera créée automatiquement.

### 3. Configurer la clé JWT

Toujours dans `appsettings.json` :

```json
"Jwt": {
  "Key": "...",
  "Issuer": "P7CreateRestApi",
  "Audience": "P7CreateRestApi",
  "ExpireMinutes": 60
}
```

La clé fournie est **temporaire** : pour un usage réel, la remplacer par une valeur secrète longue, idéalement stockée dans une variable d'environnement et non dans le dépôt.

### 4. Créer la base de données (Code First)

La migration `InitialCreate` est déjà présente dans le projet. Dans Visual Studio, ouvrir **Outils → Gestionnaire de packages NuGet → Console du gestionnaire de packages**, choisir `P7CreateRestApi` comme **projet par défaut**, puis exécuter :

```powershell
Update-Database
```

Les six tables sont créées dans la base indiquée par la chaîne de connexion.

### 5. Lancer l'API

Démarrer le projet `P7CreateRestApi` (F5). Swagger s'ouvre à l'adresse :

```
https://localhost:7210/swagger
```

## Utilisation

### Créer un premier compte

Dans Swagger, appeler `POST /User/Creation` avec un corps de ce type :

```json
{
  "username": "mon.utilisateur",
  "password": "Exemple1!",
  "fullname": "Prénom Nom",
  "role": "Admin"
}
```

### Obtenir un token

Appeler `POST /Login/Token` avec :

```json
{
  "username": "mon.utilisateur",
  "password": "Exemple1!"
}
```

La réponse contient le token JWT.

### Appeler les routes protégées

Dans Swagger, cliquer sur **Authorize** et coller le token (sans le mot `Bearer`).
Avec un autre client (Postman, curl), envoyer l'en-tête :

```
Authorization: Bearer <token>
```

Sans token valide, les routes protégées répondent **401 Unauthorized**.

## Routes de l'API

Les URL sont construites avec des **noms** et suivent la même logique pour chaque contrôleur (`BidList`, `CurvePoint`, `Rating`, `RuleName`, `Trade`, `User`).

| Méthode | Route | Action |
|---|---|---|
| GET | `/{Controller}/List` | Liste de tous les éléments |
| GET | `/{Controller}/Details/{id}` | Détail d'un élément |
| GET | `/{Controller}/FormUpdate/{id}` | Données pour préremplir un formulaire de modification |
| POST | `/{Controller}/Creation` | Création |
| PUT | `/{Controller}/Modification/{id}` | Modification |
| DELETE | `/{Controller}/Removal/{id}` | Suppression |
| POST | `/Login/Token` | Connexion, renvoie un token JWT |

Exemple : `GET /BidList/List`.

Les réponses ne contiennent que l'essentiel : par exemple, un utilisateur est renvoyé sans son mot de passe.

## Sécurité

- **Authentification JWT** : le token porte le nom d'utilisateur, le rôle et l'identifiant de l'utilisateur.
- **Autorisation** : `[Authorize]` sur les contrôleurs métier.
- **Hashage des mots de passe** avec `PasswordHasher<User>` (`Microsoft.AspNetCore.Identity`).
- **Règles de mot de passe** : 8 caractères minimum, une majuscule, un chiffre et un symbole.
- **Unicité** du nom d'utilisateur à la création et à la modification.
- Le mot de passe n'est jamais renvoyé par l'API ni écrit dans le token.

## Validation des données

- Champs texte obligatoires via `[Required]`
- Champs numériques bornés via `[Range]` (par exemple `CurveId` et `OrderNumber`, de 0 à 255)
- Règles de mot de passe via l'attribut personnalisé `PasswordValid`
- Les messages d'erreur sont stockés dans des fichiers de ressources `.fr.resx` et renvoyés dans la réponse de l'API.

## Logs

Un middleware (`LogEndpointMiddleware`) enregistre chaque appel d'endpoint :

```
URL: /BidList/List
METHOD: GET
USER: mon.utilisateur
STATUS: Success
CODE: 200
```

Les appels sans authentification apparaissent avec l'utilisateur `Anonyme`.

## Tests

Le projet `Test-P7CreateRestApi` utilise **xUnit** :

- **Tests de services** : un fichier par table (BidList, CurvePoint, Rating, RuleName, Trade, User). Ils s'exécutent sur une base **SQLite en mémoire**, avec le vrai `DbContext`, pour vérifier le comportement réel du Repository et du Service (contraintes de la base comprises).
- **Tests de validation** : un fichier par table, pour contrôler les règles des data annotations champ par champ.

Lancer les tests :

```bash
dotnet test
```

ou depuis Visual Studio : **Test → Exécuter tous les tests**.

## Choix techniques

- **Table `User` de l'énoncé conservée** : seul le hashage d'ASP.NET Core Identity (`PasswordHasher<User>`) est utilisé, sans héritage d'`IdentityUser` ni tables `AspNet*`. L'authentification repose sur un nom d'utilisateur et un mot de passe, et le rôle est transporté dans le token (claim `Role`), ce qui permet d'ajouter des restrictions par rôle sans modifier la base.
- **Couche Service ajoutée** au dépôt de départ, pour que les Controllers ne portent aucune logique et que les Repositories restent du CRUD pur.
- **Mise à jour sur l'entité existante** : le Service récupère l'entité avant de la modifier, ce qui préserve les champs de création (`CreationName`, `CreationDate`) lors d'une modification.
- **SQLite en mémoire pour les tests** : plus fiable qu'un mock de repository, car les requêtes et les contraintes de colonnes sont réellement exécutées.
