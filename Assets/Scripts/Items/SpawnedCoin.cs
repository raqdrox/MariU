using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Athena.Mario.Items
{
    public class SpawnedCoin : MonoBehaviour, ISpawnableItem
    {
        [SerializeField] Vector3 moveHeight;
        [SerializeField] float moveTime;
        Vector3 startPos;

        //EventStuff



        private bool DoSpawnCycle = false;
        public bool NeedsSpawnCycle => DoSpawnCycle;

        public void OnEndSpawn() { }

        public void OnStartSpawn()
        {
            startPos = transform.position;
            StartCoroutine(CoinAnim());
            //add event stuff
        }

        IEnumerator CoinAnim()
        {
            float currentMovementTime = 0f;
            while (Vector3.Distance(startPos, startPos + moveHeight) >= 0)
            {
                Debug.Log("UP");
                currentMovementTime += Time.deltaTime;
                transform.position = Vector3.Lerp(startPos, startPos + moveHeight, currentMovementTime / moveTime);
                yield return null;
            }
            /*currentMovementTime = 0f;
            while (Vector3.Distance(startPos + moveHeight, startPos ) < 0)
            {
                Debug.Log("Down");
                currentMovementTime += Time.deltaTime;
                transform.position = Vector3.Lerp(startPos + moveHeight, startPos , currentMovementTime/moveTime);
                yield return null;
            }
            Destroy(gameObject);*/
        }
    }
}