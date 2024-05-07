using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class DataManager : MonoBehaviour
{
    static string serverURL = "http://127.0.0.1:5000/cube_data"; // Update with your Flask server URL
    public static IEnumerator UpdateData(IDictionary data)
    {
        string jsonData = JsonConvert.SerializeObject(data);
        using (UnityWebRequest request = UnityWebRequest.Post(serverURL + "/update_data", jsonData))
        {
            ////request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error updating data: " + request.error);
            }
            else
            {
                Debug.Log("Data updated successfully");
            }
        }
    }

    public static IEnumerator GetData(System.Action<Dictionary<string, object>> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(serverURL + "/get_data"))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error retrieving data: " + request.error);
            }
            else
            {
                string jsonData = request.downloadHandler.text;
                Dictionary<string, object> data = JsonUtility.FromJson<Dictionary<string, object>>(jsonData);
                callback?.Invoke(data);
            }
        }
    }
}