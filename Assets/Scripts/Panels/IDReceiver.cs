using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace Panels
{
    public class IDReceiver : MonoBehaviour, IPhasePanel
    {
        [SerializeField]
        private TextMeshProUGUI _resultText;
    
        public event Action OnSuccess;

        private const string GET_KEY_URI = "http://45.86.183.61/Test/GetKey.php";

        public void GetIDFromServer()
        {
            StartCoroutine(GetIDCoroutine());
        }

        private IEnumerator GetIDCoroutine()
        {
            UnityWebRequest www = UnityWebRequest.Get(GET_KEY_URI);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
                _resultText.text = "Error: Failed to get ID from server";
                yield break;
            }
  
            string jsonString = www.downloadHandler.text;
            KeyData keyData = JsonUtility.FromJson<KeyData>(jsonString);
            string id = FindValidKey(keyData);
            if (id != null)
            {
                _resultText.text = "ID successfully received: " + id;
                UserInformation.ID = id;
                OnSuccess?.Invoke();
            }
            else
            {
                _resultText.text = "No valid ID found";
            }
            
        }

        private string FindValidKey(KeyData keyData)
        {
            return keyData.Keys
                .Where(key => key.Key != "No Key")
                .Select(key => GetEverySecondLetter(key.Key))
                .FirstOrDefault();
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