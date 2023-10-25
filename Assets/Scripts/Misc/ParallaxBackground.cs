using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private float lengthX, lengthY, startPosX, startPosY;
    GameObject cam;
    [SerializeField] private float parallaxEffectAmount;

    private void Start() {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        lengthX = GetComponent<RectTransform>().rect.width;
        lengthY = GetComponent<RectTransform>().rect.height;
        cam = Camera.main.gameObject;
    }

    private void FixedUpdate() {
        Vector3 pos = cam.transform.position;

        float tempX = pos.x * (1 - parallaxEffectAmount);
        float tempY = pos.y * (1 - parallaxEffectAmount);

        if (tempX > startPosX + (lengthX / 2))
            startPosX += lengthX;
        else if (tempX < startPosX - (lengthX / 2))
            startPosX -= lengthX;
            
        if (tempY > startPosY + (lengthY / 2))
            startPosY += lengthY;
        else if (tempY < startPosY - (lengthY / 2))
            startPosY -= lengthY;

        float distanceX = (cam.transform.position.x * parallaxEffectAmount);
        float distanceY = (cam.transform.position.y * parallaxEffectAmount);

        transform.position = new Vector3(startPosX + distanceX,
                                        startPosY + distanceY,
                                        transform.position.z);
    }
}
