using UnityEngine;

public class PCDetectsPlayer : MonoBehaviour
{
    private Animator _animator;
    private readonly int _interactable = Animator.StringToHash("Interactable");
    private readonly int _hasInteracted = Animator.StringToHash("HasInteracted");
    private readonly int _off = Animator.StringToHash("Off");
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerInteractable();
        }
    }

    private void TriggerInteractable()
    {
        _animator.SetTrigger(_interactable);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerOff();
        }
    }

    public void SetHasInteracted(bool hasInteracted)
    {
        _animator.SetBool(_hasInteracted, hasInteracted);
    }

    public void TriggerOff()
    {
        _animator.SetTrigger(_off);
    }
}