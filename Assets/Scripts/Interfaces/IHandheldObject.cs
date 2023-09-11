using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHandheldObject
{

    void OnAttachedToPlayer();
    void OnEquip();
    void OnUnequip();

}
