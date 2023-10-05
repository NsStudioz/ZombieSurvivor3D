using UnityEngine;


namespace ZombieSurvivor3D.Gameplay.Handheld
{
    public class HandheldSOTag : MonoBehaviour
    {
        [SerializeField] HandheldSO handheldSO_Tag;

        public HandheldSO GetHandheldSOTag()
        {
            return handheldSO_Tag;
        }
    }
}

