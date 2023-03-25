using System.Collections;
using UnityEngine;

public class HandlePCInteraction : MonoBehaviour
{
    [SerializeField] private GameObject pcInteractionUI;
    [SerializeField] private PCDetectsPlayer pcDetectsPlayer;
    [SerializeField] private Move playerMove;
    [SerializeField] private Jump playerJump;

    private bool _isInRoutine;
    private bool _isDone;

    private void Awake()
    {
        _isInRoutine = false;
        _isDone = false;
    }

    // animation event
    public void HandleStartPCRoutine()
    {
        if (_isInRoutine == false)
        {
            StartCoroutine(PCInteractionRoutine());
        }
    }

    private void Update()
    {
        if (_isInRoutine && Input.GetKeyDown(KeyCode.F))
        {
            _isDone = true;
        }
    }

    private IEnumerator PCInteractionRoutine()
    {
        _isInRoutine = true;
        playerMove.StopMovement();
        playerJump.DisableJumping();
        pcInteractionUI.SetActive(true);
        yield return new WaitUntil(() => _isDone);
        pcInteractionUI.SetActive(false);
        playerMove.RegainMovement();
        playerJump.EnableJumping();
        yield return new WaitForSeconds(.75f);
        pcDetectsPlayer.TriggerOff();
        pcDetectsPlayer.SetHasInteracted(false);
        _isInRoutine = false;
        _isDone = false;
    }
}
