using UnityEngine;
using ZombieSurvivor3D.Gameplay.RNG;

namespace ZombieSurvivor3D.Gameplay.Buffs
{
    /// <summary>
    /// GameObject Item. 
    /// Will trigger a randomized number to supply to the buff randomizer system.
    /// </summary>
    public class BuffBox : MonoBehaviour
    {
        const string PLAYER_TAG = "Player";

        private void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.CompareTag(PLAYER_TAG))
            {
                Debug.Log("Hit by Player");
                NumberGenerator.GenerateForBuffs();
                //Destroy(gameObject);
            }     
        }
    }
}
