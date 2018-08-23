using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;
using System.Linq;
using System;
public class PlayerController2 : MonoBehaviour
{
	public bool CONTROLLER_ENABLED;
	public int MAXHP;
	int HP;
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
	public float speed;
    public GameObject gameOver;


	public float warpBaseDist;
	public float warpMaxChargeTime;
	public float warpMaxCharge;
	public float warpCoolDown;
	public float warpTime;
	public float joystickDeadZone;
	public KeyCode warpKey;
	public KeyCode cancelWarp;


	bool warpOnCoolDown;
	bool isChargingWarp;
	float warpRemainingCoolDown;
	float warpCharge;
	float warpChargeTime;
	bool isWarping;



	bool canMove;
	Vector3 moveDirection;
	Vector2 shootDirection;
	Vector3 mousePosition;
	GameObject mainCamera;
	Camera cam;

	public GameObject smallProjectile;
	public GameObject largeProjectile;
	public float bulletSpawnDistance;
	public float fireCooldown;
	public int damage;
	public int maxChargeDamage;
	public float fireMaxChargeTime;
	public float chargeShotRecoilTime;
	public KeyCode chargeshootJoystickButton;
	public KeyCode chargeshootMouseButton;
	public KeyCode shootMouseButton;
	public float recoilImpact;
	public float maxRecoilMultiplier;

	bool isChargingGun ;
	bool fire1;//repeating shot
	bool fire2;//chargeshot
	float continuousFireRemainingCooldown;
	float chargeShotCharge;
	bool recoiling;
	bool cancelledPreviousFrame;

	public float invincibilityTime;

	bool invincible = false;
	bool intangible = false;

	AudioManager AM ;


	/*perhaps there is a list of classes, and one is denoted as active
	this objects select methods are called from within it. to signal to switch classes
	it returns what class should be run on the next frame. */


	/* as an example project, i want it to start in state 1, and if i press D, go to state 2
	either if i let D go or if 3 seconds pass.  */

	float time1;
	float duration;
	string nextState;
	Dictionary<string,Action> states;

	void Start()
	{
		nextState = "StartState";
		states = new Dictionary<string,Action>();
		states["StartState"] =StartState;
		states["WarpingState"] = WarpingState;
		states ["DeadState"] = DeadState;
		states ["EnterChargingShotState"] = EnterChargingShotState;
		states ["ChargingShotState"] = ChargingShotState ;
		
		AM =GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
		HP = MAXHP;
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		cam = mainCamera.GetComponent<Camera>();
		canMove = true;

		setMoveDirection();
		setShootDirection();
		mousePosition = new Vector3(0,0,0);

		intangible = false;
		invincible = false;
		
		warpOnCoolDown = false;
		isChargingWarp = false;
		warpRemainingCoolDown = 0;
		warpCharge = 0;
		warpChargeTime = 0;

		recoiling = false;
		chargeShotCharge = 0;
		continuousFireRemainingCooldown = 0;
		fire1= false;
		fire2 = false;
		isChargingGun = false;
		
	}

	float Cooldown(float timer)
	{
		timer -= Time.deltaTime;
		if(timer<0)
		{
			timer = 0;
		}
		return timer;
	}

	void Move()
	{
		if (GetComponent<UnityArmatureComponent>().animation.isPlaying == false)
            {GetComponent<UnityArmatureComponent>().animation.Play("Forward_4FPS",0);}  
        transform.position +=  moveDirection*speed*(Time.fixedDeltaTime);
	}

	void Update()
	{
		setMoveDirection();
		setShootDirection();
		warpRemainingCoolDown = Cooldown(warpRemainingCoolDown);
		continuousFireRemainingCooldown = Cooldown(continuousFireRemainingCooldown);
		fireCooldown = Cooldown(fireCooldown);
		states[nextState]();

	}

	
/*------------------------------------------------------------------ */
// to Warping and Enter Charging Shot 
	void StartState()
	{
		nextState  = "StartState";
		Move();
		ContinuousFire();
		if(Input.GetKeyDown(warpKey) & warpCoolDown == 0)
		{
			WarpingState();
		}
		if(Input.GetKeyDown(chargeshootMouseButton)&fireCooldown == 0)
		{
			EnterChargingShotState();
		}
	}

// only to Charging Shot, only from Start
	void EnterChargingShotState()
	{
		nextState = "ChargingShotState";
		AM.Play("Charging2");
        GetComponent<UnityArmatureComponent>().animation.GotoAndStopByFrame("Charge", 9);
        ChargingShotState();

	}

// from Enter Charging Shot, to Start or Warping
	void ChargingShotState()
	{
		nextState  = "ChargingShotState";
		Move();
		if(Input.GetKeyDown(warpKey) )
		{
			ExitChargingShot("WarpingState");
		}
		else if(Input.GetKeyUp(chargeshootMouseButton))
		{
			FireChargeAttack();
			nextState = "StartState";//avoid moving twice in one frame
		}
		else
		{
			chargeShotCharge += Time.deltaTime/fireMaxChargeTime*maxChargeDamage;
			if(chargeShotCharge > maxChargeDamage)
			{
				chargeShotCharge = maxChargeDamage;
			}
		}
	}
// To any, from Charging Shot State
	void ExitChargingShot(string toState)
	{
		AM.Stop("Charging2");
		chargeShotCharge = 0;
		nextState = toState;
	}

// From Starting State or Charging Shot State, holds for a while then back to starting state
	void WarpingState()
	{
		nextState = "WarpingState";
		AnimationConfig stuff;
		stuff = GetComponent<UnityArmatureComponent>().animation.animationConfig;
		GetComponent<UnityArmatureComponent>().animation.Play("Dash", 1);

		Warp();//next state is in coroutine 
		
	}

// frin any, to none. 
	void DeadState()
	{
		invincible = true;
	}
//
/*------------------------------------------------------------------- */
	void FireChargeAttack()
	{
		AM.Stop("Charging2");
		AM.Play("FireChargeShot");
        GetComponent<UnityArmatureComponent>().animation.GotoAndPlayByFrame("Charge",9,1);

        UnityEngine.Transform g;
		GameObject bullet=GameObject.Instantiate(largeProjectile,
		new Vector3(transform.position.x + shootDirection.x*bulletSpawnDistance,transform.position.y + shootDirection.y*bulletSpawnDistance,0),
		Quaternion.identity);
		bullet.transform.up = shootDirection;
		bullet.GetComponent<go>().speed *= 1 + chargeShotCharge/4;
		bullet.GetComponent<go>().damage = (int)chargeShotCharge;
		StartCoroutine(Recoil());
		isChargingGun = false;
		chargeShotCharge = 0;
		
		//change the damage too
		
	}

	void ContinuousFire()
	{
		if(continuousFireRemainingCooldown <=0 & Input.GetKey(shootMouseButton))
		{
			FireSmall();
			continuousFireRemainingCooldown = fireCooldown;
		}
		
	}
	
	void FireSmall()
	{
		AM.Play("ContinuousFireShot");
		UnityEngine.Transform g;
		GameObject bullet=GameObject.Instantiate(smallProjectile,
		new Vector3(transform.position.x + shootDirection.x*bulletSpawnDistance,transform.position.y + shootDirection.y*bulletSpawnDistance,0),
		Quaternion.identity);
		bullet.transform.up = shootDirection;

	}


	IEnumerator Recoil()
	{
		float start = Time.time;
		recoiling = true;
		float chargeMulti = chargeShotCharge/maxChargeDamage * maxRecoilMultiplier;
		//Vector3 targetPos = transform.position - new Vector3(shootDirection.x,shootDirection.y,0);
		while(Time.time-start < chargeShotRecoilTime)
		{
			float d = 1f - (Time.time-start)/chargeShotRecoilTime;
			transform.position -= new Vector3(shootDirection.x,shootDirection.y,0)*d*Time.deltaTime*recoilImpact*chargeMulti;
			yield return null;
		}
		recoiling = false;
	}



	/* --------------------------------------------------------------- */
	void StartChargingWarp()
    {
        if (warpRemainingCoolDown <= 0)
		{
            AnimationConfig stuff;
            stuff = GetComponent<UnityArmatureComponent>().animation.animationConfig;

             GetComponent<UnityArmatureComponent>().animation.Play("Dash", 1);

            //isChargingWarp = true;
			Warp();
		}
	}


	void EndWarp()
	{
		AM.Stop("EndChargeWarp");
        GetComponent<UnityArmatureComponent>().animation.GotoAndPlayByFrame("Dash",12,1);
		isChargingWarp = false;
		warpChargeTime = 0;
		warpRemainingCoolDown = warpCoolDown;
		warpCharge = 0 ;
		isWarping = false;
   }

	void Warp()
    {
		//AM.Stop("StartChargingWarp");
		AM.Play("WarpSound");
        Vector3 target = transform.position + moveDirection * (1+warpCharge)*warpBaseDist;
		RaycastHit2D hit ;
		hit = Physics2D.Raycast(new Vector2(transform.position.x + moveDirection.x, transform.position.y + moveDirection.y), moveDirection,warpBaseDist*(1+warpCharge) );
		if(hit)
		{
			if(   LayerMask.LayerToName(hit.transform.gameObject.layer) != "EnemyBullets")
			{
				target = hit.point - new Vector2(moveDirection.x,moveDirection.y);
			}
		}
		StartCoroutine(WarpCoroutine(target));
       
    }

	IEnumerator WarpCoroutine(Vector3 target)
	{
		Vector3 startingPosition = transform.position;
		float warpDistance = Vector3.Distance(transform.position, target);
		float start = Time.time;
		intangible = true;
		while(Vector3.Distance(transform.position, target) > .1f)
		{
			if(Input.GetKeyUp(warpKey))
			{
				break;
			}
			float elapsed = Time.time - start;
			if (elapsed > warpTime) { elapsed = warpTime;}
			float progress = elapsed / warpTime ;
			transform.position = startingPosition*(1-progress) + target*progress;
			yield return null;
		}
		intangible = false;
		EndWarp();
		StartState();
		nextState = "StartState";
	}
/*--------------------------------------------------------- */



	void setMoveDirection()
	{
        
		Vector2 moveVector =  new Vector2(DeadZone(Input.GetAxis("Horizontal")),
			DeadZone(Input.GetAxisRaw("Vertical"))).normalized;
		moveDirection= moveVector;
   
   
    }

	void setShootDirection() //also checks (per frame) whether shooting should be done or not
	{
		if(CONTROLLER_ENABLED)
		{
			Vector2 shootVector =  new Vector2(DeadZone(Input.GetAxis("ShootX")),
				DeadZone(Input.GetAxisRaw("ShootY"))).normalized;
			shootDirection = shootVector;
			
		}
		else
		{
			Vector2 shootVector = new Vector2(mousePosition.x-transform.position.x,mousePosition.y -
				 transform.position.y).normalized;
			shootDirection = shootVector;
			
		}
        var angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


    }

	float DeadZone(float axis)
	{
		if(Mathf.Abs(axis)<joystickDeadZone)
		{
			return 0;
		}
		else
		{
			return axis;
		}
	}

	void OnTriggerEnter2D(Collider2D col)//bullets should be triggers
	{
		if( !intangible & !invincible )
	 	{
	 		string layerName = LayerMask.LayerToName(col.gameObject.layer);
			// print(layerName);	 
	 		if (layerName == "EnemyBullets")
	 		{
	 			TakeDamage();
                col.gameObject.SetActive(false);
//	 			Destroy(col.gameObject);
	 			return;
	 		}		
		 }
	}
	void OnCollisionEnter2D(Collision2D col)
	{
	 	if( !intangible)
	 	{
	 		string layerName = LayerMask.LayerToName(col.gameObject.layer);
	 		if(layerName == "Enemy" | layerName == "EnemyIndestructibles")
	 		{
	 			TakeDamage();
	 		}
	 	}
	 }

	void TakeDamage()
	{
		
		if(!invincible) {HP -= 1;AM.Play("PlayerTakeDamage");StartCoroutine("invincibility");}
		if(HP == 0) {Die();}
	}

	void Die()
	{
        AM.Stop("Loop 2");
		AM.Play("Game Over Track");
		nextState = "DeadState";
        gameOver.SetActive(true);
	}

	IEnumerator invincibility()
	{
        if(HP == 2)
        {
            heart3.SetActive(false);
        }
        else if(HP == 1)
        {
            heart2.SetActive(false);
        }
        else if(HP == 0)
        {
            heart1.SetActive(false);
        }
		invincible = true;
        GetComponent<UnityArmatureComponent>().animation.Play("Hit", 0);
        float start = Time.time;
		float blinkTimer = invincibilityTime/7;
		float blinkCooldown = 0;

		
		while(Time.time - start < invincibilityTime)
		{

//            Debug.Log("Counting");

              
    
				blinkCooldown -= Time.deltaTime;
			
			yield return null;
		}

		invincible = false;
        GetComponent<UnityArmatureComponent>().animation.Play("Forward_4FPS", 0);
    }


	/*
	class State
	{
		
		public List<Action> GetMethods()
		{
			List<Action> functions = new List<Action>();
			functions.Add(Do1);
			functions.Add(Do2);
			functions.Add(Do3);
			return functions;
		}
		public void Do1()
		{
			print("Do1");
		}
		public void Do2()
		{
			print("Do2");
		}
		public void Do3()
		{
			print("Do3");
		}
	}
	
	class State2:State
	{
		new public List<Action> GetMethods()
		{
			List<Action> functions = new List<Action>();
			functions.Add(Do1);
			functions.Add(Do3);
			return functions;
		}
	}*/





}
