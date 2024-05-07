using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Button Right;
    public Button Left;
    public Button Up;
    public Button Down;
    //public Button Start;
    public Button Stop;
    public Image Control;

    public void SetUI(bool active)
    {
        Debug.Log("Orbital Ui is active");

        Control.gameObject.SetActive(active);
        Right.gameObject.SetActive(active);
        Left.gameObject.SetActive(active);
        Up.gameObject.SetActive(active);
        Down.gameObject.SetActive(active);
        //Start.gameObject.SetActive(active);
        Stop.gameObject.SetActive(active);
    }
}
