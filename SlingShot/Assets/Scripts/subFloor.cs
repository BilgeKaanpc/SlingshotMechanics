using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class subFloor : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("obstacle"))
        {
            StartCoroutine(DestroyGameObject(other));
        }
    }
    IEnumerator DestroyGameObject(Collider other)
    {
        gameManager.PointUpdate();
        yield return new WaitForSeconds(1f);
        Destroy(other.gameObject);
    }
}
