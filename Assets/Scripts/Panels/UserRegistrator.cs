using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace Panels
{
    public class UserRegistrator : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _resultText;

        public static IEnumerator RegisterUser(string id, string phoneNumber, TextMeshProUGUI resultText)
        {
            // Разбиваем номер телефона на код страны, код оператора и номер
            string[] phoneParts = ParsePhoneNumber(phoneNumber);

            WWWForm form = new WWWForm();
            form.AddField("ID", id);
            form.AddField("Country", phoneParts[0]);
            form.AddField("Operator", phoneParts[1]);
            form.AddField("Number", phoneParts[2]);

            string url = "http://45.86.183.61/Test/RegUsers.php";
            using (UnityWebRequest www = UnityWebRequest.Post(url, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError(www.error);
                    resultText.text = "Error: Request to server failed";
                }
                else
                {
                    string response = www.downloadHandler.text;
                    if (response.Equals("RegOK"))
                    {
                        resultText.text = "User successfully registered";
                    }
                    else
                    {
                        resultText.text = "Registration failed: Unexpected response";
                    }
                }
            }
        }

        private static string[] ParsePhoneNumber(string phoneNumber)
        {
            // Разбиение номера телефона на код страны, код оператора и номер
            // Ваша реализация разбиения
            return new string[] { "+7", "916", "5586998" }; // Пример для тестирования
        }
    }
}