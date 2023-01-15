using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;
using System;

namespace Athena.Mario.RenderScripts
{

    [System.Serializable]
    public class KVPair<K,V>{
        public K key;
        public V value;
    }
    


    [System.Serializable]
    [CreateAssetMenu(fileName = "Palette", menuName = "Athena/Palette", order = 1)]
    public class Palette : ScriptableObject
    {
        
        public ColorPalette colors;
        public List<string> palettes;

        

        public List<Color> GetColors(int palIdx)
        {
            if (palIdx >= palettes.Count)
            {
                return null;
            }
            var pal = palettes[palIdx];
            if (pal == "")
            {
                return null;
            }
            List<string> colorIds = pal.Trim().Split(' ').ToList();
            List<Color> palColors = new List<Color>();
            for (int i = 0; i < colorIds.Count; i++)
            {
                int id;
                var alpha = 1f;
                if (colorIds[i].EndsWith("X"))
                {
                    alpha = 0f;
                    colorIds[i] = colorIds[i].Substring(0, colorIds[i].Length - 1);
                }
                id = Convert.ToInt32(colorIds[i], 16);
                var col=this.colors.GetColor(id);
                col.a = alpha;
                palColors.Add(col);
            }
            if(palColors.Count<4)
            {
                for (int i = palColors.Count; i < 4; i++)
                {
                    palColors.Add(Color.black);
                }

            }

            return palColors;
        }
        

        
    }
}
