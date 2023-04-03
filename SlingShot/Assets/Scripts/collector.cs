using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collector : MonoBehaviour
{
    [SerializeField] public List<GameObject> ObjectList;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            setKinematic();
        }
    }
    void setKinematic()
    {
        foreach (var item in ObjectList)
        {
            if (item != null)
            {
                item.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }
}
