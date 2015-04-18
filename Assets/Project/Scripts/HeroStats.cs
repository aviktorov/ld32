using UnityEngine;
using System.Collections;

/*
 */
public class HeroStats : MonoBehaviour {
	
	//
	public int health = 100;
	public int stamina = 100;
	
	public void AddDamage(float damage) {
		health -= Mathf.CeilToInt(damage);
		Debug.Log(string.Format("Ouch! Health: {0}",health));
	}
}
