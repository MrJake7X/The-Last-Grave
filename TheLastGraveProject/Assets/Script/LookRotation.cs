using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookRotation : MonoBehaviour
{
    private Vector2 axisRotation;

    private Transform cameraTransform;
    private Quaternion cameraRot;
    private Transform playerTransform;
    private Quaternion playerRot;



    public bool smooth;
    public float smoothSpeed;

    public bool limitCameraRot;
    public float minAngle = -60;
    public float maxAngle = 60;

    // Use this for initialization
    void Start ()
    {
        //Definimos el transform y la rotacion de la camara
        cameraTransform = Camera.main.transform;
        cameraRot = cameraTransform.localRotation;

        //Definimos el transform y la rotacion del player
        playerTransform = transform;
        playerRot = playerTransform.localRotation;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Añadimos el axis a la rotacion
        cameraRot *= Quaternion.Euler(-axisRotation.y, 0, 0);
        playerRot *= Quaternion.Euler(0, axisRotation.x, 0);

        if(limitCameraRot) cameraRot = ClampRotationAroundXAxis(cameraRot);

        if(smooth)
        {
            cameraTransform.localRotation = Quaternion.Slerp(cameraTransform.localRotation, cameraRot, Time.deltaTime * smoothSpeed);
            playerTransform.localRotation = Quaternion.Slerp(playerTransform.localRotation, playerRot, Time.deltaTime * smoothSpeed);
        }
        else
        {
            cameraTransform.localRotation = cameraRot;
            playerTransform.localRotation = playerRot;
        }
    }

    public void SetMouseAxis(Vector2 mouseAxis)
    {
        axisRotation = mouseAxis;
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, minAngle, maxAngle);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}
