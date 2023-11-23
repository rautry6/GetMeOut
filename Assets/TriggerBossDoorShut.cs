using System.Collections;
using DG.Tweening;
using UnityEngine;

public class TriggerBossDoorShut : MonoBehaviour
{
    [SerializeField] private Transform resetDoorPosition;
    [SerializeField] private GameObject door;
    [SerializeField] private BlindBoss boss;
    [SerializeField] private BossPlatformSpawning platforms;
    [SerializeField] private CutsceneManager cutscene;
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
        var playerRigidBody = player.GetComponent<Rigidbody2D>();
        
        player.GetComponentInChildren<PlayerAnimations>().PlayerAnimator.Play("Player_Idle");
        playerMove.StopMovement();
        playerJump.DisableJumping();
        playerRigidBody.gravityScale = 1f;

        door.transform.DOMoveY(resetDoorPosition.position.y, 1.5f).OnComplete(() =>
        {
            //boss.UpdateToWander();
            cutscene.StartCutscene(2);
        });
        yield return new WaitForSeconds(2.5f);

        yield return new WaitWhile(() => cutscene._inCutscene);
        //playerMove.RegainMovement();
        //playerJump.EnableJumping();
        platforms.EnableSpawning();
        Destroy(gameObject);
    }
}
