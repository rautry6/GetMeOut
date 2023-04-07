using System.Collections;
using DG.Tweening;
using UnityEngine;

public class TriggerBossDoorShut : MonoBehaviour
{
    [SerializeField] private Transform resetDoorPosition;
    [SerializeField] private GameObject door;
    [SerializeField] private DeafBoss boss;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(TriggerBossDoorShutRoutine(other.gameObject));
        }
    }

    IEnumerator TriggerBossDoorShutRoutine(GameObject player)
    {
        var playerMove = player.GetComponent<Move>();
        var playerJump = player.GetComponent<Jump>();
        
        playerMove.StopMovement();
        playerJump.DisableJumping();

        door.transform.DOMoveY(resetDoorPosition.position.y, 1.5f).OnComplete(() =>
        {
            boss.UpdateToWander();
        });

        yield return new WaitForSeconds(2.5f);
        playerMove.RegainMovement();
        playerJump.EnableJumping();
        Destroy(gameObject);
    }
}
