using UnityEngine;
using System.Collections;

/*
 */
public class GameMusic : MonoBehaviour {
	
	//
	public AudioSource arenaMusic = null;
	public AudioSource menuMusic = null;
	
	public float fadeTime = 2.0f;
	
	//
	private float currentFadeTime;
	private bool isFadeIn;
	private bool isFading;
	
	//
	public void FadeInArena() {
		arenaMusic.time = 0.0f;
		arenaMusic.volume = 0.0f;
		
		isFadeIn = true;
		isFading = true;
		currentFadeTime = 0.0f;
	}
	
	public void FadeOutArena() {
		isFadeIn = false;
		isFading = true;
		currentFadeTime = 0.0f;
	}
	
	//
	private void Awake() {
		isFading = false;
		isFadeIn = false;
	}
	
	private void Update() {
		if(!isFading) return;
		
		currentFadeTime += Time.deltaTime;
		
		float progress = Mathf.Clamp01(currentFadeTime / fadeTime);
		
		arenaMusic.volume = (isFadeIn) ? progress : 1.0f - progress;
		menuMusic.volume = (isFadeIn) ? 1.0f - progress : progress;
		
		if(currentFadeTime > fadeTime) isFading = false;
	}
}
