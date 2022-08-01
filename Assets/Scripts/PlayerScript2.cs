using UnityEngine;
using System.Collections;

public class PlayerScript2 : MonoBehaviour
{
    [SerializeField] PlayerScript ps;
    [SerializeField] Rigidbody2D body;
    [SerializeField] GameManager gm;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip[] knockClips;
    [SerializeField] AudioClip[] roll;

    [SerializeField] GameObject bounceEffect, iceEffect;
    bool isPlayingSound = false;
    bool isRolling = false;
    private void PlaySound()
    {
        if (!isPlayingSound)
        {
            isPlayingSound = true;
            source.clip = knockClips[Random.Range(0, knockClips.Length)];
            source.Play();
            StartCoroutine(Playsound());
        }
    }
    private  IEnumerator Playsound()
    {
        yield return new WaitForSeconds(1f);
        isPlayingSound = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlaySound();
        if (collision.gameObject.CompareTag("jumpBoost"))
        {
            ps.GainJumpBoost();
        }
        if (collision.gameObject.CompareTag("sticky"))
        {
            ps.BecomeSticky();
        }
        if (collision.gameObject.CompareTag("bounceLeft"))
        {
            ps.BounceLeft();
        }
        if (collision.gameObject.CompareTag("bounceUp"))
        {
            ps.BounceUp();
        }
        if (collision.gameObject.CompareTag("bounce"))
        {
            Instantiate(bounceEffect, transform.position, transform.rotation);
        }
        if (collision.gameObject.CompareTag("icy"))
        {
            Instantiate(iceEffect, transform);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("pip"))
        {
            gm.CollectPip();
            ps.CollectPip();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("bounceLeft"))
        {
            ps.BounceLeft();
        }
        if (collision.gameObject.CompareTag("death"))
        {
            gm.ResetLevel();
        }
        if (collision.gameObject.CompareTag("cheese"))
        {
            Instantiate(bounceEffect, transform);
            gm.CollectCheese();
        }
        if (collision.gameObject.CompareTag("mouse") && gm.hasCheese)
        {
            Instantiate(bounceEffect, transform);
            gm.OpenGateWay();
        }
        if (collision.gameObject.CompareTag("Win"))
        {
            gm.WinGame();
        }
    }
}
