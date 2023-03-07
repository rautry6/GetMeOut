using UnityEngine;
using DG.Tweening;

public class VentManager : MonoBehaviour
{
    [SerializeField] private Move playerMove;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private float duration = 3f;

    public GameObject DetectedVent { get; set; }

    public void MovePlayerThroughVent()
    {
        if (DetectedVent == null) return;
        
        var destinationVent = DetectedVent.GetComponent<Vent>().DestinationVent;

        if (destinationVent == null) return;
        
        playerMove.StopMovement();
        playerSpriteRenderer.enabled = false;
        playerMove.gameObject.transform.DOMove(destinationVent.transform.position, duration).OnComplete(() =>
        {
            playerSpriteRenderer.enabled = true;
            playerMove.RegainMovement();
        });
    }
}