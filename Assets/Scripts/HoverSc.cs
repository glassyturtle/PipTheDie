using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverSc : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Fall();
    }
    private void Fall()
    {
        LeanTween.moveY(gameObject, transform.position.y + 0.2f, 2).setEaseInExpo().setOnComplete(Rise).setEaseInOutExpo();
    }
    private void Rise()
    {
        LeanTween.moveY(gameObject, transform.position.y + -0.2f, 2).setEaseInExpo().setOnComplete(Fall).setEaseInOutExpo();
    }
}
