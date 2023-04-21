using System.Collections;
using UnityEngine;

public class PlayerSFXManager : MonoBehaviour
{
    [SerializeField] private AudioSource playerWalkSFX;
    [SerializeField] private AudioSource playerJumpSFX;
    [SerializeField] private AudioSource playerHurtSFX;
    [SerializeField] private float walkClipDelay = .5f;
    [SerializeField] private AudioSource playerDeathSFX;
    
    private bool _coroutinePlaying;

    public void PlayWalkSFX()
    {
        if (!_coroutinePlaying)
        {
            if (!playerWalkSFX.isPlaying)
            {
                playerWalkSFX.PlayOneShot(playerWalkSFX.clip);
                StartCoroutine(PlayWalkSoundCooldown());
            }
        }
    }

    private IEnumerator PlayWalkSoundCooldown()
    {
        _coroutinePlaying = true;
        yield return new WaitForSeconds(walkClipDelay);
        _coroutinePlaying = false;
    }
    public void PlayJumpSFX()
    {
        playerJumpSFX.PlayOneShot(playerJumpSFX.clip);
    }

    public void PlayHurtSFX()
    {
        playerHurtSFX.PlayOneShot(playerHurtSFX.clip);
    }

    public void PlayDeathSFX()
    {
        playerDeathSFX.PlayOneShot(playerDeathSFX.clip);
    }

}
