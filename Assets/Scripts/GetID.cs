using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Networking;

public class GetID : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI resultText;

    public void GetIDFromServer()
    {
        StartCoroutine(GetIDCoroutine());
    }

    IEnumerator GetIDCoroutine()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://45.86.183.61/Test/GetKey.php");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
            resultText.text = "Error: Failed to get ID from server";
        }
        else
        {
            string jsonString = www.downloadHandler.text;
            KeyData keyData = JsonUtility.FromJson<KeyData>(jsonString);
            string id = FindFirstValidKey(keyData);
            if (id != null)
            {
                resultText.text = "ID successfully received: " + id;
            }
            else
            {
                resultText.text = "No valid ID found";
            }
        }
    }
    
    private string FindFirstValidKey(KeyData keyData)
    {
        foreach (KeyValue key in keyData.Keys)
        {
            if (key.Key != "No Key")
            {
                return key.Key;
            }
        }
        return null;
    }
}