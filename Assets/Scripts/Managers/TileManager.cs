using UnityEngine;
using static UnityEditor.Progress;

public class TileManager : MonoBehaviour
{
    public Material matTileDefault, matTileHovered, matTilePlayerTarget;
    public GameObject prefabTile;
    [Range(1, 100)]
    public int nbInterationsPerAxis = 10;

    GameObject[,] allTiles;
    float tileScale = 1;
    Tile tileCurrentHover, tileCurrentPlayerTarget;
    int[] playerTargetPos = new int[2] { 0, 0 };

    private void Awake()
    {
        tileScale = prefabTile.transform.lossyScale.x;
        allTiles = new GameObject[nbInterationsPerAxis, nbInterationsPerAxis];

        //allTiles = GameObject.FindGameObjectsWithTag("Tile");
        for (int i = 0; i < nbInterationsPerAxis; i++)
        {
            GameObject[] tempArray = new GameObject[nbInterationsPerAxis];
            for (int j = 0; j < nbInterationsPerAxis; j++)
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



    public void SetHoveredTile(Tile tile)
    {
        if (tileCurrentHover != tile)
        {
            tileCurrentHover = tile;
            UpdateTilesMaterial();
        }
    }
    public void SetTargetedTile(Tile tile)
    {
        if (tileCurrentPlayerTarget != tile)
        {
            tileCurrentPlayerTarget = tile;
            UpdateTilesMaterial();
        }
    }
    public void UpdateTilesMaterial()
    {
        //Debug.Log("update mat of tiles");
        foreach (var item in allTiles)
        {
            if (tileCurrentPlayerTarget != null && item == tileCurrentPlayerTarget.gameObject)
            {
                //Debug.Log("new targeted tile");
                item.GetComponent<Tile>().meshRenderer.material = matTilePlayerTarget;
            }
            else if (tileCurrentHover != null && item == tileCurrentHover.gameObject)
            {
                //Debug.Log("new hovered tile");
                item.GetComponent<Tile>().meshRenderer.material = matTileHovered;
            }
            else
            {
                item.GetComponent<Tile>().meshRenderer.material = matTileDefault;
            }
        }

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
