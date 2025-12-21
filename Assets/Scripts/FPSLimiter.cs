using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSLimiter : MonoBehaviour {

	public Text fpsText; // Assign in inspector
	public float updateInterval = 0.5f; // How often to update the display

	private float accum = 0f;
	private int frames = 0;
	private float timeLeft;
	private float fps = 0f;

	void Start()
	{
		Application.targetFrameRate = 30;
		timeLeft = updateInterval;

		// If no text component is assigned, try to get one
		if (fpsText == null)
		{
			fpsText = GetComponent<Text>();

			// Create UI text if none exists
			if (fpsText == null)
			{
				CreateFPSText();
			}
		}
	}

	void Update()
	{
		timeLeft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		frames++;

		if (timeLeft <= 0f)
		{
			fps = accum / frames;
			timeLeft = updateInterval;
			accum = 0f;
			frames = 0;

			if (fpsText != null)
			{
				fpsText.text = string.Format("FPS: {0:F1}", fps);
			}
		}
	}

	void CreateFPSText()
	{
		// Create a Canvas if none exists
		if (FindObjectOfType<Canvas>() == null)
		{
			GameObject canvasGO = new GameObject("Canvas");
			Canvas canvas = canvasGO.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvasGO.AddComponent<CanvasScaler>();
			canvasGO.AddComponent<GraphicRaycaster>();
		}

		// Create FPS Text GameObject
		GameObject textGO = new GameObject("FPS Text");
		textGO.transform.SetParent(FindObjectOfType<Canvas>().transform);

		fpsText = textGO.AddComponent<Text>();

		// Try to get a font - multiple methods
		Font font = GetDefaultFont();
		if (font != null)
		{
			fpsText.font = font;
		}

		fpsText.fontSize = 20;
		fpsText.alignment = TextAnchor.UpperLeft;
		fpsText.color = Color.white;

		// Add outline for better visibility
		Outline outline = textGO.AddComponent<Outline>();
		outline.effectColor = Color.black;

		// Position the text
		RectTransform rectTransform = fpsText.GetComponent<RectTransform>();
		rectTransform.anchorMin = new Vector2(0, 1);
		rectTransform.anchorMax = new Vector2(0, 1);
		rectTransform.pivot = new Vector2(0, 1);
		rectTransform.anchoredPosition = new Vector2(10, -10);
		rectTransform.sizeDelta = new Vector2(200, 30);
	}

	Font GetDefaultFont()
	{
		// Method 1: Try Arial (available on all platforms)
		Font[] allFonts = Resources.FindObjectsOfTypeAll<Font>();
		foreach (Font font in allFonts)
		{
			if (font.name == "Arial")
				return font;
		}

		// Method 2: Try to load from resources
		Font loadedFont = Resources.Load<Font>("Fonts/Arial");
		if (loadedFont != null)
			return loadedFont;

		// Method 3: Use any available font
		if (allFonts.Length > 0)
			return allFonts[0];

		// Method 4: Create a default font using Unity's dynamic font
		// Unity will use the system's default font
		return null; // Let Unity use the default
	}
}
