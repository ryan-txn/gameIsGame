using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Boss2RoomController : MonoBehaviour
{
    [SerializeField]
    private GameObject jam1;

    [SerializeField]
    private GameObject jam2;

    private HealthController jam1HealthController;
    private HealthController jam2HealthController;

    public UnityEvent OnHalfHealth;
    public UnityEvent OnBothDied;

    private bool _spawnersActive;


    // Start is called before the first frame update
    void Start()
    {
        jam1HealthController = jam1.GetComponent<HealthController>();
        jam2HealthController = jam2.GetComponent<HealthController>();

        for (int i = 2; i < transform.childCount; i++)
        {
            GameObject spawnerObj = transform.GetChild(i).gameObject;
            spawnerObj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (jam1HealthController != null && jam2HealthController != null)
        {
            //if current total health goes below 50%
            if ((jam1HealthController._currentHealth + jam2HealthController._currentHealth)
                    <= (jam1HealthController._maximumHealth + jam2HealthController._maximumHealth) / 2)
            {
                OnHalfHealth.Invoke();
            }
        }

        //if both health == 0
        if ((jam1HealthController._currentHealth + jam2HealthController._currentHealth) <= 0
                || jam1HealthController == null && jam2HealthController == null)
        {
            OnBothDied.Invoke();
        }
    }

    //used in half hp event
    public void ActivateSpawners()
    {
        if (!_spawnersActive)
        {
            Debug.Log("Spawners activated");
            for (int i = 2; i < transform.childCount; i++)
            {
                GameObject spawnerObj = transform.GetChild(i).gameObject;
                spawnerObj.SetActive(true);
                _spawnersActive = true;
            }
        }

    }

    //used in on both died event
    public void DeactivateSpawners()
    {
        if (_spawnersActive)
        {
            Debug.Log("Spawners deactivated");
            for (int i = 2; i < transform.childCount; i++)
            {
                GameObject spawnerObj = transform.GetChild(i).gameObject;
                spawnerObj.SetActive(false);
            }

            EnemyCounter.SetEnemies(0);
            _spawnersActive = false;
        }
    }
}
