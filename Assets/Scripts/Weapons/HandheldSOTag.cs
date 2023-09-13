using UnityEngine;

public class HandheldSOTag : MonoBehaviour
{
    [SerializeField] private HandheldSO HandheldSO_Tag;

    public HandheldSO GetHandheldSOTag() 
    { 
        return HandheldSO_Tag;
    }
}
