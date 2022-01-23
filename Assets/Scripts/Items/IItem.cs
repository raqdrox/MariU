using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Athena.Mario.Items
{
    public interface IItem
    {
        public Sprite ItemSprite { get; }

        public void OnSpawn();
    }
}