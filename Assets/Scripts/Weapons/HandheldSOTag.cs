using UnityEngine;

namespace ZombieSurvivor.Carrier.Handheld.Tag
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

