using TMPro;
using UnityEngine;

public class ButtonsController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _resultText;
    
    private IDReceiver idReceiver;
    private int _id;

    private void OnEnable()
    {
        idReceiver = new IDReceiver();

        idReceiver.OnFinishReceivingID += HandleFinishReceivingID;
    }

    private void OnDisable()
    {
        idReceiver.OnFinishReceivingID -= HandleFinishReceivingID;
    }

    public void OnGetIDButtonClick()
    {
        idReceiver.GetIDFromServer();
    }

    private void HandleFinishReceivingID(string result)
    {
        if (int.TryParse(result, out int id))
        {
            _resultText.text = $"ID successfully received: {id}";
            _id = id;
            // далее следующая панель
        }
        else
        {
            _resultText.text = "No valid ID found";
        }
    }
}