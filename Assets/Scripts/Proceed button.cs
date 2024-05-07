using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proceedbutton : MonoBehaviour
{
    public GameObject listPanel;
    public GameObject ThirdPagePanel;

    public void OnLoginButtonClick()
    {
        Debug.Log("Proceed button was pressed");
        listPanel.SetActive(false);

        ThirdPagePanel.SetActive(true);
    }
}
