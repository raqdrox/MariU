using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Athena.Mario.Entities;
namespace Athena
{
    public abstract class Entity : MonoBehaviour
    {
        public abstract EntityIdentifierEnum entityIdentifier{get;} 
    }
}
