using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Athena.Mario.Items
{
    public class SpawnedCoin : MonoBehaviour, ISpawnableItem
    {
        [SerializeField] int pointVal = 100;
        [SerializeField] Vector3 moveHeight;
        [SerializeField] Vector3 destroyHeight;
        [SerializeField] float moveTime;
        Vector3 startPos;

        //EventStuff



        private bool DoSpawnCycle = false;
        public bool NeedsSpawnCycle => DoSpawnCycle;

        public void OnEndSpawn() { }

        public void OnStartSpawn()
        {
            startPos = transform.position;
            GetComponent<Animator>().SetTrigger("coin");
            //StartCoroutine(CoinAnim());
            //add event stuff
        }

        public void OnAnimEnd()
        {
            //show text here
            Destroy(gameObject);
        }

        IEnumerator CoinAnim()
        {
            float currentMovementTime = 0f;
            while (Vector3.Distance(transform.position, startPos + moveHeight) > 0)
            {
                Debug.Log("UP");
                currentMovementTime += Time.deltaTime;
                transform.position = Vector3.Lerp(startPos, startPos + moveHeight, currentMovementTime / moveTime);
                yield return null;
            }
            currentMovementTime = 0f;
            while (Vector3.Distance(startPos+destroyHeight,transform.position) > 0)
            {
                Debug.Log("Down");
                currentMovementTime += Time.deltaTime;
                transform.position = Vector3.Lerp(startPos + moveHeight, startPos+ destroyHeight, (currentMovementTime / moveTime)* (moveHeight.y/destroyHeight.y));
                yield return null;
            }

            //Show Points gained

            Destroy(gameObject);
        }
    }
}