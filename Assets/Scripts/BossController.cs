using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{

    //General Boss Variables
    public int health = 1000;
    public Image healthBar;

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
    [Expandable]
    public AttackPattern[] attackPatterns;
    public ShotRunner runner;
    public bool attackStarted;
    public bool attackOver;

    Rigidbody2D rb2d;
    float cameraSpeed;
    //Initializes movement,attack pattern, and movement cooldown randomly
    void Start()
    {
        StartCoroutine("BeginBattle");
        rb2d = GetComponent<Rigidbody2D>();
        cameraSpeed = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ConsistentMovement>().speed;
        rb2d.velocity = new Vector3(0, rb2d.velocity.y + cameraSpeed, 0);
    }

    //Deals with actual movement and attack patterns while not moving
    void Update()
    {
        healthBar.fillAmount = (float)health / 1000.0f;
        if (movementStarted && transform.position != movementLocation.transform.position)
        {
            //If movement has started but the boss hasn't reached the location, move towards it
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, movementLocation.transform.position, step);
        }
        else if (movementStarted && transform.position == movementLocation.transform.position)
        {
            //If movement has started and the boss has reached the location
            //stop all movement, randomize next movement, randomize attack, begin cooldown
            movementStarted = false;
            RandomizeMovement();
            RandomizeAttack();
        }
        else if (attackStarted)
        {
            Debug.Log("Pew");
        }
    }

    void RandomizeMovementInterval()
    {
        //Randomizes the amount of time to wait for the boss to move again
        movementCooldownTime = Random.Range(movementIntervalMin, movementIntervalMax);
        cooldownOver = false;
    }

    void RandomizeMovement()
    {
        //Randomizes the location the boss will move to from the list of locations provided
        movementPattern = (int)Mathf.RoundToInt(Random.Range(0.0f, (float)(movementLocationList.Count - 1)));
        RandomizeMovementInterval();
    }

    void RandomizeAttack()
    {
        //Randomizes the attack pattern from a list of attack patterns
        attackStarted = true;
        AttackPattern attackPattern = attackPatterns[Random.Range(0, attackPatterns.Length)];
        StartCoroutine(attackPattern.SequenceCoroutine(runner, AttackPatternCallBack));
    }

    public void AttackPatternCallBack()
    {
        attackStarted = false;
        StartCoroutine("moveOnCooldown");
    }

    public IEnumerator BeginBattle()
    {
        //The kickoff coroutine for the Boss
        yield return new WaitForSeconds(3.0f);
        RandomizeMovement();
        StartCoroutine("moveOnCooldown");
    }

    public IEnumerator moveOnCooldown()
    {
        //This coroutine takes the location chosen from RandomMovement and the 
        //cooldown from RandomMovementInterval and makes both happen
        movementLocation = movementLocationList[movementPattern];
        yield return new WaitForSeconds(movementCooldownTime);
        cooldownOver = true;
        movementStarted = true;

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("PlayerBullets"))//10 is player bullets
        {
            takeDamage(col.gameObject.GetComponent<go>().damage);
            Destroy(col.gameObject);
        }
    }

    void takeDamage(int damage)
    {
        health -= damage;
        //THATS ALOTA DAMAGE
    }
}
