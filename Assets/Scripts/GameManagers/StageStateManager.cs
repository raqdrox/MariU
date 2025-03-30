using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Athena.Mario.GameManagers.Stage
{
    public enum StageType{
        OVERWORLD,
        UNDERGROUND,
        CASTLE,
        WATER,
    }
    public class StageStateManager : MonoBehaviour
    {
        //maintain current level state

        [SerializeField] private StageType stageType;
        public StageType StageType { get => stageType; set => stageType = value; }

    }
}
