using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PanelManager : MonoBehaviour
{
    public Toggle[] toggles; 
    public string[] sceneNames;

    private void Start()
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            int index = i;
            toggles[i].onValueChanged.AddListener((bool value) => ToggleChanged(index, value));
        }
    }

    private void ToggleChanged(int index, bool value)
    {
        if (value)
        {
            SceneManager.LoadScene(sceneNames[index]);
        }
    }
}
