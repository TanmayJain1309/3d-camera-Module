using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginScript : MonoBehaviour
{
    
    public GameObject loginPanel;
    public GameObject nextPagePanel;

    public void OnLoginButtonClick()
    {
        Debug.Log("Login button was pressed");
        loginPanel.SetActive(false);

        nextPagePanel.SetActive(true);
    }
}

