using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public Transform player;
    public float distanceFromPlayer;
    public float maxDistanceFromPlayer;
    private Vector3 eye;
    // Start is called before the first frame update
    void Start()
    {
        eye = new Vector3(0, 0, -distanceFromPlayer);
        transform.position = player.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerPos = player.position;
        Vector2 lerpedPosition = Vector2.Lerp(transform.position, playerPos, 0.05f);
        lerpedPosition = new Vector2(Mathf.Clamp(lerpedPosition.x, playerPos.x - maxDistanceFromPlayer, playerPos.x + maxDistanceFromPlayer),
                                     Mathf.Clamp(lerpedPosition.y, playerPos.y - maxDistanceFromPlayer, playerPos.y + maxDistanceFromPlayer));
        /*
        if ((lerpedPosition - playerPos).magnitude > maxDistanceFromPlayer) {
            lerpedPosition = playerPos + lerpedPosition.normalized * maxDistanceFromPlayer;
        }
        */
        transform.position = lerpedPosition;
        transform.position += eye;
    }
}
