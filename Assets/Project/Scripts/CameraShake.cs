using UnityEngine;
using System.Collections;

/*
 */
public class CameraShake : MonoBehaviour {
	
	//
	public float duration = 0.2f;
	public float amplitude = 0.2f;
	
	//
	private Transform cachedTransform;
	private Vector3 cachedPosition;
	private bool isShaking;
	private float currentTime;
	
	//
	public void Shake() {
		if(!isShaking) cachedPosition = cachedTransform.position;
		
		isShaking = true;
		currentTime = 0.0f;
	}
	
	// 
	private void Awake() {
		cachedTransform = GetComponent<Transform>();
		
		isShaking = false;
		currentTime = 0.0f;
	}
	
	private void LateUpdate() {
		if(!isShaking) return;
		
		currentTime += Time.deltaTime;
		
		float angle = Random.Range(0.0f,Mathf.PI * 2.0f);
		float radius = Random.Range(0.0f,amplitude);
		
		Vector3 offset = new Vector3(Mathf.Sin(angle),Mathf.Cos(angle),0.0f) * radius;
		cachedTransform.position = cachedPosition + offset;
		
		if(currentTime > duration) {
			cachedTransform.position = cachedPosition;
			isShaking = false;
		}
	}
}
