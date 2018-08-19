using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public bool CONTROLLER_ENABLED;
	public int MAXHP;
	int HP;
	public float speed;


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
	//-----------------------------------------------------------------------------------------------


	// Use this for initialization
	void Start ()
	{
		HP = MAXHP;
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		cam = mainCamera.GetComponent<Camera>();
		canMove = true;

		setMoveDirection();
		setShootDirection();
		mousePosition = new Vector3(0,0,0);

		
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
	
	// Update is called once per frame
	void Update ()
	{
		continuousFireRemainingCooldown -= Time.deltaTime;
		mousePosition = cam.ScreenToWorldPoint(   Input.mousePosition );
		setMoveDirection();
		setShootDirection();

		warpRemainingCoolDown -= Time.deltaTime;
		//override
		if(!isWarping)
		{
			if(isChargingGun)
			{
				if(Input.GetKeyDown(warpKey) && warpRemainingCoolDown <= 0)
				{
					CancelChargingGun();
					StartChargingWarp();
				}
				else
				{
					if(Input.GetKeyUp(chargeshootJoystickButton) | Input.GetKeyUp(chargeshootMouseButton))
					{
						FireChargeAttack();
					}
					else if(Input.GetKey(chargeshootJoystickButton) | Input.GetKey(chargeshootMouseButton))
					{
						KeepChargingAttack();
					}
				}
			}
			else if(isChargingWarp)
			{
				if(Input.GetKeyDown(chargeshootJoystickButton) | Input.GetKeyDown(chargeshootMouseButton) & !recoiling)
				{
					CancelChargingWarp();
					StartChargingShot();
				}
				else
				{
					if(Input.GetKeyUp(warpKey))
					{
						Warp();
					}
					else if(Input.GetKey(warpKey))
					{
						KeepChargingWarp();
					}
				}
					
			}
			else if(Input.GetKeyDown(warpKey) )
			{
				StartChargingWarp();
			}
			else if(Input.GetKeyDown(chargeshootJoystickButton) | Input.GetKeyDown(chargeshootMouseButton) & !recoiling)
			{
				StartChargingShot();
			}
			else
			{
				ContinuousFire();
			}
		}
		if(isWarping|isChargingWarp){canMove = false;}
		else{canMove = true;}
		if (canMove) // player cant move while charging.
		{
			transform.position +=  moveDirection*speed*(Time.fixedDeltaTime);
		}
	}

	void LateUpdate()
	{

	}

	void FixedUpdate()
	{
		
	}

	void TakeDamage()
	{
		if(!invincible) {HP -= 1;}
		if(HP == 0) {Die();}
	}

	void Die()
	{

	}
	IEnumerator invincibility()
	{
		invincible = true;
		float start = Time.time;
		while(Time.time - start < invincibilityTime)
		{
			yield return null;
		}
		invincible = false; 
	}

	//---------------------------------------------------------------------


	//pressing space charges warp, stopping movement
	//warp continues to charge until the key is released
	//warp max charge increments by maxcharge/maxchargetime*chargeTime then stops incrementing
	//on key release, warp method is called, OnCoolDown is set True and Remaining Cooldown is set to cooldown
	//cooldown decrements until it hits zero, when warpOnCooldown is set to false and it stops decrementing

	void StartChargingWarp()
	{
		if(warpRemainingCoolDown <= 0)
		{
			isChargingWarp = true;
			KeepChargingWarp();
		}
	}

	void KeepChargingWarp() //returns true if player is charging
	{
		warpCharge +=  Time.deltaTime/warpMaxChargeTime*warpMaxCharge;
		if(warpCharge > warpMaxCharge) {warpCharge = warpMaxCharge;}
		
	}

	void CancelChargingWarp()
	{
		isChargingWarp = false;
		warpChargeTime = 0;
		warpRemainingCoolDown = warpCoolDown;
		warpCharge = 0 ;
		isWarping = false;
	}

	void Warp()
	{
		Vector3 target = transform.position + moveDirection * (1+warpCharge)*warpBaseDist;
		StartCoroutine(WarpCoroutine(target));
		
	}

	IEnumerator WarpCoroutine(Vector3 target)
	{
		Vector3 startingPosition = transform.position;
		float warpDistance = Vector3.Distance(transform.position, target);
		float start = Time.time;
		isWarping = true;
		while(Vector3.Distance(transform.position, target) > .1f)
		{
			float elapsed = Time.time - start;
			if (elapsed > warpTime) { elapsed = warpTime;}
			float progress = elapsed / warpTime ;
			transform.position = startingPosition*(1-progress) + target*progress;
			yield return null;
		}
		CancelChargingWarp();
	}




	//------------------------------------------------------------------------------------
	/*
	shooting on keyboard : leftclick to shoot, mouse position to aim, right click to charge shot
	shooting on controller : aim to shoot, rb to chargeshot
	you can only shoot while you can move

	charging your warp cancels shooting
	charging your shot, but only the charge shot, cancels warp. 
	cannot shoot while charging warp 
	*/
	void StartChargingShot()
	{
		isChargingGun = true;
		KeepChargingAttack();
	}

	void CancelChargingGun()
	{
		isChargingGun = false;
		chargeShotCharge = 0;
	}

	void KeepChargingAttack()
	{
		if(chargeShotCharge < maxChargeDamage) 
		{
			chargeShotCharge += Time.deltaTime/fireMaxChargeTime*maxChargeDamage;
		}
		else
		{
			chargeShotCharge = maxChargeDamage;
		}

	}


	void FireChargeAttack()
	{
		Transform g;
		GameObject bullet=GameObject.Instantiate(largeProjectile,
		new Vector3(transform.position.x + shootDirection.x*bulletSpawnDistance,transform.position.y + shootDirection.y*bulletSpawnDistance,0),
		Quaternion.identity);
		bullet.transform.up = shootDirection;
		bullet.GetComponent<go>().speed *= 1 + chargeShotCharge;
		StartCoroutine(Recoil());
		CancelChargingGun();
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
		Transform g;
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



//-----------------------------------------------------------------------------------


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
	}


}
