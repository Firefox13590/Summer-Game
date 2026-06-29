using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FollowPlayer : MonoBehaviour
{
    public Vector3 adjustPos = Vector3.one,
        adjustRot = Vector3.zero;

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
