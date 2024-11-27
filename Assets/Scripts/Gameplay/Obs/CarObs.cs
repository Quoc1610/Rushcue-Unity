using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarObs : MonoBehaviour
{
    public GameObject posMove;
    public GameObject goCar;
    public float speed = 10f;

    private Coroutine moveCoroutine;

    public void StartMoving()
    {
  
        moveCoroutine = StartCoroutine(MoveCarCoroutine());
    }

    private IEnumerator MoveCarCoroutine()
    {
        while (Vector3.Distance(goCar.transform.position, posMove.transform.position) > 0.1f)
        {
            Vector3 direction = (posMove.transform.position - goCar.transform.position).normalized;

            goCar.transform.position = Vector3.MoveTowards(
                goCar.transform.position,
                posMove.transform.position,
                speed * Time.deltaTime
            );

            yield return null;
        }
        moveCoroutine = null;
    }
}
