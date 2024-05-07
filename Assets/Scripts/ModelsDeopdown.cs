using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.Networking;
using System.Text;

public class ModelsDeopdown : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public TMP_Text propertyText;
    private Dictionary<string, Dictionary<string, string>> modelProperties;
    public TextAsset ModelJson;
    public GameObject textFieldPrefab;
    public Transform textFieldParent;
    private string selectedModel;
    public TMP_InputField PName;
    public TMP_InputField PValue;
    public TMP_InputField AddPName;
    public TMP_InputField AddPValue;
    public Transform OpenPanel;
    public Button Options;

    private Coroutine moveCameraCoroutine;

    public Button AddButton;
    public Button DeleteButton;
    private TMP_Text currentTextField;

    public float distanceFromObject = 10f ;

    private Dictionary<string, Vector3> modelPositions = new Dictionary<string, Vector3>();


    void Start()
    {
        PopulateDropdown();
/*        LoadModelProperties();
*/        dropdown.gameObject.SetActive(false);
        PName.gameObject.SetActive(false);
        PValue.gameObject.SetActive(false);
        AddPName.gameObject.SetActive(false);
        AddPValue.gameObject.SetActive(false);
        DeleteButton.gameObject.SetActive(false);
        AddButton.gameObject.SetActive(false);
        // textFieldParent.gameObject.SetActive(false);
        OpenPanel.gameObject.SetActive(false);

        modelProperties = new();

/*        StartCoroutine(FetchModelProperties());
*/    }


    /*IEnumerator FetchModelProperties()
    {
    using (UnityWebRequest www = UnityWebRequest.Get("http://127.0.0.1:5000/get_model_properties"))
    {
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string jsonString = www.downloadHandler.text;
            modelProperties = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(jsonString);
        }
        else
        {
            Debug.LogError("Failed to fetch model properties: " + www.error);
        }
    }
    }*/

    

    /*void LoadModelProperties()
        {
            ModelJson = Resources.Load<TextAsset>("ModelProp");
            string jsonString = ModelJson.text;
            modelProperties = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(jsonString);
        }*/

    void PopulateDropdown()
    {
        dropdown.ClearOptions();

        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        List<string> modelNamesExceptPlane = new List<string>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.GetComponent<MeshFilter>() != null && obj.name != "Plane")
            {
                modelNamesExceptPlane.Add(obj.name);
                modelPositions[obj.name] = obj.transform.position; 

                Debug.Log(modelPositions[obj.name]);
            }
        }

        dropdown.AddOptions(modelNamesExceptPlane);
    }



public void OnDropdownValueChanged(int index)
{
    selectedModel = dropdown.options[index].text;
    DeleteButton.gameObject.SetActive(true);
    AddButton.gameObject.SetActive(true);
    //textFieldParent.gameObject.SetActive(true);

    StartCoroutine(FetchModelProperties(selectedModel)); // Fetch properties from the server
}

IEnumerator FetchModelProperties(string modelName)
{
    string url = $"http://127.0.0.1:5000/get_model_properties?model=\"{modelName}\"";
    using (UnityWebRequest www = UnityWebRequest.Get(url))
    {
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string jsonString = www.downloadHandler.text;
            Dictionary<string, string> properties = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);

            if (properties.Count > 0)

            {
                modelProperties.TryAdd(modelName, properties);

                ClearTextFields();

                foreach (var property in properties)
                {
                    CreateTextField(property.Key, property.Value);
                }
                
                if (modelPositions.ContainsKey(selectedModel))
                {
                    Vector3 modelPosition = modelPositions[selectedModel];

                    if (moveCameraCoroutine != null)
                        StopCoroutine(moveCameraCoroutine);

                    moveCameraCoroutine = StartCoroutine(MoveCameraTowardsModel(modelPosition));

                    Debug.Log(modelPosition);
                }
                else
                {
                    Debug.LogWarning("Position not found for the selected model.");
                }
            }
            else
            {
                propertyText.text = "No properties found for the selected model.";
            }
        }
        else
        {
            Debug.LogError("Failed to fetch model properties: " + www.error);
        }
    }
}

    IEnumerator UpdateModelProperties(string jsonString)
    {
        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonString);
        using (UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:5000/update_model_property", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(jsonBytes);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Model properties updated successfully.");
            }
            else
            {
                Debug.LogError("Failed to update model properties: " + www.error);
            }
        }
    }


    IEnumerator MoveCameraTowardsModel(Vector3 targetPosition)
    {
        float journeyLength = Vector3.Distance(Camera.main.transform.position, targetPosition);
        float startTime = Time.time;

        while (Time.time < startTime + 1f)
        {
            float distanceCovered = (Time.time - startTime) * distanceFromObject;
            float fractionOfJourney = distanceCovered / journeyLength;

            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPosition - (Camera.main.transform.forward * distanceFromObject), fractionOfJourney);
            yield return null;
        }

        Camera.main.transform.position = targetPosition - (Camera.main.transform.forward * distanceFromObject); 
        Camera.main.transform.LookAt(targetPosition); // Look at the model
        Debug.Log($"Camera Position: {Camera.main.transform.position}, LookAt: {Camera.main.transform.forward}");
    }

    public void StopCameraMovement()
    {
        if (moveCameraCoroutine != null)
        {
            StopCoroutine(moveCameraCoroutine);
            moveCameraCoroutine = null;
        }
    }


    void ClearTextFields()
    {
        foreach (Transform child in textFieldParent)
        {
            Destroy(child.gameObject);
        }
    }

    void CreateTextField(string propertyName, string propertyValue)
    {
        GameObject textFieldGO = Instantiate(textFieldPrefab, textFieldParent);
        TMP_Text textField = textFieldGO.GetComponent<TMP_Text>();

        textField.text = $"{propertyName}: {propertyValue}";

        Button button = textFieldGO.GetComponent<Button>();
        button.onClick.AddListener(() => ShowInputField(textField, propertyName, propertyValue));
    }

    public void ShowInputField(TMP_Text textField, string propertyName, string propertyValue)
    {
        currentTextField = textField;
        PName.gameObject.SetActive(true);
        PValue.gameObject.SetActive(true);

        PName.text = propertyName;
        PValue.text = propertyValue;
    }

    public void UpdateProperty()
    {
        if (currentTextField != null)
        {
            string newName = PName.text;
            string newValue = PValue.text;

            string[] splitText = currentTextField.text.Split(':');
            if (splitText.Length == 2)
            {
                string oldName = splitText[0].Trim();
                string oldValue = splitText[1].Trim();

                currentTextField.text = $"{newName}: {newValue}";

                

                if (modelProperties.ContainsKey(selectedModel) && modelProperties[selectedModel].ContainsKey(oldName))
                {
                    modelProperties[selectedModel].Remove(oldName);

                    modelProperties[selectedModel][newName] = newValue;
                }

                UpdateJsonFile();

            }

            PName.text = "";
            PValue.text = "";


        }
    }

    public void EditButton()
    {

        string newName = AddPName.text;
        string newValue = AddPValue.text;

        if (!string.IsNullOrEmpty(newName) && !string.IsNullOrEmpty(newValue))
        {
            if (modelProperties.ContainsKey(selectedModel))
            {
                modelProperties[selectedModel][newName] = newValue;
            }
            else
            {
                modelProperties[selectedModel] = new Dictionary<string, string>();
                modelProperties[selectedModel][newName] = newValue;
            }

            UpdateJsonFile();

            CreateTextField(newName, newValue);
        }
        else
        {
            Debug.LogWarning("Property name or value is empty.");
        }

        AddPName.text = "";
        AddPValue.text = "";
    }

    public void DeleteTextField()
    {

        if (currentTextField != null)
        {
            string[] splitText = currentTextField.text.Split(':');
            if (splitText.Length == 2)
            {
                string propertyName = splitText[0].Trim();

                if (modelProperties.ContainsKey(selectedModel) && modelProperties[selectedModel].ContainsKey(propertyName))
                {
                    modelProperties[selectedModel].Remove(propertyName);
                    UpdateJsonFile();

                    Destroy(currentTextField.gameObject);
                    currentTextField = null;
                }
            }
        }
        PValue.gameObject.SetActive(false);
        PName.gameObject.SetActive(false);   
    }
    public void AddNewProperty()
    {
        AddPName.gameObject.SetActive(!AddPName.gameObject.activeSelf);
        AddPValue.gameObject.SetActive(!AddPValue.gameObject.activeSelf);


    }


    public void Deselect()
    {
        PName.gameObject.SetActive(false);
        PValue.gameObject.SetActive(false);
        AddPName.gameObject.SetActive(false);
        AddPValue.gameObject.SetActive(false);  
    }

    void UpdateJsonFile()
    {
        string jsonString = JsonConvert.SerializeObject(modelProperties, Formatting.Indented);
        StartCoroutine(UpdateModelProperties(jsonString));
    }


    public void ShowHideDropdown()
    {
        dropdown.gameObject.SetActive(!dropdown.gameObject.activeSelf);
       // textFieldParent.gameObject.SetActive(!textFieldParent.gameObject.activeSelf);

    }

    public void ShowPanel() {

        OpenPanel.gameObject.SetActive(!OpenPanel.gameObject.activeSelf);


    }
}
