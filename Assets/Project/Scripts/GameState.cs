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
	
	public void GotoArena() {
		stage = Stage.Arena;
		cachedVisuals.SetTransition(Stage.Menu,Stage.Arena);
	}
	
	public void GotoMenu() {
		stage = Stage.Menu;
		cachedVisuals.SetTransition(Stage.Arena,Stage.Menu);
	}
	
	//
	private void Awake() {
		cachedVisuals = GetComponent<GameVisuals>();
	}
	
	private void Start() {
		
		if(uiMenuFader) uiMenuFader.alpha = 0.0f;
		if(uiArenaFader) uiArenaFader.alpha = 0.0f;
		
		cachedVisuals.SetTransition(stage,stage);
	}
}
