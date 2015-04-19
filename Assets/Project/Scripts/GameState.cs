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
	[Header("Data")]
	public Stage stage = Stage.Arena;
	
	[Header("Anchors")]
	public Transform menuAnchor = null;
	public Transform arenaAnchor = null;
	
	public Transform hero1AnchorMenu = null;
	public Transform hero2AnchorMenu = null;
	public Transform hero1AnchorArena = null;
	public Transform hero2AnchorArena = null;
	
	[Header("UI")]
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
	
	public Transform GetStageHero1Anchor(Stage s) {
		switch(s) {
			case Stage.Menu: return hero1AnchorMenu;
			case Stage.Arena: return hero1AnchorArena;
		}
		
		return null;
	}
	
	public Transform GetStageHero2Anchor(Stage s) {
		switch(s) {
			case Stage.Menu: return hero2AnchorMenu;
			case Stage.Arena: return hero2AnchorArena;
		}
		
		return null;
	}
	
	public void GotoArena() {
		stage = Stage.Arena;
		cachedVisuals.SetTransition(Stage.Menu,Stage.Arena);
		cachedVisuals.SetHeroTransition(Stage.Menu,Stage.Arena); // wait?
		
		// TODO: start entry scene
		// TODO: suck heroes through the pipe to arena
	}
	
	public void GotoMenu() {
		stage = Stage.Menu;
		cachedVisuals.SetTransition(Stage.Arena,Stage.Menu);
		cachedVisuals.SetHeroTransition(Stage.Arena,Stage.Menu); // wait?
		
		// TODO: start death scene
		// TODO: reset hero states
		// TODO: suck heroes through the pipe to menu
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
