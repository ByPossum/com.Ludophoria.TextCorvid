using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextCorvid
{
    public abstract class Mover : MonoBehaviour
    {
        public abstract void Begin();
        public abstract GameObject GetStart();
        public abstract GameObject GetEnd();
    }
}