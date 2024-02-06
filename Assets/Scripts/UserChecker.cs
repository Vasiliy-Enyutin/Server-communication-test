using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class UserChecker : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField phoneNumberInputField;
    [SerializeField]
    private TextMeshProUGUI _resultText;

    public event Action<string> OnUserNoExist;

    private string _id;
    private string _phoneNumber;

    public void CheckUser()
    {
        _id = UserInformation.ID;
        _phoneNumber = phoneNumberInputField.text;

        if (!IsValidPhoneNumber(_phoneNumber))
        {
            Debug.LogError("Invalid phone number format.");
            return;
        }

        StartCoroutine(CheckUserCoroutine());
    }

    private IEnumerator CheckUserCoroutine()
    {
        WWWForm form = new();
        form.AddField("ID", _id );
        form.AddField("Phone", _phoneNumber);
        
        string url = "http://45.86.183.61/Test/CheckUser.php";
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
            if (response.Equals("Exist"))
            {
                _resultText.text = "User already exist";
            }
            else if (response.Equals("NoExist"))
            {
                _resultText.text = "User does not exist";
                OnUserNoExist?.Invoke(_phoneNumber);
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
