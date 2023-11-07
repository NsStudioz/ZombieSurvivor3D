using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ZombieSurvivor3D
{
    public class SpawnObjectsAddressables : MonoBehaviour
    {

        [SerializeField] private AssetLabelReference assetLabelReference;


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                Addressables.LoadAssetAsync<GameObject>(assetLabelReference).Completed +=
                    (AsyncOperationHandle) =>
                    {
                        if (AsyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                            Instantiate(AsyncOperationHandle.Result);
                        else
                            Debug.Log("Failed to load!");
                    }; 
            }
        }
    }
}
