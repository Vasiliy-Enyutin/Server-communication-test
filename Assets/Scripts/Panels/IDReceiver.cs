using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace Panels
{
    public class IDReceiver : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _resultText;
    
        public event Action<string> OnIDReceived;

        public void GetIDFromServer()
        {
            StartCoroutine(GetIDCoroutine());
        }

        private IEnumerator GetIDCoroutine()
        {
            UnityWebRequest www = UnityWebRequest.Get("http://45.86.183.61/Test/GetKey.php");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
                _resultText.text = "Error: Failed to get ID from server";
            }
            else
            {
                string jsonString = www.downloadHandler.text;
                KeyData keyData = JsonUtility.FromJson<KeyData>(jsonString);
                string id = FindValidKey(keyData);
                if (id != null)
                {
                    _resultText.text = "ID successfully received: " + id;
                    OnIDReceived?.Invoke(id);
                }
                else
                {
                    _resultText.text = "No valid ID found";
                }
            }
        }

        private string FindValidKey(KeyData keyData)
        {
            foreach (KeyValue key in keyData.Keys)
            {
                if (key.Key != "No Key")
                {
                    return GetEverySecondLetter(key.Key);
                }
            }
            return null;
        }
    
        private string GetEverySecondLetter(string input)
        {
            if (string.IsNullOrEmpty(input) || input.Length < 2)
            {
                return null; // Возвращаем null, если входная строка пустая или короче двух символов
            }

            return new string(input.Where((_, i) => i % 2 == 0).ToArray());
        }
    }
}