using UnityEngine;

public class Tile : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
}
