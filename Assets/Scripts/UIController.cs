using System.Collections;
using System.Linq;
using Panels;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _phasePanels;

    private const float PHASE_CHANGE_TIMEOUT = 2f;
    
    private IPhasePanel[] _panels;
    private int _currentPanelNumber = 0;

    private void OnEnable()
    {
        _panels = new IPhasePanel[_phasePanels.Length];
        _panels = _phasePanels.Select(panel => panel.GetComponent<IPhasePanel>()).ToArray();
        foreach (IPhasePanel panel in _panels)
        {
            panel.OnSuccess += HandlePhaseSuccess;
        }
        
        _phasePanels[_currentPanelNumber].gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        foreach (IPhasePanel panel in _panels)
        {
            panel.OnSuccess -= HandlePhaseSuccess;
        }
    }

    private void HandlePhaseSuccess()
    {
        StartCoroutine(ChangePhasePanelRoutine());
    }

    private IEnumerator ChangePhasePanelRoutine()
    {
        yield return new WaitForSeconds(PHASE_CHANGE_TIMEOUT);
        _phasePanels[_currentPanelNumber].gameObject.SetActive(false);
        if (_currentPanelNumber + 1 < _panels.Length)
        {
            _currentPanelNumber++;
            _phasePanels[_currentPanelNumber].gameObject.SetActive(true);
        }
    }
}