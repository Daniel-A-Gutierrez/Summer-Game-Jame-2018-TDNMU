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
	public float warpMaxChargeMultiplier;
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
	}

	class StateManager()
	{

	}


	void Start()
	{
		State s = new State();
		List<Action> j = s.GetMethods();
		State v = new State2();
		List<Action> o = v.GetMethods();

		foreach(var method in o)
		{
			method();
		}
		
	}

	
	void Update()
	{

	}

}
