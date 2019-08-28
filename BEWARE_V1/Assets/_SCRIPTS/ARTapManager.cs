using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;


[RequireComponent(typeof(ARRaycastManager))]
public class ARTapManager  : MonoBehaviour
{
    #region VARIABLES


    [Header("PARAMETRES PLACEMENT OBJETS")]

    public GameObject placementIndicator; // tracker RA au sol

    public Image viewerIcon; //GameObject avec Image de démarrage lors du tracking au sol

    public GameObject lastInstantiate; // Dernier GO instantié pour calculer distance entre le tracker et le dernier GO dans l'espace

    public float distanceGameobject; // Distance entre le tracker et le dernier GO

    public Text infoButton; // Text indicateur de l'action suivante à mener

    public Animator animationPanel; //Animation de l'arrivée de l'UI avec les différents boutons sur le côté

    public InputField inputField; // champ text pour écrire un commentaire sur les points d'informations

   
    private Pose placementPose; // Point d'impact d'ARKIT dans le world

    private GameObject indicatorAwake; // permet d'indiquer le premier Objet posé sur le sol pour commencer à calculer les distances

    private ARRaycastManager _arRaycastManager; // Appel au script ARRaycastManager pour récupérer le raycast propre a ARFoundation

    private bool placementPoseIsValide; // Boolean pour savoir si le sol est détecté et si il y a des "hit"

    private Vector3 _ScreenCenter; // définition du centre de l'écran pour création du GO instantié

    private List<ARRaycastHit> _Hits = new List<ARRaycastHit>(); // Liste pour stocker les hits dans le world

    private List<GameObject> itemList = new List<GameObject>(); // Liste pour stocker les GO instantié dans le world


    #endregion VARIABLES


    #region CLASSIC METHOD



    private void Awake()
    {
        indicatorAwake = GameObject.FindGameObjectWithTag("indicator"); // trouve lobjet tagger indicator

        lastInstantiate = indicatorAwake; // l'objet tagger indicator devient l'objet lastInstantiate (pour calculer la distance avec le premier élément da la liste)

        inputField.text = "Point Information !"; // text par défaut de l'objet champ text
    }


    void Start()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>(); // création d'une variable pour acceder aux infos du Raycast propre a ARFoundation

        distanceGameobject = 1000; // définition de distance objet entre tracker et GO instantié par defaut

        infoButton.text = "Scannez le sol ! "; // text par défaut au lancement de l'application

        inputField.gameObject.SetActive(false); // masquage d u champ text lorsque l'on ne fait pas appel a lui


    }

    void Update()
    {
      
        UpdatePlacementPose(); // mise a jour des possibilités de placement dans la scene

        UpdatePlacementIndicator(); // mise a jour du GameObject indicator dans la scene

        UpdateDistanceIndicator(); // mise a jour de la distance entre l'objet posé et le futur objet posé

        UpdateInputFieldTextConnection(); // mise a jour du texte du champ texte dans l'info bull posé dans la scène

        Debug.Log("Update");

    }



    #endregion CLASSIC METHOD

    #region SPECIFIC METHOD

    private void UpdateInputFieldTextConnection()
    {
        Debug.Log("4_SwapText");

       placementIndicator.GetComponentInChildren<Text>().text = inputField.text;
    }

    // methode d'instantiation pour placer l'objet par rapport au "POSE" fournit par ARKIT (methode appelée via BUTTON)
    public void PlaceObject()
    {
        var _cameraForward = Camera.current.transform.forward;
       
        var _cameraBearing = new Vector3(_cameraForward.x, 0, _cameraForward.z).normalized;

        GameObject _instantiateIndicator = Instantiate(placementIndicator, placementPose.position, placementPose.rotation) as GameObject;

        itemList.Add(_instantiateIndicator);

        lastInstantiate = itemList[itemList.LastIndexOf(_instantiateIndicator)];

        inputField.text = (Application.persistentDataPath);

    }

    // methode pour upadte le placement de l'indicateur de pose des éléments 3D en réalité augmentée
    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValide)
        {
         
            placementIndicator.SetActive(true);

            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);

            infoButton.text = "Placez votre première balise!";

            animationPanel.Play("Swap");

            viewerIcon.gameObject.SetActive(false);

        }
        else
        {
            Debug.Log("2 UpdatePlacementIndicator");

            placementIndicator.SetActive(false);
           
            infoButton.text = "Scannez le sol !" ;
           
           viewerIcon.gameObject.SetActive(true);

        }
    }

    // methode pour update le placement de l'indicateur par rapport au placement de la camera
    private void UpdatePlacementPose()
    {

        Debug.Log("1 UpdatePlacementPose");
      
        _ScreenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0));
     
  
        _arRaycastManager.Raycast(_ScreenCenter, _Hits, TrackableType.Planes);
       

        placementPoseIsValide = _Hits.Count > 0;

        if (placementPoseIsValide)
        {

            placementPose = _Hits[0].pose;

            var _cameraForward = Camera.current.transform.forward;
           
            var _cameraBearing = new Vector3(_cameraForward.x, 0, _cameraForward.z).normalized;

            placementPose.rotation = Quaternion.LookRotation(_cameraBearing);

        }

       
    }

    // methode pour update la distance entre l'indicator et le dernier prefab créer dans le monde (permet d'empecher de poser 2 fleches les unes à côté des autres)
    private void UpdateDistanceIndicator()
    {
        Debug.Log("3 UpdateDistanceIndicator");

        distanceGameobject = Vector3.Distance(placementIndicator.transform.position, lastInstantiate.transform.position);


        if(lastInstantiate != indicatorAwake) 
        
        { 

        if (distanceGameobject < 0.5f)
        {
            placementIndicator.GetComponentInChildren<MeshRenderer>().enabled = false;
            
            infoButton.text = "Avancez pour placer votre prochaine balise! ";
           
        }
        else
        {
            placementIndicator.GetComponentInChildren<MeshRenderer>().enabled = true;
           
            infoButton.text = "Vous pouvez placer votre balise! ";

        }

    }

}


    #endregion SPECIFIC METHOD
}
