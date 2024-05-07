using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;
    public GameObject loginPanel;
    public GameObject nextPagePanel;
    public TMP_Text invalidText;

    private string baseUrl = "http://127.0.0.1:5000";  

    public void OnLoginButtonClick()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        StartCoroutine(ValidateLogin(username, password));
    }

    private IEnumerator ValidateLogin(string username, string password)
    {
        string url = baseUrl + $"/validate-login?username={UnityWebRequest.EscapeURL(username)}&password={UnityWebRequest.EscapeURL(password)}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseJson = request.downloadHandler.text;
                bool isValid = JsonUtility.FromJson<LoginResponse>(responseJson).valid;

                if (isValid)
                {
                    Debug.Log("Login successful!");
                    loginPanel.SetActive(false);
                    nextPagePanel.SetActive(true);
                }
                else
                {
                    ShowInvalidMessage();
                }
            }
            else
            {
                Debug.LogError("Login request failed.");
            }
        }
    }

    private void ShowInvalidMessage()
    {
        invalidText.text = "Invalid username or password.";
        invalidText.gameObject.SetActive(true);
    }

    [System.Serializable]
    private class LoginResponse
    {
        public bool valid;
    }
}
