using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteTimer : MonoBehaviour
{
    public float timer = 3.0f;
    IEnumerator delete()
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }

	void Start ()
    {
        StartCoroutine(delete());
	}

}
