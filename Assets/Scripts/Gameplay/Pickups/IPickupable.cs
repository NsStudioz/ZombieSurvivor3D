

namespace ZombieSurvivor3D.Gameplay.Pickups
{
    public interface IPickupable
    {
        void OnSpawned();

        void OnIgnored();

        void OnPicked();
    }
}
