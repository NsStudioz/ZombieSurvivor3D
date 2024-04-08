

namespace ZombieSurvivor3D.Gameplay.Pickups
{
    public interface IPickupable
    {
        void OnPickup();

        void OnIgnored();
    }
}
