using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SonicHowlManager : MonoBehaviour
{
    [SerializeField] private GameObject _sonicAttackPrefab;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Image _uiIndicator;
    [SerializeField] private GameObject _directionalIndicator;
    [SerializeField] private float _shootSpeed = 4;
    private float _uiIndicatorStartingXScale = 12f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(_playerTransform.position.y - transform.position.y < 4) {
            ResetUIScale();
            return; 
        }

        _uiIndicator.transform.localScale = new Vector3(_uiIndicator.transform.localScale.x - _shootSpeed * Time.deltaTime, _uiIndicator.transform.localScale.y, _uiIndicator.transform.localScale.z); ;

        if(_uiIndicator.transform.localScale.x <= 0 )
        {
            _uiIndicator.transform.localScale = new Vector3(_uiIndicatorStartingXScale, _uiIndicator.transform.localScale.y, _uiIndicator.transform.localScale.z);

            var spawnedProjectile = Instantiate(_sonicAttackPrefab, transform.position, Quaternion.identity);
            var spawnedIndicator = Instantiate(_directionalIndicator, transform.position, Quaternion.identity);
            var directionToPlayer = _playerTransform.transform.position - transform.position;
            var angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

            spawnedProjectile.transform.rotation = Quaternion.Euler(0, 0, angle);
            spawnedIndicator.transform.rotation = Quaternion.Euler(0,0, angle -90f);
        }
    }

    void ResetUIScale()
    {
        _uiIndicator.transform.localScale = new Vector3(_uiIndicatorStartingXScale, _uiIndicator.transform.localScale.y, _uiIndicator.transform.localScale.z);
    }
}