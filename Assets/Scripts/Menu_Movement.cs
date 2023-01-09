using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Movement : MonoBehaviour
{


    float mouseX;
    float mouseY;

    [SerializeField] float movementQuality;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.mousePosition.x;
        mouseY = Input.mousePosition.y;

        this.GetComponent<RectTransform>().position = new Vector2(
            (mouseX / Screen.width) * movementQuality + (Screen.width / 2),
            (mouseY / Screen.height) * movementQuality + (Screen.height / 2));

    }
}
