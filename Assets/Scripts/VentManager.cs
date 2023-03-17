using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VentManager : MonoBehaviour
{
    [SerializeField] private Move playerMove;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private float duration = 3f;
    [SerializeField] private float delayTime = 3f;
    public GameObject DetectedVent { get; set; }
    public bool IsInVentMovement { get; private set; }

    public void MovePlayerThroughVent()
    {
        if (DetectedVent == null) return;
        
        var destinationVent = DetectedVent.GetComponent<Vent>().DestinationVent;

        if (destinationVent == null) return;
        
        playerMove.StopMovement();
        IsInVentMovement = true;
        playerSpriteRenderer.enabled = false;
        playerMove.gameObject.transform.DOMove(destinationVent.transform.position, duration).OnComplete(() =>
        {
            playerSpriteRenderer.enabled = true;
            playerMove.RegainMovement();
            StartCoroutine(DelayInteractionAfterVent());
        });
    }

    private IEnumerator DelayInteractionAfterVent()
    {
        yield return new WaitForSeconds(delayTime);
        IsInVentMovement = false;
    }
}