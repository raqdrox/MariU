using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;
using System;
 
namespace Athena.Mario.RenderScripts
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "ColorPalette", menuName = "Athena/ColorPalette", order = 0)]
    public class ColorPalette : ScriptableObject{
        public List<Color> colors;
        
        public void AddColor(Color color)
        {
            colors.Add(color);
            
        }

        // Remove a color from the palette
        public void RemoveColor(Color color)
        {
            colors.Remove(color);
        }

        public Color GetColor(int index)
        {
            return colors[index];
        }

        
    }
    
}