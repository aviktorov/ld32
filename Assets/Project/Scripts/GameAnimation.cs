using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/*
 */
public class GameAnimation : MonoBehaviour {
	
	//
	public UnityEvent onGameStart = null;
	public UnityEvent onGameEnd = null;
	
	//
	public void PlayGameStart() { onGameStart.Invoke(); }
	public void PlayGameEnd() { onGameEnd.Invoke(); }
}
