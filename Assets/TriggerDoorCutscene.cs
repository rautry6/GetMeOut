using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoorCutscene : MonoBehaviour
{
    [SerializeField] private CutsceneManager cutscene;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(DoorCutsceneRoutine(other.gameObject));
        }

        IEnumerator DoorCutsceneRoutine(GameObject player)
        {
            cutscene.StartCutscene();
            yield return new WaitForSeconds(2.5f);

            yield return new WaitWhile(() => cutscene._inCutscene);
            Destroy(gameObject);
        }
    }
}
