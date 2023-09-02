using System;
using System.Collections.Generic;
using TLDLoader;
using UnityEngine;
using ChaosMod.Core;
using ChaosMod.Modules;
using Logger = ChaosMod.Modules.Logger;
using Random = System.Random;
using System.Collections;
using System.Linq;

namespace ChaosMod
{
	public class ChaosMod : Mod
	{
		// Mod meta stuff.
		public override string ID => Meta.ID;
		public override string Name => Meta.Name;
		public override string Author => Meta.Author;
		public override string Version => Meta.Version;

		// Initialise modules.
		private readonly Logger logger = new Logger();

		// Mod control.
		private bool enabled = false;
		private KeyCode toggleBind = KeyCode.F2;
		private KeyCode resetBind = KeyCode.F3;

		// GUI.
		private int resolutionX = 0;
		private int resolutionY = 0;
		private Color messageColor = new Color(0, 100, 0);
		private GUIStyle messageStyle = new GUIStyle();
		private string message = string.Empty;
		private float messageMaxTime = 10f;
		private float messageTime = 0f;
		private GUIStyle effectStyle = new GUIStyle()
		{
			fontSize = 20,
			alignment = TextAnchor.UpperRight,
			wordWrap = true,
			normal = new GUIStyleState()
			{
				textColor = Color.white,
			}
		};
		private float baseEffectHistoryY = 60f;
		private float effectHistoryY = 0;
		private GUIStyle timerBackground = null;
		private GUIStyle timerBar = null;
		private GUIStyle timerText = new GUIStyle()
		{
			fontSize = 24,
			alignment = TextAnchor.MiddleCenter,
			normal = new GUIStyleState()
			{
				textColor = Color.black,
			}
		};

		// Effect control.
		private Random random;
		private List<Effect> effects = new List<Effect>();
		private List<Effect> effectHistory = new List<Effect>();
		private List<ActiveEffect> activeEffects = new List<ActiveEffect>();
		private float baseEffectDelay = 30f;
		private float effectDelay = 0f;

		public override void OnLoad()
		{
			resolutionX = mainscript.M.SettingObj.S.IResolutionX;
			resolutionY = mainscript.M.SettingObj.S.IResolutionY;

			// Set styles.
			messageStyle = new GUIStyle() {
				fontSize = 24,
				alignment = TextAnchor.UpperCenter,
				normal = new GUIStyleState()
				{
					textColor = messageColor,
				}
			};

			// Set starting values.
			messageTime = messageMaxTime;
			effectDelay = baseEffectDelay;
			effectHistoryY = baseEffectHistoryY;
			random = new Random();

			RegisterEffect(new Effects.Test());
		}

		/// <summary>
		/// Register an effect.
		/// </summary>
		/// <param name="effect">The effect to register</param>
		public void RegisterEffect(Effect effect)
		{
			effects.Add(effect);
		}

		public override void OnGUI()
		{
			// Set styles.
			if (timerBackground == null && timerBar == null)
			{
				timerBackground = new GUIStyle(GUI.skin.box);
				timerBar = new GUIStyle(GUI.skin.box);
				timerBackground.normal.background = ColorTexture(2, 2, new Color(175, 175, 175));
				timerBar.normal.background = ColorTexture(2, 2, new Color(200, 0, 80));
			}

			// Messaging.
			if (message != string.Empty)
			{
				GUI.Label(new Rect(resolutionX / 2f - resolutionX / 2, 60f, resolutionX, resolutionY - 60f), message, messageStyle);
				messageTime -= Time.deltaTime;
				if (messageTime <= 0)
				{
					message = string.Empty;
					messageTime = messageMaxTime;
				}
			}

			// Main UI rendering.
			if (enabled && !mainscript.M.menu.Menu.activeSelf)
			{
				GUI.Box(new Rect(0, 0, resolutionX, 40f), string.Empty, timerBackground);
				float barFill = effectDelay / baseEffectDelay;
				GUI.Box(new Rect(0, 0, resolutionX - (resolutionX * barFill), 40f), string.Empty, timerBar);
				GUI.Label(new Rect(0, 0, resolutionX, 40f), $"{Mathf.RoundToInt(effectDelay)}s", timerText);

				// Render effect history.
				if (effectHistory.Count != 0)
				{
					effectHistoryY = baseEffectHistoryY;
					GUI.Label(new Rect(resolutionX - 450f, effectHistoryY, 400f, resolutionY - baseEffectHistoryY), "<b>Effect history:</b>", effectStyle);
					effectHistoryY += 25f;
					foreach (Effect effect in effectHistory.Skip(Math.Max(0, effectHistory.Count - 10)))
					{
						GUI.Label(new Rect(resolutionX - 450f, effectHistoryY, 400f, resolutionY - baseEffectHistoryY), effect.Name, effectStyle);
						effectHistoryY += 50f;
					}
				}
			}
		}

		/// <summary>
		/// Create a colored texture.
		/// </summary>
		/// <param name="width">Texture width</param>
		/// <param name="height">Texture height</param>
		/// <param name="color">Texture color</param>
		/// <returns>Texture</returns>
		private Texture2D ColorTexture(int width, int height, Color color)
		{
			Color[] pixels = new Color[width * height];
			for (int i = 0; i < pixels.Length; i++)
			{
				pixels[i] = color;
			}
			Texture2D texture = new Texture2D(width, height);
			texture.SetPixels(pixels);
			texture.Apply();
			return texture;
		}

		public override void Update()
		{
			// Keybinds.
			if (Input.GetKeyDown(toggleBind))
			{
				enabled = !enabled;

				messageStyle.normal.textColor = messageColor;
				message = $"Chaos mod v{Meta.Version} by M- enabled";
				if (!enabled)
				{
					message = $"Chaos mod v{Meta.Version} by M- disabled";
					messageColor = new Color(100, 0, 0);
					messageStyle.normal.textColor = messageColor;
				}
			}

			if (Input.GetKeyDown(resetBind))
			{
				// Disable any active effects.
				foreach (ActiveEffect activeEffect in activeEffects)
				{
					Effect effect = activeEffect.Effect;
					try
					{
						effect.End();
					}
					catch (Exception ex)
					{
						logger.Log($"Failed to end effect {effect.Name} - {ex}", Logger.LogLevel.Error);
					}
				}

				// Reset everything to defaults.
				activeEffects.Clear();
				effectHistory.Clear();
				effectDelay = baseEffectDelay;
				enabled = false;

				message = $"Chaos mod has been reset";
				messageColor = new Color(0, 0, 100);
				messageStyle.normal.textColor = messageColor;
			}

			// Return early if disabled.
			if (!enabled)
				return;

			// Return early if no effects are loaded.
			if (effects.Count == 0)
				return;

			effectDelay -= Time.deltaTime;
			if (effectDelay <= 0)
			{
				// Trigger effect.
				int index = random.Next(effects.Count);
				Effect effect = effects[index];
				effectHistory.Add(effect);

				switch (effect.Type)
				{
					case "instant":
						try
						{
							effect.Trigger();
						}
						catch (Exception ex)
						{
							logger.Log($"Effect {effect.Name} errored during trigger and will be disabled. Error - {ex}", Logger.LogLevel.Error);
							effects.Remove(effect);
							effectHistory.RemoveAt(effectHistory.Count - 1);
						}
						break;
					case "timed":
						break;
					case "repeated":
						break;
				}

				effectDelay = baseEffectDelay;
			}
		}
	}
}
