using System;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace Panels
{
    public class UserRegistrator : MonoBehaviour, IPhasePanel
    {
        [SerializeField]
        private TextMeshProUGUI _resultText;

        public event Action OnSuccess;

        private string _id;
        private string _phoneNumber;
        
        public void RegisterUser()
        {
            _id = UserInformation.ID.Substring(0, UserInformation.ID.Length - 1);
            _phoneNumber = UserInformation.PhoneNumber;
            StartCoroutine(RegisterUserCoroutine());
        }

        private IEnumerator RegisterUserCoroutine()
        {
            // Разбиваем номер телефона на код страны, код оператора и номер
            string[] phoneParts = ParsePhoneNumber(_phoneNumber);

            WWWForm form = new();
            form.AddField("ID", _id);
            form.AddField("Country", phoneParts[0]);
            form.AddField("Operator", phoneParts[1]);
            form.AddField("Number", phoneParts[2]);

            string url = "http://45.86.183.61/Test/RegUsers.php";
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
                _resultText.text = "Error: Request to server failed";
            }
            else
            {
                string response = www.downloadHandler.text;
                if (response.Equals("RegOK"))
                {
                    _resultText.text = "User successfully registered";
                    OnSuccess?.Invoke();
                }
                else
                {
                    _resultText.text = "Registration failed: Unexpected response";
                }
            }
        }

        private static string[] ParsePhoneNumber(string phoneNumber)
        {
            string numericPhoneNumber = Regex.Replace(phoneNumber, @"\D", "");
            
            string country = "+" + numericPhoneNumber[0];
            string operatorCode = numericPhoneNumber.Substring(1, 3);
            string number = numericPhoneNumber.Substring(4);

            return new[] { country, operatorCode, number };
        }

    }
}