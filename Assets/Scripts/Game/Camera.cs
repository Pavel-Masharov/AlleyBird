using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [HideInInspector]
    public static Player player;

    void LateUpdate()
    {
        if(player)
        transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
    }
}
