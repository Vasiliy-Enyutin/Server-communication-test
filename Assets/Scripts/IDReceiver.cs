using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class IDReceiver
{
    public event Action<string> OnFinishReceivingID;

    public void GetIDFromServer()
    {
        CoroutineHandler.Instance.StartMyCoroutine(GetIDCoroutine());
    }

    private IEnumerator GetIDCoroutine()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://45.86.183.61/Test/GetKey.php");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            string jsonString = www.downloadHandler.text;
            KeyData keyData = JsonUtility.FromJson<KeyData>(jsonString);
            string id = FindValidKey(keyData);
            OnFinishReceivingID?.Invoke(id);
        }
    }

    private string FindValidKey(KeyData keyData)
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