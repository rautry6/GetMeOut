using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoorCutscene : MonoBehaviour
{
    [SerializeField] private CutsceneManager cutscene;
    [SerializeField] private int cutsceneIndex;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !CutsceneData.Instance.CutsceneIndexList.Contains(cutsceneIndex))
        {
            cutscene.StartCutscene(cutsceneIndex);
            Destroy(gameObject);
        }
    }
}
