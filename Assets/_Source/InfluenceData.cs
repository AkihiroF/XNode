using UnityEngine;

namespace _Source
{
    public class InfluenceData
    {
        public int Influence { get; private set; }

        public void ApplyInfluenceChange(int change) {
            Influence += change;
        }
    }
}