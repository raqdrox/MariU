using System;
using System.Collections;
using System.Collections.Generic;
using Athena.Mario.Misc;
using UnityEngine;
using UnityEngine.UI;

namespace Athena.Mario.Tiles
{
    
    public class PipeTailHandler : MonoBehaviour
    {
        [SerializeField] private GameObject tailPf;
        [SerializeField] private GameObject parent;
        [SerializeField] private float offset=0;
        [SerializeField] private int count=0;
        //private GameObject[] tails;
        [SerializeField] private Direction direction;
        public static readonly Dictionary<Direction, Vector3> DirectionMap=new Dictionary<Direction, Vector3>()
        {
            {Direction.TOP,Vector3.up},
            {Direction.DOWN,Vector3.down},
            {Direction.LEFT,Vector3.left},
            {Direction.RIGHT,Vector3.right}
        };

        private void Awake()
        {
            SetTails();
        }

        private void SetTails()
        {
            if(parent==null||tailPf==null)
                return;
            /*if (tails!=null &&tails.Length != 0)
            {
                foreach (var tail in tails)
                {
                    DestroyImmediate(tail);
                }
            }
            tails = new GameObject[count];*/
            for (var i = 0; i < count; i++)
            {
                var tail = Instantiate(tailPf,parent.transform);
                tail.transform.position += DirectionMap[direction]*offset*i;
                tail.transform.rotation = parent.transform.rotation;
                //tails[i] = tail;
            }
        }
    }
}
