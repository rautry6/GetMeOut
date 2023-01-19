using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace
{
    public class Slime : EnemyBase
    {
        [SerializeField] private Rigidbody2D slimeRigidbody;
        private static readonly int IsMoving = Animator.StringToHash("isMoving");
        private static readonly int Dead = Animator.StringToHash("Dead");
        private static readonly int Hit = Animator.StringToHash("Hit");

        protected override void EnemyIdle()
        {
            Debug.Log(Vector2.Distance(transform.position, player.transform.position));
        }

        protected override void EnemyMove()
        {
        }

        public void OnSlimeDie()
        {
            enemyAnimator.SetTrigger(Hit);
            UpdateCurrentState(EnemyStates.Dead);
        }

        #region AnimationEvents

        private void DisableSlime()
        {
            StartCoroutine(DeadSlime());
        }
        private void PauseBeforeStartDeath()
        {
            StartCoroutine(PauseBeforeDeath());
        }

        #endregion

        private IEnumerator DeadSlime()
        {
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
            yield return new WaitForSeconds(2f);
            gameObject.SetActive(true);
        }

        private IEnumerator PauseBeforeDeath()
        {
            yield return new WaitForSeconds(.5f);
            enemyAnimator.SetTrigger(Dead);
        }
    }
}