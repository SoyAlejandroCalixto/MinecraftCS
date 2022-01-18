using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSteve : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition)))
         {
            if(Input.GetMouseButton(0))
             {
                if(Input.GetAxis("Mouse X") != 0)
                {
                    transform.Rotate(0, -Input.GetAxis("Mouse X")*20, 0);
                }
             }            
         }
    }
}
