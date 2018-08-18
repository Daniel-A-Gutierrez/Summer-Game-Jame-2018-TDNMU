using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public bool CONTROLLER_ENABLED;
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
	bool isCharging;
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

	public float firePeriod;
	public int damage;
	public int maxChargeDamage;
	public float fireMaxChargeTime;
	public float chargeShotRecoilTime;

	bool fire1;//repeating shot
	bool fire2;//chargeshot
	float continuousFireRemainingCooldown;
	float chargeShotCharge;
	

	//-----------------------------------------------------------------------------------------------


	// Use this for initialization
	void Start ()
	{
		
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		cam = mainCamera.GetComponent<Camera>();
		canMove = true;

		setMoveDirection();
		setShootDirection();
		mousePosition = new Vector3(0,0,0);

		
		warpOnCoolDown = false;
		isCharging = false;
		warpRemainingCoolDown = 0;
		warpCharge = 0;
		warpChargeTime = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		mousePosition = cam.ScreenToWorldPoint(   Input.mousePosition );
		setMoveDirection();
		setShootDirection();
		canMove = !ChargeWarp();
		if(isWarping){canMove = false;}
		if (canMove) // player cant move while charging.
		{
			transform.position +=  moveDirection*speed*(Time.fixedDeltaTime);
			if(fire2)
			{

			}
			
		}
	}

	void LateUpdate()
	{

	}

	void FixedUpdate()
	{
		
	}


	//---------------------------------------------------------------------


	//pressing space charges warp, stopping movement
	//warp continues to charge until the key is released
	//warp max charge increments by maxcharge/maxchargetime*chargeTime then stops incrementing
	//on key release, warp method is called, OnCoolDown is set True and Remaining Cooldown is set to cooldown
	//cooldown decrements until it hits zero, when warpOnCooldown is set to false and it stops decrementing
	bool ChargeWarp() //returns true if player is charging
	{
		if(warpOnCoolDown)
		{
			warpRemainingCoolDown -= Time.deltaTime;
			if (warpRemainingCoolDown < 0)  {warpOnCoolDown = false;}
			return false;
		}
		else
		{
			if(isCharging & Input.GetKeyDown(cancelWarp))
			{
				isCharging = false;
				warpChargeTime = 0;
				warpOnCoolDown = true;
				warpRemainingCoolDown = warpCoolDown;
				warpCharge = 0 ;
				return false;
			}

			if(isCharging & Input.GetKeyUp(warpKey) ) 
			{
				Warp();
				return true; //player cant move their position with warp during the same frame they move normally
			}

			if (Input.GetKey(warpKey) & ! isCharging)
			{
				isCharging = true;
			}

			if(Input.GetKey(warpKey) & isCharging)
			{
				warpCharge +=  Time.deltaTime/warpMaxChargeTime*warpMaxCharge;
				if(warpCharge > warpMaxCharge) {warpCharge = warpMaxCharge;}
				return true;
			}
			return false;
		}
	}

	void Warp()
	{
		Vector3 target = transform.position + moveDirection * (1+warpCharge)*warpBaseDist;
		StartCoroutine(WarpCoroutine(target));
		isCharging = false;
		warpChargeTime = 0;
		warpOnCoolDown = true;
		warpCharge = 0;
		warpRemainingCoolDown = warpCoolDown;
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
		isWarping = false;
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

	void setShootDirection()
	{
		if(CONTROLLER_ENABLED)
		{
			Vector2 shootVector =  new Vector2(DeadZone(Input.GetAxis("ShootX")),
				DeadZone(Input.GetAxisRaw("ShootY"))).normalized;
			shootDirection = shootVector;
			if(shootDirection.magnitude>0){fire1 = true;}
			else {fire1=false;}
		}
		else
		{
			Vector2 shootVector = new Vector2(mousePosition.x-transform.position.x,mousePosition.y -
				 transform.position.y).normalized;
			shootDirection = shootVector;
		}
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
	void ContinuousFire()
	{

	}

	void ChargeAttack()
	{

	}

	void FireChargeAttack()
	{

	}
	

}
