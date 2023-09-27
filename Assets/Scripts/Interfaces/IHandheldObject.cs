using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSurvivor.Carrier;

namespace ZombieSurvivor.Interfaces
{
    public interface IHandheldObject : ControlsPC.IGameplayControlsActions
    {

        void OnAttachedCarrier(CarrierSystem attachedCarrier);
        void OnEquip();
        void OnUnequip();

    }
}