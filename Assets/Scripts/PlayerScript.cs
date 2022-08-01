using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] int speed, pullRange;
    [SerializeField] Rigidbody2D playerBody;
    [SerializeField] SpriteRenderer dragSR, dotSR;
    [SerializeField] GameObject jumpParticle, boosteffect, slimeEffect;

    [SerializeField] GameManager gm;
    public SpriteRenderer sr;
    GameObject currentEffect = null, currentSLimeyGameobjecty = null;
    private bool isDraging = false, jumpBoost = false, onIce = false, isStuck = false, canBecomeSticky = true;

    private float noMovementThreshold = 0.0001f;
    private const int noMovementFrames = 3;
    Vector3[] previousLocations = new Vector3[noMovementFrames];
    private bool isMoving;
    public Sprite[] spriteForEachSide;


    public void BecomeSticky()
    {
        if (!canBecomeSticky) return;
        canBecomeSticky = false;
        playerBody.velocity = Vector2.zero;
        playerBody.angularVelocity = 0;
        playerBody.isKinematic = true;

        for (int i = 0; i < previousLocations.Length; i++)
        {
            previousLocations[i] = playerBody.transform.position;
        }
        currentSLimeyGameobjecty = Instantiate(slimeEffect, playerBody.transform);
        isStuck = true;
    }
    void Awake()
    {
        //For good measure, set the previous locations
        for (int i = 0; i < previousLocations.Length; i++)
        {
            previousLocations[i] = Vector3.zero;
        }
    }
    public void GainJumpBoost()
    {
        if (currentEffect) Destroy(currentEffect);
        currentEffect = Instantiate(jumpParticle, playerBody.transform);
        currentEffect.transform.position += new Vector3(0, 0, -1);
        jumpBoost = true;
    }
    public void GoOnIce()
    {
        onIce = true;
        playerBody.sharedMaterial.friction = 0;
    }
    public void BounceLeft()
    {
        playerBody.AddForce(new Vector2(-1,0) * 600);
        currentEffect = Instantiate(boosteffect, playerBody.transform.position, transform.rotation);
    }
    public void BounceUp()
    {
        playerBody.AddForce(new Vector2(0, 1) * 700);
        currentEffect = Instantiate(boosteffect, playerBody.transform.position, transform.rotation);
    }
    public void CollectPip()
    {
        int side = (gm.pipsCollected % 6);
        sr.sprite = spriteForEachSide[side];
    }
    IEnumerator WaitTillStick()
    {
        yield return new WaitForSeconds(0.1f);
        canBecomeSticky = true;
    }

    private void Update()
    {
        if (!isDraging)
        {
            transform.position = new Vector3(playerBody.position.x, playerBody.position.y, transform.position.z);
        }


        //Store the newest vector at the end of the list of vectors
        for (int i = 0; i < previousLocations.Length - 1; i++)
        {
            previousLocations[i] = previousLocations[i + 1];
        }
        previousLocations[previousLocations.Length - 1] = playerBody.transform.position;

        //Check the distances between the points in your previous locations
        //If for the past several updates, there are no movements smaller than the threshold,
        //you can most likely assume that the object is not moving
        for (int i = 0; i < previousLocations.Length - 1; i++)
        {
            if (Vector3.Distance(previousLocations[i], previousLocations[i + 1]) >= noMovementThreshold)
            {
                //The minimum movement has been detected between frames
                isMoving = true;

                if (isStuck)
                {
                    Destroy(currentSLimeyGameobjecty);
                    isStuck = false;
                    playerBody.isKinematic = false;
                }

                break;
            }
            else
            {
                isMoving = false;
                if(onIce) playerBody.sharedMaterial.friction = 0.4f;

            }
        }
    }
    private void OnMouseDown()
    {
        if (isMoving) return;
        dragSR.enabled = true;
        dotSR.enabled = true;
        isDraging = true;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

    }
    private void OnMouseUp()
    {
        dotSR.size = new Vector2(0, 0);
        dragSR.enabled = false;
        dotSR.enabled = false;

        isDraging = false;

        if (isMoving) return;



        Vector2 direction = playerBody.position -  new Vector2(transform.position.x, transform.position.y);
        float distance = (playerBody.position - new Vector2(transform.position.x, transform.position.y)).sqrMagnitude;

        direction.Normalize();

        distance = Mathf.Clamp(distance, 0, pullRange);
        if(jumpBoost)
        {
            jumpBoost = false;
            distance = distance * 1.5f;
            Destroy(currentEffect);
        }
        if (isStuck)
        {
            StartCoroutine(WaitTillStick());
            playerBody.isKinematic = false;
        }

        playerBody.AddForce(direction * speed * distance);

    }

    private void OnMouseDrag()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float distance = Vector2.Distance(mousePosition, playerBody.transform.position);
        Vector2 direction = mousePosition - playerBody.transform.position;
        direction.Normalize();
        if (distance > pullRange)
        {
            mousePosition = playerBody.transform.position + (new Vector3(direction.x, direction.y) * pullRange);
        }


        transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);


        Vector3 targ = playerBody.transform.position;

        Vector3 objectPos = transform.position;
        targ.x = targ.x - objectPos.x;
        targ.y = targ.y - objectPos.y;

        float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
        dotSR.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));


        distance = Vector2.Distance(mousePosition, playerBody.transform.position);
        dotSR.size = new Vector2(distance, 1);

    }
}
