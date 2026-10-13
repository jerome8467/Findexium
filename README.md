# P7CreateRestApi

API REST en **C# / .NET 6** pour gérer six entités métier (BidList, CurvePoint, Rating, RuleName, Trade, User), sécurisée par **JWT** avec deux rôles : `Admin` et `User`.

Projet 7 de la formation *Développeur back-end .NET* (OpenClassrooms).

## Architecture

```
P7CreateRestApi/
├── Controllers/   Routage HTTP, délègue au Service
├── Service/       Logique métier, validation, mapping
├── Repositories/  Accès aux données (CRUD pur)
├── Domain/        Entités mappées sur les tables
├── Models/        Objets reçus par l'API
├── Data/          LocalDbContext et données de départ (IdentitySeedData)
├── Middleware/    Journalisation des appels
├── Attribute/     Règles de mot de passe
└── Ressource/     Messages d'erreur en français

Test-P7CreateRestApi/  Tests unitaires (xUnit)
```

Flux d'une requête : **Controller → Service → Repository → Base de données**.

## Installation

Prérequis : .NET 6 SDK, SQL Server (Express ou Developer), Visual Studio 2022.

1. Cloner le dépôt.
2. Dans `P7CreateRestApi/appsettings.json`, adapter la chaîne de connexion `DefaultConnection` à votre machine (`Server=.\\SQLEXPRESS` pour SQL Server Express, `Server=.` pour une instance par défaut).
3. Créer la base : dans Visual Studio, **Outils → Gestionnaire de packages NuGet → Console du gestionnaire de packages**, projet par défaut `P7CreateRestApi`, puis :

```powershell
Update-Database
```

4. Lancer le projet (F5). Swagger s'ouvre sur `https://localhost:7210/swagger`.

## Premiers pas

Au premier lancement, si la table `Users` est vide, l'API crée automatiquement un compte administrateur :

| Username | Mot de passe | Rôle |
|---|---|---|
| `Admin` | `Password1!` | Admin |

1. Appeler `POST /Login/Access` avec `{ "username": "Admin", "password": "Password1!" }` : la réponse contient le token JWT.
2. Dans Swagger, cliquer sur **Authorize** et coller le token (sans le mot `Bearer`).

Sans token, les routes protégées répondent **401**. Avec un token d'un rôle insuffisant, elles répondent **403**.

## Routes

Les URL utilisent des noms et suivent la même logique pour `BidList`, `Curve`, `Rating`, `RuleName` et `Trade` (token requis) :

| Méthode | Route | Action |
|---|---|---|
| GET | `/{Controller}/List` | Liste |
| GET | `/{Controller}/Details/{id}` | Détail |
| GET | `/{Controller}/FormUpdate/{id}` | Données pour préremplir une modification |
| POST | `/{Controller}/Creation` | Création |
| PUT | `/{Controller}/Modification/{id}` | Modification |
| DELETE | `/{Controller}/Removal/{id}` | Suppression |

### Utilisateurs

| Méthode | Route | Accès | Action |
|---|---|---|---|
| POST | `/Login/Access` | Public | Connexion, renvoie un token |
| POST | `/User/Creation` | Public | Inscription (rôle `User`) |
| GET | `/User/Profile` | Connecté | Son propre profil |
| GET | `/User/FormUpdate` | Connecté | Prérempli de son profil |
| PUT | `/User/Modification` | Connecté | Modifier son profil |
| PUT | `/User/ChangePassword` | Connecté | Changer son mot de passe |
| GET | `/Admin/User/List` | Admin | Tous les utilisateurs |
| GET | `/Admin/User/DetailsById/{id}` | Admin | Un utilisateur par id |
| GET | `/Admin/User/DetailsByUsername/{username}` | Admin | Un utilisateur par username |
| POST | `/Admin/User/Creation` | Admin | Créer un utilisateur avec le rôle choisi |
| GET | `/Admin/User/FormUpdate/{id}` | Admin | Prérempli de modification |
| PUT | `/Admin/User/Modification/{id}` | Admin | Modifier un utilisateur, rôle compris |
| DELETE | `/Admin/User/Removal/{id}` | Admin | Supprimer un utilisateur |

## Sécurité

- **JWT** : le token porte le nom, le rôle et l'identifiant de l'utilisateur.
- **Rôles** : `Admin` et `User` (enum `UserRole`), stockés en texte en base. Un utilisateur ne peut pas changer son propre rôle.
- **Mots de passe hashés** avec `PasswordHasher<User>` : 8 caractères minimum, une majuscule, un chiffre, un symbole.
- **Validation** des champs (`[Required]`, `[Range]`) avec messages d'erreur en français.
- **Logs** : chaque appel est journalisé (URL, méthode, utilisateur, code HTTP).

## Tests

Projet `Test-P7CreateRestApi` (xUnit) :

- **Services** : exécutés sur une base SQLite en mémoire, avec le vrai `DbContext`.
- **Validation** : règles des data annotations champ par champ.

```bash
dotnet test
```

## Choix techniques

- La table `User` de l'énoncé est conservée : seul le hashage d'Identity est utilisé, sans `IdentityUser`.
- Une couche Service est ajoutée : les Controllers ne portent aucune logique, les Repositories restent du CRUD pur.
- Le rôle est un enum côté Models et DTO, converti en texte pour la table.
