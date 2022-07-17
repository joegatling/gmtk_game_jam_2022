using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGame
{
    public interface IExplodable
    {
        public void Explode(Bomb bomb);
    }
}