using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ZombieSurvivor3D.Gameplay.Handheld
{
    public interface IHandheldObject : ControlsPC.IGameplayControlsActions
    {

        void OnAttachedCarrier(HandheldCarrier attachedCarrier);
        void OnEquip();
        void OnUnequip();

    }
}