using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARItemDestroy : MonoBehaviour
{

    public Camera mainCamera;
    


    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) // lorsque je touche l'écran

        {
            RaycastHit hitRay; // définition du "hit" du raycast

            Ray ray = mainCamera.ScreenPointToRay(Input.GetTouch(0).position); // Envoid u raycast par rapport a la position du touch


            if (Physics.Raycast(ray, out hitRay))

            {

                if (hitRay.transform.gameObject.tag == "Item") // si le rayon topuch un objet item

                {

                    Destroy(hitRay.transform.gameObject); // ce gameobject est détruit

                }

            }

        }
    }

}
