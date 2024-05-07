using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
public class RequestScript : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GetRequest("http://127.0.0.1:5000"));
    }

    [System.Obsolete]
    IEnumerator GetRequest(string uri)
    {   using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();
            if(webRequest.isNetworkError)
            {
                Debug.Log("Error : " + webRequest.error);
            }else
            {
                Debug.Log(webRequest.downloadHandler.text);
            }    
        }
    }
   
}
