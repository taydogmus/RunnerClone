using Dreamteck.Splines;
using UnityEngine;

namespace Taydogmus
{
    public class Level : MonoBehaviour
    {
        [SerializeField] SplineComputer computer;
        
        public SplineComputer Computer => computer;
    }
}