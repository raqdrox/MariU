using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Athena.Mario.Items
{
    public interface ISpawnableItem
    {
        public bool NeedsSpawnCycle { get; }

        public void OnStartSpawn();

        public void OnEndSpawn();

    }
}