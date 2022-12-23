using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Athena.Mario.Teleport;
using Athena.Mario.Player;
using Athena.Mario.Misc;

namespace Athena.Mario.Tiles
{
    public class Tile_Pipe : TeleportNode
    {
        //TODO: Refactor Player Input Check
        [SerializeField] private float pipeExitDistance=1f;
        [SerializeField] private Direction PipeDirection=Direction.DOWN;
        private bool exiting = false;

        public static readonly Dictionary<Direction, KeyCode> DirectionKeyMap=new Dictionary<Direction, KeyCode>()
        {
            {Direction.TOP,KeyCode.UpArrow},
            {Direction.DOWN,KeyCode.DownArrow},
            {Direction.LEFT,KeyCode.LeftArrow},
            {Direction.RIGHT,KeyCode.RightArrow}
        };
        public static readonly Dictionary<Direction, Vector3> DirectionMap=new Dictionary<Direction, Vector3>()
        {
            {Direction.TOP,Vector3.down},
            {Direction.DOWN,Vector3.up},
            {Direction.LEFT,Vector3.right},
            {Direction.RIGHT,Vector3.left}
        };
        //TODO: Pipe Trigger
        void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player") && IsEnterable && Input.GetKey(DirectionKeyMap[PipeDirection]))
            {
                if(exiting)
                {
                    exiting = false;
                    return;
                }
                Debug.Log("Player Entered Pipe");
                bool entered=EnterNode(other.gameObject);
                Debug.Log("Entered : " + entered);
            }
        }

        //TODO: Pipe Entry

        public override bool EnterNode(GameObject entity)
        {

            PlayerManager player = entity.GetComponent<PlayerManager>();
            player.TogglePlayer(false);
            bool hasTeleported=TeleportHandler.Instance.TeleportPlayerFromNode(this, entity);
            if (!hasTeleported)
            {
                player.TogglePlayer(true);
            }
            return hasTeleported;
        }

        //TODO: Pipe Exit

        public override bool ExitNode(GameObject entity)
        {
            exiting = true;
            PlayerManager player = entity.GetComponent<PlayerManager>();
            player.TogglePlayer(false);
            player.MovePlayerToPosition(transform.position+(pipeExitDistance*DirectionMap[PipeDirection]));
            player.TogglePlayer(true);
            return true;
        }
    void OnValidate(){
        if (PipeDirection == Direction.DOWN)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (PipeDirection == Direction.TOP)
        {
            transform.rotation = Quaternion.Euler(0, 180, 180);
        }
        else if (PipeDirection == Direction.RIGHT)
        {
            transform.rotation = Quaternion.Euler(180, 0, 90);
        }
        else if (PipeDirection == Direction.LEFT)
        {
            transform.rotation = Quaternion.Euler(0, 0, 270);
        }
    }
    
    
    }

    
}
