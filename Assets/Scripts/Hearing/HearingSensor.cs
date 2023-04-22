using UnityEngine;
using static HearingManager;

[RequireComponent(typeof(DeafBoss))]
public class HearingSensor : MonoBehaviour
{
    private DeafBoss LinkedBoss;

    // Start is called before the first frame update
    void Start()
    {
        LinkedBoss = GetComponent<DeafBoss>();
        HearingManager.Instance.Register(this);
    }

    private void OnDestroy()
    {
        if(HearingManager.Instance != null)
        {
            HearingManager.Instance.DeRegister(this);
        }
    }

    public void OnSoundHeard(Vector3 location, EHeardSoundCategory category, float intensity)
    {
        if(Vector3.Distance(location, LinkedBoss.transform.position) > LinkedBoss.HearingRange && category != EHeardSoundCategory.ECrash)
        {
            return;
        }

        if (LinkedBoss.GetCurrentState() != BossStates.Idle) {
            LinkedBoss.ReportSoundHeard(location, category, intensity);
        }
    }
}
