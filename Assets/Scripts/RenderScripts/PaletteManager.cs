using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Athena.Mario.RenderScripts
{
    public class PaletteManager : MonoBehaviour
    {
        [SerializeField]private Palette _palette;

        public static PaletteManager Instance { get; private set; }
        PaletteManager()
        {
            Instance = this;
        }

        public List<Color> GetPaletteColors(int id)
        {
            return _palette.GetColors(id);
        }
    }
}
