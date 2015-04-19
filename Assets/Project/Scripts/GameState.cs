using UnityEngine;
using UnityEngine.EventSystems;
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
	public Transform hero1 = null;
	public Transform hero2 = null;
	
	[Header("Anchors")]
	public Transform menuAnchor = null;
	public Transform arenaAnchor = null;
	
	public Transform hero1AnchorMenu = null;
	public Transform hero1AnchorArena = null;
	
	public Transform hero2AnchorMenu = null;
	public Transform hero2AnchorArena = null;
	
	[Header("UI")]
	public UICanvasFader uiArenaFader = null;
	public UICanvasFader uiMenuFader = null;
	public EventSystem eventSystem = null;
	public GameObject playButton = null;
	
	//
	private GameVisuals cachedVisuals;
	
	private HeroStats cachedHero1Stats;
	private HeroInput cachedHero1Input;
	private HeroStats cachedHero2Stats;
	private HeroInput cachedHero2Input;
	
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
		
		cachedHero1Stats.Reset();
		cachedHero1Input.SetGrabbed(false);
		
		cachedHero2Stats.Reset();
		cachedHero2Input.SetGrabbed(false);
		
		eventSystem.SetSelectedGameObject(null);
		// TODO: start entry scene
	}
	
	public void GotoMenu() {
		stage = Stage.Menu;
		cachedVisuals.SetTransition(Stage.Arena,Stage.Menu);
		
		cachedHero1Stats.Reset();
		cachedHero1Input.SetGrabbed(true);
		
		cachedHero2Stats.Reset();
		cachedHero2Input.SetGrabbed(true);
		
		eventSystem.SetSelectedGameObject(playButton);
		// TODO: start death scene
	}
	
	//
	private void Awake() {
		cachedVisuals = GetComponent<GameVisuals>();
		cachedHero1Stats = hero1.GetComponent<HeroStats>();
		cachedHero1Input = hero1.GetComponent<HeroInput>();
		cachedHero2Stats = hero2.GetComponent<HeroStats>();
		cachedHero2Input = hero2.GetComponent<HeroInput>();
	}
	
	private void Start() {
		
		if(uiMenuFader) uiMenuFader.alpha = 0.0f;
		if(uiArenaFader) uiArenaFader.alpha = 0.0f;
		
		cachedVisuals.SetTransition(stage,stage);
		
		bool inMenu = (stage == Stage.Menu);
		cachedHero1Input.SetGrabbed(inMenu);
		cachedHero2Input.SetGrabbed(inMenu);
		
		eventSystem.SetSelectedGameObject((inMenu) ? playButton : null);
	}
}
