using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float speed;
	//pressing space charges warp, stopping movement
	//warp continues to charge until the key is released
	//warp max charge increments by maxcharge/maxchargetime*chargeTime then stops incrementing
	//on key release, warp method is called, OnCoolDown is set True and Remaining Cooldown is set to cooldown
	//cooldown decrements until it hits zero, when warpOnCooldown is set to false and it stops decrementing
	public float warpBaseDist;
	public float warpMaxChargeTime;
	public float warpMaxCharge;
	public float warpCoolDown;
	public KeyCode warpKey;
	public KeyCode cancelWarp;

	bool warpOnCoolDown;
	bool isCharging;
	float warpRemainingCoolDown;
	float warpCharge;
	float warpChargeTime;

	float xInput;
	float yInput;
	// Use this for initialization
	void Start ()
	{
		xInput = Input.GetAxisRaw("Horizontal");
		yInput = Input.GetAxisRaw("Vertical");
		warpOnCoolDown = false;
		isCharging = false;
		warpRemainingCoolDown = 0;
		warpCharge = 0;
		warpChargeTime = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		xInput = Input.GetAxisRaw("Horizontal");
		yInput = Input.GetAxisRaw("Vertical");
	}

	void FixedUpdate()
	{

		transform.position +=  new Vector3(xInput,yInput,0).normalized*speed*Time.deltaTime;
	}

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
		Vector3 direction = new Vector3(xInput,yInput,0).normalized;
		transform.position += newVect

		isCharging = false;
		warpChargeTime = 0;
		warpOnCoolDown = true;
		warpCharge = 0;
		warpRemainingCoolDown = warpCoolDown;
	}
}
