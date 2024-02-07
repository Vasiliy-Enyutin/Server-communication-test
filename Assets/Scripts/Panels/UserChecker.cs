using System;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace Panels
{
    public class UserChecker : MonoBehaviour, IPhasePanel
    {
        [SerializeField]
        private TMP_InputField phoneNumberInputField;
        [SerializeField]
        private TextMeshProUGUI _resultText;

        public event Action OnSuccess;

        private const string CHECK_USER_URI = "http://45.86.183.61/Test/CheckUser.php";

        private string _id;
        private string _phoneNumber;

        public void CheckUser()
        {
            _id = UserInformation.ID;
            _phoneNumber = phoneNumberInputField.text;

            if (!IsValidPhoneNumber(_phoneNumber))
            {
                _resultText.text = "Invalid phone number format.";
                phoneNumberInputField.text = "";
                return;
            }

            StartCoroutine(CheckUserCoroutine());
        }

        private IEnumerator CheckUserCoroutine()
        {
            WWWForm form = new();
            form.AddField("ID", _id );
            form.AddField("Phone", _phoneNumber);
        
            UnityWebRequest www = UnityWebRequest.Post(CHECK_USER_URI, form);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
                _resultText.text = "Error: Request to server failed";
            }
            else
            {
                string response = www.downloadHandler.text;
                if (response.Equals("Exist"))
                {
                    _resultText.text = "User already exist";
                }
                else if (response.Equals("NoExist"))
                {
                    _resultText.text = "User does not exist";
                    UserInformation.PhoneNumber = _phoneNumber;
                    OnSuccess?.Invoke();
                }
                else
                {
                    _resultText.text = "Unexpected response";
                }
            }
        }

        // Метод для проверки корректности формата номера телефона
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^\+\d{1,3}\(\d{3}\)\d{7}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }
    }
}
