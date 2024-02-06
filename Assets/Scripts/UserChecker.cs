using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class UserChecker
{
    public event Action<string> OnFinishCheckingUser;

    public void CheckUser(string id, string phoneNumber)
    {
        CoroutineHandler.Instance.StartMyCoroutine(CheckUserCoroutine(id, phoneNumber));
    }

    private IEnumerator CheckUserCoroutine(string id, string phoneNumber)
    {
        string url = "http://45.86.183.61/Test/CheckUser.php?id=" + id + "&phone=" + phoneNumber;
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            string response = www.downloadHandler.text;
            if (response.Equals("Exist"))
            {
                OnFinishCheckingUser?.Invoke("User already exists");
            }
            else if (response.Equals("NoExist"))
            {
                OnFinishCheckingUser?.Invoke("User does not exist");
            }
        }
    }
}