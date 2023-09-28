using UnityEngine;


namespace ZombieSurvivor3D.Gameplay.Handheld
{
    public class HandheldSOTag : MonoBehaviour
    {
        [SerializeField] private HandheldSO HandheldSO_Tag;

        public HandheldSO GetHandheldSOTag()
        {
            return HandheldSO_Tag;
        }
    }
}

