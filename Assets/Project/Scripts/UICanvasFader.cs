using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 */
public class UICanvasFader : MonoBehaviour {
	
	//
	public float alpha = 1.0f;
	
	//
	private Graphic[] cachedGraphics;
	private float[] cachedAlphas;
	
	//
	private void Awake() {
		cachedGraphics = GetComponentsInChildren<Graphic>();
	}
	
	private void Start() {
		if(cachedGraphics != null) {
			cachedAlphas = new float[cachedGraphics.Length];
			for(int i = 0; i < cachedGraphics.Length; ++i) {
				cachedAlphas[i] = cachedGraphics[i].color.a;
			}
		}
	}
	
	void Update () {
		if(cachedGraphics == null) return;
		if(cachedAlphas == null) return;
		
		for(int i = 0; i < cachedGraphics.Length; ++i) {
			Graphic cachedGraphic = cachedGraphics[i];
			float cachedAlpha = cachedAlphas[i];
			
			cachedGraphic.color = cachedGraphic.color.WithA(cachedAlpha * alpha);
		}
	}
}
