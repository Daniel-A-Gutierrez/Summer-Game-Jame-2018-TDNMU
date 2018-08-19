using DragonBones;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour {

    public GameObject boss;
  public void ChangeAnimation(string name)
    {
        boss.GetComponent<UnityArmatureComponent>().animationName = name;
        boss.GetComponent<UnityArmatureComponent>().animation.Play();
    }
}
