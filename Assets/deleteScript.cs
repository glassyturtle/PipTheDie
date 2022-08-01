using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteScript : MonoBehaviour
{
    [SerializeField] int secondsTillDelete;

    private void Start()
    {
        StartCoroutine(Delete());
    }

    IEnumerator Delete()
    {
        yield return new WaitForSeconds(secondsTillDelete);
        Destroy(gameObject);
    }
}
