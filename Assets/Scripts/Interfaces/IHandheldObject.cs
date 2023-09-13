using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHandheldObject : ControlsPC.IGameplayControlsActions
{

    void OnAttachedCarrier(CarrierSystem attachedCarrier);
    void OnEquip();
    void OnUnequip();

}
