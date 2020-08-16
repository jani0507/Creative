using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotor : MonoBehaviour
{
    public Transform lookAt; //Der Spieler oder Object, das wir beobachten
    public Vector3 offset = new Vector3(0, 92, -8.0f);

    private void Start()
    {
        transform.position = lookAt.position + offset;
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = lookAt.position + offset;
        desiredPosition.x = 0;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime);
    }
}
