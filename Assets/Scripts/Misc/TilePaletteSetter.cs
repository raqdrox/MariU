using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Athena.Mario.RenderScripts
{
    public enum LevelType
    {
        Overworld,
        Underground,
        Castle,
        Night,
        Underwater,
    }

    [Serializable]
    public class PaletteInformation
    {
        public LevelType levelType;
        public int paletteIndex;
    }
    public class TilePaletteSetter : PaletteSetter
    {

        
        [SerializeField]private LevelType _levelType;
        [SerializeField]private List<PaletteInformation> paletteInformation;
        public void SetPalette(LevelType levelType)
        {
            _levelType = levelType;
            var paletteIdx = paletteInformation.FirstOrDefault(x => x.levelType == levelType).paletteIndex;
            SetPalette(paletteIdx);

        }

        void OnValidate()
        {
            SetPalette(_levelType);
        }
    }
}
