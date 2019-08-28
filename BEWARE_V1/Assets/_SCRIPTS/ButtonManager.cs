using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{


    public void OpenScene()
    {

        SceneManager.LoadScene("ARFOUNDATION");
    }

    public void Call()
    {

        Application.OpenURL("tel://0662411448");
    }


 
}
