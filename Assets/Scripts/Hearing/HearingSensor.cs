using System.Collections;
using System.Collections.Generic;
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

    // Update is called once per frame
    void Update()
    {

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

        LinkedBoss.ReportSoundHeard(location, category, intensity);
    }
}
