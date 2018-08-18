using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {

    //General Boss Stuff
    public int health = 100;

    //Boss Movement
    public int movementPattern;
    private bool movementStarted;
    private float movementCooldownTime;
    private bool cooldownOver;
    public float movementIntervalMin;
    public float movementIntervalMax;
    public float speed;

    //Boss Movement Patterns
    public List<GameObject> movementLocationList;
    private GameObject movementLocation;

    //Boss Attacks
    public int attackPattern;
    public bool attackStarted;
    public bool attackOver;

	//Initializes movement,attack pattern, and movement cooldown randomly
	void Start () {
        StartCoroutine("BeginBattle");
	}
	
    //Deals with actual movement and attack patterns while not moving
	void Update () {    
        if(movementStarted && transform.position != movementLocation.transform.position)
        {
            //if movement has started but the boss hasn't reached the location, move towards it
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, movementLocation.transform.position, step);
        }
        else if(movementStarted && transform.position == movementLocation.transform.position)
        {
            //if movement has started and the boss has reached the location
            //stop all movement, randomize next movement, randomize attack, begin cooldown
            movementStarted = false;
            RandomizeMovement();
            RandomizeAttack();
        }
        else if(attackStarted)
        {
            Debug.Log("Pew");
        }
	}

    void RandomizeMovementInterval()
    {
        movementCooldownTime = Random.Range(movementIntervalMin, movementIntervalMax);
        cooldownOver = false;
    }

    void RandomizeMovement()
    {
        movementPattern = (int)Mathf.RoundToInt(Random.Range(0.0f, (float)(movementLocationList.Count-1)));
        RandomizeMovementInterval();
    }

    void RandomizeAttack()
    {
        Debug.Log("Blah");
        attackStarted = true;
        StartCoroutine("TempAttackPattern");
    }

    public IEnumerator BeginBattle()
    {
        yield return new WaitForSeconds(3.0f);
        RandomizeMovement();
        StartCoroutine("moveOnCooldown");
    }

    public IEnumerator moveOnCooldown()
    {
        movementLocation = movementLocationList[movementPattern];
        yield return new WaitForSeconds(movementCooldownTime);
        cooldownOver = true;
        movementStarted = true;
        
    }

    public IEnumerator TempAttackPattern()
    {
        yield return new WaitForSeconds(2.0f);
        attackStarted = false;
        StartCoroutine("moveOnCooldown");
    }
}
