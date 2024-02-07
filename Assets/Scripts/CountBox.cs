using TMPro;
using UnityEngine;

public class CountBox : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _countText;

    private int _count = 0;
    
    public int Count
    {
        get { return _count; }
        set
        {
            _count = value;
            _countText.text = value.ToString();
        }
    }
}
