# Summer Game

Projet d'été

## Stack technologique

- Unity 6.5 (6000.5.0f1)
  - modules:
    - Microslop Visual Studio Community (22022 ou 2026)
    - Documentation (optionnel)
- Git 2.54.0 (2.54.0.windows.1)+
- Git LFS 3.7.1+
- GitHub

## Méthodologie

### Git

- JAMAIS TRAVAILLER SUR LA BRANCHE 'main' ou 'dev' sauf pour @Firefox13590.

### Structure des fichiers

- Mettre chaque fichiers et éléments Unity dans le bon dossier.
- Créer les dossiers manquants pour les ressources non catégorisées si nécessaire.

### Structure du code

- Structure standard pour un script de type `MonoBehaviour`:

```c#
  /*
    Variables publiques
  */

  // Une catégorie majeure de variables publiques qui seront associées a des ressources ou composantes complexes affectées via l'inspecteur.
  // Ajouter un attribut Header pour les catégories de variables publiques afin de les identifier et les distinguer.
  // Certaines distinctions sont obligatoires à intégrer unique si une variable coordonne avec la catégorie mineure.
  [Header("Affectation inspecteur"), Space(30)]
  // Catégorie mineure obligatoire pour les affectations venant de la scène (Hiérarchie).
  [Header("Hiérarchie")]
  public GameObject calibOverlay;
  public GameObject modelePapier, jumpscareImage;
  // Catégorie mineure obligatoire pour les affectations venant d'un fichier local (Projet).
  [Header("Projet")]
  public Material matPapier;
  public AudioClip sonJumpscare;
  // Catégorie mineure obligatoire pour les affectations de valeur au lieu de données ou ressources complexes.
  [Header("Ajustement inspecteur")]
  public bool modeProgBarre = false;
  [Range(0, 1)] public float progressionBarre = .001f;

  // Une catégorie majeure de variables publiques qui seront utilisées par des scripts externes.
  // Une variable publique peut nécessiter l'affection de ressources complexes. Dans ce cas, la repositionner dans la catégorie majeure précédente.
  [Header("Accès pour autres scripts"), Space(30)]
  public StageJeu stageJeu = 0;
  // Aucune catégorie mineure obligatoire, mais il est recommandé de tout de même établir des distinctions.
  [Header("Modes")]
  public bool modeObtentionItem;
  [Header("États")]
  public bool gameOver;
  public bool allowGameLoop = true;
  [Header("Progression des niveaux")]
  public bool objectifComplete;
  public bool niveauComplete;
  public int indexLampCour, progressionBoutsPapier;
  public GameObject[] listeLampadaires = new GameObject[5],
      listeBoutsPapier = new GameObject[5];
  [Header("Autres")]
  public GameObject player, recompense, cameraJoueur;
  public ControlesPersonnage playerScript;
  public LanguageManager.Language langue = LanguageManager.Language.French;

  // Évènements
  public static event Action GameOverEvent, LevelCompleteEvent;
  public static event Action<float> LevelProgressEvent;
  public static event Action<StageJeu> ObjectiveCompleteEvent;


  /*
    Variables privées
  */

  // Variables de travail
  // Variables qui seront utilisées exclusivement dans le script où elles sont déclarées.
  // Généralement privées, mais elles peuvent être publiques aussi. Dans ce cas, les repositonner dans la bonne catégorie majeure du côté des variables publiques.
  RectTransform rectBarre;
  float maxWidth;

  // Constantes
  // Variables dont la valeur ne change pas (valeur par défaut ou de départ).
  // Si une variable constante ne peut pas être affectée et déclarée en même temps (ex: `const Vector2 DEFAULT_POS = new Vector2(0, -50);`), il est recommandé de la déclarer en tant que `readonly` plutôt que `const`.
  readonly Vector2 DEFAULT_POS = new Vector2(0, -50);
  readonly Vector3 DEFAULT_ROT = Vector3.zero;
  const float DEFAULT_SCALE = 1f;
```

- Les méthodes personnalisées devraient se retrouver après les méthodes propres à la Classe `MonoBehaviour`.

#### Nomenclature:

- Le nom des classes doivent être en PascalCase.
- Le nom des champs doivent être en camelCase et peuvent ou non débuter par un `_`. Ex: `_health` ou `health`.
- Le nom des propriétés doivent être en PascalCase.
  - Il n'est pas obligatoire de préciser le modificateur d'accès si celui-ci est privé (manque de mention assume privé).
  - Le nom des constantes doivent être en SCREAMING_SNAKE_CASE.
  - Le nom des évènements doivent être en PascalCase et doivent toujours finir par `Event`. Ex: `EyeContactEvent`.
- Le nom des méthodes (fonctions) doivent être en PascalCase.

## Implémentation

### Ajouter implémentation
