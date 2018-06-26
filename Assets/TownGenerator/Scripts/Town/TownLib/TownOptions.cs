using UnityEngine;

namespace Town
{
    [System.Serializable]
    public class TownOptions
    {
        public bool Overlay = true;
        public bool Walls = true;
        public bool Water = true;
        [Range (20, 45)]
        public int Patches = 35;
        public int Seed = 100;

        public static TownOptions Default => new TownOptions { Patches = 15 };
    }
}