using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Something went wrong. there are more than 1 buildmanagers in the game!");
            return;
        }
        Instance = this;
    }

    #region SpawnObject

    public void SpawnObject(Queue<GameObject> queueGO, int maxQueueCount ,GameObject prefabInstance, Vector3 position, Quaternion rotation)
    {
        if(queueGO.Count > 0)
            PopObjectFromPool(queueGO, position, rotation);

        else if (queueGO.Count <= 0)
        {
            GameObject newInstance = Instantiate(prefabInstance, position, rotation);
            // Observe this line of code!:
            newInstance.transform.SetParent(this.transform, true);
        }
    }

    private void PopObjectFromPool(Queue<GameObject> queueGO, Vector3 position, Quaternion rotation)
    {
        GameObject existingPrefabInstance = queueGO.Dequeue();
        existingPrefabInstance.SetActive(true);
        SetObjectsPositionAndRotation(existingPrefabInstance, position, rotation);
    }
    
    private void SetObjectsPositionAndRotation(GameObject instance, Vector3 position, Quaternion rotation)
    {
        instance.transform.position = position;
        instance.transform.rotation = rotation;
    }

    #endregion


    #region DespawnObject
    
    public void DespawnObjectDelay(float timeDelay, Queue<GameObject> queueGO, GameObject instanceToDespawn, Vector3 position, Quaternion rotation)
    {
        StartCoroutine(DespawnObjectCoroutine(timeDelay, queueGO, instanceToDespawn, position, rotation));
    }
    
    public void DespawnObjectImmediately(Queue<GameObject> queueGO, GameObject instanceToDespawn, Vector3 position, Quaternion rotation)
    {
        DespawnObject(queueGO, instanceToDespawn, position, rotation);
    }

    private void DespawnObject(Queue<GameObject> queueGO, GameObject instanceToDespawn, Vector3 position, Quaternion rotation)
    {
        queueGO.Enqueue(instanceToDespawn);
        instanceToDespawn.SetActive(false);
        SetObjectsPositionAndRotation(instanceToDespawn, position, rotation);
    }

    private IEnumerator DespawnObjectCoroutine(float timeDelay, Queue<GameObject> queueGO, GameObject instanceToDespawn, Vector3 position, Quaternion rotation)
    {
        yield return new WaitForSeconds(timeDelay);
        DespawnObject(queueGO, instanceToDespawn, position, rotation);
    }

    #endregion

}
