using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Athena.Mario.Player;

namespace Athena.Mario.Enemies
{
    public abstract class Enemy :MonoBehaviour
    {
        [SerializeField] protected bool isActive;
        protected Rigidbody2D rb;
        [SerializeField]protected Transform activationPoint;

        protected bool isDead=false;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }
}
