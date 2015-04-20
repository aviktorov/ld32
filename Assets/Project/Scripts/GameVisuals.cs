using UnityEngine;
using System.Collections;

/*
 */
public class GameVisuals : MonoBehaviour {
	
	//
	public float transitionTime = 3.0f;
	public float transitionHeroTime = 1.0f;
	public float kamiSmoothness = 5.0f;
	
	public AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);
	
	//
	private GameState cachedState;
	private Transform cachedCameraTransform;
	private Stage stageFrom;
	private Stage stageTo;
	
	private float currentTime;
	private float currentHeroTime;
	private bool kamiToggled;
	
	//
	public void ToggleKami() {
		kamiToggled = !kamiToggled;
	}
	
	public void SetTransition(Stage from,Stage to) {
		stageFrom = from;
		stageTo = to;
		currentTime = 0.0f;
		currentHeroTime = 0.0f;
	}
	
	//
	private void Awake() {
		cachedState = GetComponent<GameState>();
		
		stageFrom = Stage.Menu;
		stageTo = Stage.Menu;
		currentTime = 0.0f;
		currentHeroTime = 0.0f;
	}
	
	private void Start() {
		cachedCameraTransform = Camera.main.GetComponent<Transform>();
		kamiToggled = false;
		cachedState.uiKamiFader.alpha = 0.0f;
	}
	
	private void Update() {
		
		// animate time
		currentTime = Mathf.Min(currentTime + Time.deltaTime,transitionTime);
		float progress = transitionCurve.Evaluate(Mathf.Clamp01(currentTime / transitionTime));
		
		currentHeroTime = Mathf.Min(currentHeroTime + Time.deltaTime,transitionHeroTime);
		float progressHero = transitionCurve.Evaluate(Mathf.Clamp01(currentHeroTime / transitionHeroTime));
		
		// animate kami
		cachedState.uiKamiFader.alpha = Mathf.Lerp(cachedState.uiKamiFader.alpha,(kamiToggled) ? 1.0f : 0.0f,kamiSmoothness * Time.deltaTime);
		
		// animate heroes
		if(Mathf.Abs(currentHeroTime - transitionHeroTime) > Mathf.Epsilon) {
			Transform hero1From = cachedState.GetStageHero1Anchor(stageFrom);
			Transform hero1To = cachedState.GetStageHero1Anchor(stageTo);
			
			Transform hero2From = cachedState.GetStageHero2Anchor(stageFrom);
			Transform hero2To = cachedState.GetStageHero2Anchor(stageTo);
			
			if(hero1From && hero1To) {
				cachedState.hero1.position = Vector3.Lerp(hero1From.position,hero1To.position,progressHero);
			}
			
			if(hero2From && hero2To) {
				cachedState.hero2.position = Vector3.Lerp(hero2From.position,hero2To.position,progressHero);
			}
		}
		
		if(Mathf.Abs(currentTime - transitionTime) > Mathf.Epsilon) {
			
			// animate ui
			UICanvasFader uiFrom = cachedState.GetStageFader(stageFrom);
			UICanvasFader uiTo = cachedState.GetStageFader(stageTo);
			
			if(uiFrom) uiFrom.alpha = 1.0f - progress;
			if(uiTo) uiTo.alpha = progress;
			
			// animate camera
			Transform anchorFrom = cachedState.GetStageAnchor(stageFrom);
			Transform anchorTo = cachedState.GetStageAnchor(stageTo);
			
			if(anchorFrom && anchorTo) {
				cachedCameraTransform.position = Vector3.Lerp(anchorFrom.position,anchorTo.position,progress);
			}
		}
	}
}
