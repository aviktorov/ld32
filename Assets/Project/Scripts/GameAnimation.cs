using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/*
 */
public class GameAnimation : MonoBehaviour {
	
	//
	public AnimationClip gameStart = null;
	public AnimationClip gameEnd = null;
	
	public UnityEvent onGameStart = null;
	public UnityEvent onGameEnd = null;
	
	//
	private Animation cachedAnimation;
	
	//
	public void PlayGameStart() { StartCoroutine(Play(gameStart,onGameStart)); }
	public void PlayGameEnd() { StartCoroutine(Play(gameEnd,onGameEnd)); }
	
	//
	private void Awake() {
		cachedAnimation = GetComponent<Animation>();
	}
	
	private IEnumerator Play(AnimationClip clip,UnityEvent callback) {
		clip.legacy = true;
		cachedAnimation.Play(clip.name);
		while(cachedAnimation.isPlaying) yield return new WaitForSeconds(0.5f);
		
		Debug.Log("The end of " + clip.name);
		callback.Invoke();
	}
}
