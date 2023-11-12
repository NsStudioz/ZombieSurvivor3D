using System;

namespace ZombieSurvivor3D
{
    public class NumberGenerator
    {

        public static event Action<int> OnRandomNumberGenerated;

        public void Generate()
        {
            int rnd = UnityEngine.Random.Range(1, 100);
            OnRandomNumberGenerated?.Invoke(rnd);
        }
    }
}
