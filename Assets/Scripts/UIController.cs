using Panels;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private IDReceiver _idReceiver;
    [SerializeField]
    private UserChecker _userChecker;
    
    private void OnEnable()
    {
        _idReceiver.gameObject.SetActive(true);
        _idReceiver.OnIDReceived += HandleIDReceived;
        _userChecker.OnUserNoExist += HandleUserNoExist;
    }

    private void OnDisable()
    {
        _idReceiver.OnIDReceived -= HandleIDReceived;
        _userChecker.OnUserNoExist -= HandleUserNoExist;
    }

    private void HandleIDReceived(string id)
    {
        UserInformation.ID = id;
        _idReceiver.gameObject.SetActive(false);
        _userChecker.gameObject.SetActive(true);
    }

    private void HandleUserNoExist(string phoneNumber)
    {
        UserInformation.PhoneNumber = phoneNumber;
        _userChecker.gameObject.SetActive(false);
    }
}