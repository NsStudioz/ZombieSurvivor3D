using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvivor3D
{
    public class TurretOld : MonoBehaviour
    {
        // Start is called before the first frame update
        void LockRotation()
        {
            //Backup:
            //  Vector3 dir = target.position - transform.position;
            //partToRotate.rotation = Quaternion.Euler(ZEROED_VALUE, rotation.y, ZEROED_VALUE);
        }


    }
}
