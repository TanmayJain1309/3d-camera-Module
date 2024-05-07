using UnityEngine;
using System.Collections.Generic;

public class Get3DModelsExceptPlane : MonoBehaviour
{
    void Start()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        List<GameObject> modelsExceptPlane = new List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.GetComponent<MeshRenderer>() != null && obj.name != "Plane")
            {
                modelsExceptPlane.Add(obj);
            }
        }

        foreach (GameObject model in modelsExceptPlane)
        {
            Debug.Log("Model Name: " + model.name);
        }
    }
}
