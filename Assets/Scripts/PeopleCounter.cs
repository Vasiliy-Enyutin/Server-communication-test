using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PeopleCounter : MonoBehaviour
{
    [SerializeField] private CountBox _countBoxPrefab;

    
    private int _maxCountBoxes;
    private Queue<CountBox> _countBoxQueue;
    private float getPeopleCountTimeout = 5;
    
    private const string URI = "http://45.86.183.61/Test/HowMany.php";

    private void Awake()
    {
        GridLayoutGroup gridLayoutGroup = GetComponent<GridLayoutGroup>();
        RectTransform rectTransform = GetComponent<RectTransform>();
        _maxCountBoxes = Mathf.FloorToInt((rectTransform.rect.height + gridLayoutGroup.spacing.y) / (gridLayoutGroup.spacing.y +
                         gridLayoutGroup.cellSize.y));
        _countBoxQueue = new Queue<CountBox>(_maxCountBoxes);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetPeopleCount), 0f, getPeopleCountTimeout);
    }
    
    private void GetPeopleCount()
    {
        StartCoroutine(GetPeopleCountCoroutine());
    }

    private IEnumerator GetPeopleCountCoroutine()
    {
        UnityWebRequest www = UnityWebRequest.Get(URI);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + www.error);
        }
        else
        {
            string response = www.downloadHandler.text;
            UpdateCountBoxes(response);
        }
    }

    private void UpdateCountBoxes(string response)
    {
        if (!int.TryParse(response, out int receivedCount))
        {
            Debug.LogError("Failed to parse count: " + response);
            return;
        }
        if (_countBoxQueue.Count > 0)
        {
            if (_countBoxQueue.Last().Count == receivedCount)
            {
                return;
            }
        }
        
        if (_countBoxQueue.Count == _maxCountBoxes)
        {
            CountBox oldestCountBox = _countBoxQueue.Dequeue();
            Destroy(oldestCountBox.gameObject);
        }

        CountBox newCountBox = Instantiate(_countBoxPrefab, transform);
        newCountBox.Count = receivedCount;
        _countBoxQueue.Enqueue(newCountBox);
    }
}