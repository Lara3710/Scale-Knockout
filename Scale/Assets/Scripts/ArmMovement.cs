using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmMovement : MonoBehaviour
{
    
    private Vector3 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        float rotaitonZ = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        transform.localRotation = Quaternion.Euler(0, 0, rotaitonZ);
    }
}
