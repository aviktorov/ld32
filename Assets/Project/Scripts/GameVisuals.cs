using UnityEngine;
using System.Collections;

/*
 */
public class GameVisuals : MonoBehaviour {
	
	//
	public float transitionTime = 3.0f;
	public AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);
	
	//
	private GameState cachedState;
	private Transform cachedCameraTransform;
	private Stage stageFrom;
	private Stage stageTo;
	
	private float currentTime;
	
	//
	public void SetTransition(Stage from,Stage to) {
		stageFrom = from;
		stageTo = to;
		currentTime = 0.0f;
		
		// set ui
		UICanvasFader uiFrom = cachedState.GetStageFader(stageFrom);
		UICanvasFader uiTo = cachedState.GetStageFader(stageTo);
		
		if(uiFrom) uiFrom.alpha = 1.0f;
		if(uiTo) uiTo.alpha = 0.0f;
		
		// set camera
		Transform anchorFrom = cachedState.GetStageAnchor(stageFrom);
		cachedCameraTransform.position = anchorFrom.position;
	}
	
	//
	private void Awake() {
		cachedState = GetComponent<GameState>();
	}
	
	private void Start() {
		stageFrom = Stage.Menu;
		stageTo = Stage.Menu;
		currentTime = 0.0f;
		cachedCameraTransform = Camera.main.GetComponent<Transform>();
	}
	
	private void LateUpdate() {
		
		// animate time
		currentTime = Mathf.Min(currentTime + Time.deltaTime,transitionTime);
		float k = transitionCurve.Evaluate(Mathf.Clamp01(currentTime / transitionTime));
		
		// animate ui
		UICanvasFader uiFrom = cachedState.GetStageFader(stageFrom);
		UICanvasFader uiTo = cachedState.GetStageFader(stageTo);
		
		if(uiFrom) uiFrom.alpha = 1.0f - k;
		if(uiTo) uiTo.alpha = k;
		
		// animate camera
		Transform anchorFrom = cachedState.GetStageAnchor(stageFrom);
		Transform anchorTo = cachedState.GetStageAnchor(stageTo);
		
		cachedCameraTransform.position = Vector3.Lerp(anchorFrom.position,anchorTo.position,k);
	}
}
