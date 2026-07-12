using UnityEngine;

/// <summary>
/// Gère la création et la gestion des tuiles dans la scène. 
/// Ce script instancie un ensemble de tuiles à partir d'un prefab, les organise en une grille et gère les interactions avec ces tuiles, telles que le survol et la sélection par le joueur. 
/// Il permet également de mettre à jour les matériaux des tuiles en fonction de leur état (par défaut, survolé ou ciblé par le joueur).
/// </summary>
/// <remarks>
/// Le code présent sert de placeholder pour tester la logique de gestion des tuiles et peut être remplacé par une version plus avancée à l'avenir.
/// </remarks>
public class TileManager : MonoBehaviour
{
    [Header("Affectation inspecteur"), Space(30)]
    [Header("Projet")]
    public Material matTileDefault;
    public Material matTileHovered, matTilePlayerTarget;
    public GameObject prefabTile;
    [Header("Ajustement inspecteur")]
    [Range(1, 100)]
    public int nbTilesPerAxis = 10;

    GameObject[,] allTiles;
    float tileScale = 1;
    Tile tileCurrentHover, tileCurrentPlayerTarget;
    int[] playerTargetPos = new int[2];

    private void Awake()
    {
        tileScale = prefabTile.transform.lossyScale.x;
        allTiles = new GameObject[nbTilesPerAxis, nbTilesPerAxis];

        //allTiles = GameObject.FindGameObjectsWithTag("Tile");
        for (int i = 0; i < nbTilesPerAxis; i++)
        {
            GameObject[] tempArray = new GameObject[nbTilesPerAxis];
            for (int j = 0; j < nbTilesPerAxis; j++)
            {
                GameObject tileInstance = Instantiate(prefabTile);
                tempArray[j] = tileInstance;
                tileInstance.name += $"_{i}-{j}";
                tileInstance.transform.parent = transform;
                tileInstance.transform.position = new Vector3(0 + tileScale * i, 0, 0 + tileScale * j);
                allTiles[i, j] = tileInstance;
            }
        }
    }



    /// <summary>
    /// Sélectionne une case qui est survolée par la souris et met à jour l'affichage des cases.
    /// </summary>
    /// <remarks>Si la case sélectionnée est déjà survolée, rien ne se passe.</remarks>
    /// <param name="tile">La case <see cref="Tile"/> sélectionnée comme étant survolée. Peut être <see langword="null"/> pour démontrer aucun survol.</param>
    public void SetHoveredTile(Tile tile)
    {
        if (tileCurrentHover != tile)
        {
            tileCurrentHover = tile;
            UpdateTilesMaterial();
        }
    }
    /// <summary>
    /// Sélectionne une case qui devient la cible du joueur et met à jour l'affichage des cases.
    /// </summary>
    /// <remarks>Si la case sélectionnée est déjà ciblée, rien ne se passe.</remarks>
    /// <param name="tile">La case <see cref="Tile"/> sélectionnée comme étant ciblée. Peut être <see langword="null"/> pour démontrer aucune cible.</param>
    public void SetTargetedTile(Tile tile)
    {
        if (tileCurrentPlayerTarget != tile)
        {
            tileCurrentPlayerTarget = tile;
            UpdateTilesMaterial();
        }
    }
    /// <summary>
    /// Met à jour les matériaux de toutes les cases en fonction de leur état actuel (par défaut, survolé ou ciblé par le joueur).
    /// </summary>
    public void UpdateTilesMaterial()
    {
        for (int i = 0; i < allTiles.GetLength(0); i++)
        {
            for (int j = 0; j < allTiles.GetLength(1); j++)
            {
                if (tileCurrentPlayerTarget != null && allTiles[i, j] == tileCurrentPlayerTarget.gameObject)
                {
                    //Debug.Log("new targeted tile");
                    allTiles[i, j].GetComponent<Tile>().meshRenderer.material = matTilePlayerTarget;
                    playerTargetPos = new int[2] { i, j };
                    //Debug.Log($"[{playerTargetPos[0]}, {playerTargetPos[1]}]");
                    //Debug.Log(allTiles[i, j].name, allTiles[i, j]);
                }
                else if (tileCurrentHover != null && allTiles[i, j] == tileCurrentHover.gameObject)
                {
                    //Debug.Log("new hovered tile");
                    allTiles[i, j].GetComponent<Tile>().meshRenderer.material = matTileHovered;
                }
                else
                {
                    allTiles[i, j].GetComponent<Tile>().meshRenderer.material = matTileDefault;
                }
            }
        }
    }
}
