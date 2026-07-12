using UnityEngine;

/// <summary>
/// Permet à la caméra de suivre le joueur en ajustant sa position et sa rotation si nécessaire.
/// </summary>
/// <remarks>
/// Le code présent est un placeholder simple et sera remplacé par un système de caméra plus avancé dans le futur.
/// </remarks>
[RequireComponent(typeof(Camera))]
public class FollowPlayer : MonoBehaviour
{
    [Header("Affectation inspecteur"), Space(30)]
    [Header("Ajustement inspecteur")]
    public Vector3 adjustPos = Vector3.one;
    public Vector3 adjustRot = Vector3.zero;

    GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.SetPositionAndRotation(player.transform.position + adjustPos, Quaternion.Euler(adjustRot));
    }
}
