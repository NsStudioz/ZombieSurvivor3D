using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHandheldObject
{

    void OnAttachedCarrier(CarrierSystem attachedCarrier);
    void OnEquip();
    void OnUnequip();

}
