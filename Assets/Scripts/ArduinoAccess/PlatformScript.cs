using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    private float xValue = 0f, yValue = 0f;
    public bool active = false;

    public float floatingSpeed;
    private bool currentlyMoving = false;

    private Vector3 movementVector = Vector3.zero;
    
    private float actionCountdown = 0.1f;
    private float actionCountdownBase = 0.1f;

    public bool hit = false;

    public GameObject playerObject;
    public bool playerFollow = false;

    void Start()
    {
        
    }

    void Update()
    {
        checkDistance();
        if (playerFollow)
        {
            playerObject.transform.parent = this.gameObject.transform;
        }
        else
        {
            playerObject.transform.parent = null;
        }

        if (hit == true)
        {
            gameObject.transform.position += movementVector * -1 * Time.deltaTime;
        }
        if (active)
        {
            readInput();
            if (currentlyMoving) movePlatform();

            actionCountdown -= Time.deltaTime;
        }
        
    }
    
    private void readInput()
    {
        if(actionCountdown <= 0f && !currentlyMoving)
        {

            if(xValue > 0.1f || xValue < -0.1f)
            {
                movementVector = new Vector3(1f, 0f, 0f) * xValue;
                movementVector.Normalize();
                currentlyMoving = true;
            }
            else if(yValue > 0.1f || yValue < -0.1f)
            {
                movementVector = new Vector3(0f, 0f, 1f) * yValue * -1f;
                movementVector.Normalize();
                currentlyMoving = true;
            }


        }
    }
    private void movePlatform()
    {
        currentlyMoving = true;

        gameObject.transform.position += movementVector * floatingSpeed * Time.deltaTime;

        if (playerFollow)
        {
            playerObject.GetComponent<CharacterController>().enabled = false;
            playerObject.transform.localPosition = new Vector3(0f, playerObject.transform.localPosition.y, 0f);
            playerObject.GetComponent<CharacterController>().enabled = true;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Blocker")
        {
            
            currentlyMoving = false;
            actionCountdown = actionCountdownBase;
            hit = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.collider.gameObject.tag == "Blocker")
        {
            hit = false;
        }
    }
    private void checkDistance()
    {
        if ((gameObject.transform.position - playerObject.transform.position).magnitude < 50f) playerFollow = true;
        else playerFollow = false;
    }
    public void assignValues(string pXValue, string pYValue)
    {
        float.TryParse(pXValue, out xValue);
        xValue = (xValue / 512f) - 1f;
        float.TryParse(pYValue, out yValue);
        yValue = (yValue / 512f) - 1f;
    }
}
