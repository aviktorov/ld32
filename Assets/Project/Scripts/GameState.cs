using UnityEngine;
using System.Collections;

public enum Stage {
	Menu,
	Arena
}

/*
 */
public class GameState : MonoSingleton<GameState> {
	
	//
	public Stage stage = Stage.Arena;
	
	public Transform menuAnchor = null;
	public Transform arenaAnchor = null;
	
	public UICanvasFader uiArenaFader = null;
	public UICanvasFader uiMenuFader = null;
	
	//
	private GameVisuals cachedVisuals;
	
	//
	public UICanvasFader GetStageFader(Stage s) {
		switch(s) {
			case Stage.Menu: return uiMenuFader;
			case Stage.Arena: return uiArenaFader;
		}
		
		return null;
	}
	
	public Transform GetStageAnchor(Stage s) {
		switch(s) {
			case Stage.Menu: return menuAnchor;
			case Stage.Arena: return arenaAnchor;
		}
		
		return null;
	}
	
	//
	private void Awake() {
		cachedVisuals = GetComponent<GameVisuals>();
	}
	
	private void Start() {
		cachedVisuals.SetTransition(stage,stage);
	}
}
