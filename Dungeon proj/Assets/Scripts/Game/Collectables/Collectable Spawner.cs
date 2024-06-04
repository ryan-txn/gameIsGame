using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _collectablePrefabs; //list of spawnable collectables

    public void SpawnCollectable(Vector2 position)
    {
        bool rightProbability = CheckTotalProbability();
        if (rightProbability == false) 
        {
            Debug.LogError("Probabilities dont add to 100. Check probabilities.");
        }

        int randomPoint = Random.Range(0, 100);
        GameObject selectedCollectable = null;

        foreach (GameObject collectable in _collectablePrefabs)
        {
            DropProbability dropProbability = collectable.GetComponent<DropProbability>();
            if (dropProbability != null)
            {
                if (randomPoint < dropProbability.probability)
                {
                    selectedCollectable = collectable;
                    break;
                }
                else
                {
                    randomPoint -= dropProbability.probability;
                }
            }
        }

        if (selectedCollectable != null)
        {
            Instantiate(selectedCollectable, position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("No collectable selected. Check probabilities.");
        }
    }

    private bool CheckTotalProbability()
    {
        int total = 0;

        foreach (GameObject collectable in _collectablePrefabs)
        {
            DropProbability dropProbability = collectable.GetComponent<DropProbability>();
            if (dropProbability != null)
            {
                total += dropProbability.probability;
            }
            else
            {
                Debug.LogWarning("Collectable does not have a DropProbability script attached: " + collectable.name);
            }
        }

        return (total == 100);
    }
}
