using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;


public class ToggleManager : MonoBehaviour
{
    public Dictionary<string, Toggle> toggleDictionary = new Dictionary<string, Toggle>();
    public Button proceedButton;

    // Add your toggles from the inspector
    public Toggle[] toggles;

    void Start()
    {
        proceedButton.gameObject.SetActive(false);


        // Add each toggle to the dictionary using its name as the key
        foreach (Toggle toggle in toggles)
        {
            toggleDictionary.Add(toggle.gameObject.name, toggle);
        }

        // Example: Call a method when any toggle value changes
        foreach (Toggle toggle in toggleDictionary.Values)
        {
            toggle.onValueChanged.AddListener((bool value) => OnToggleValueChanged(toggle.gameObject.name, value));
        }
    }

    // Example method to handle toggle value changes
    void OnToggleValueChanged(string toggleName, bool newValue)
    {
        Debug.Log("Toggle " + toggleName + " value changed to " + newValue);
        // Add your logic here based on the toggle value
        proceedButton.gameObject.SetActive(newValue);

    }
    
}
