using System;
using System.Collections.Generic;
using TLDLoader;
using UnityEngine;
using ChaosMod.Core;
using ChaosMod.Modules;
using ChaosMod.Extensions;
using Logger = ChaosMod.Modules.Logger;
using Random = System.Random;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

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
		private List<Effect> effects = new List<Effect>();
		private List<EffectHistory> effectHistory = new List<EffectHistory>();
		private List<ActiveEffect> activeEffects = new List<ActiveEffect>();
		private float baseEffectDelay = 30f;
		private float currentEffectDelay = 0f;
		private float effectDelay = 0f;

		// Meta effects.
		private bool noChaos = false;
		private bool hideUI = false;

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
			currentEffectDelay = baseEffectDelay;
			effectDelay = currentEffectDelay;
			effectHistoryY = baseEffectHistoryY;

			// Register meta effects.
			RegisterEffect(new Effects.Meta.MetaNoChaos());
			RegisterEffect(new Effects.Meta.Meta2xTimer());
			RegisterEffect(new Effects.Meta.Meta5xTimer());
			RegisterEffect(new Effects.Meta.MetaHideUI());

			// Register core effects.
			// Player effects.
			RegisterEffect(new Effects.Player.EffectFreeze());
			RegisterEffect(new Effects.Player.EffectBhop());
			RegisterEffect(new Effects.Player.EffectSuperFOV());
			RegisterEffect(new Effects.Player.EffectWobblyHead());
			RegisterEffect(new Effects.Player.EffectBlind(resolutionX, resolutionY));

			// Vehicle effects.
			RegisterEffect(new Effects.Vehicle.EffectExitVehicle());
			RegisterEffect(new Effects.Vehicle.EffectEmptyFuel());
			RegisterEffect(new Effects.Vehicle.EffectPopTires());
			RegisterEffect(new Effects.Vehicle.EffectRandomiseCondition());
			RegisterEffect(new Effects.Vehicle.EffectRandomiseColor());
			RegisterEffect(new Effects.Vehicle.EffectSpammyDoors());
			RegisterEffect(new Effects.Vehicle.EffectToggleHandbrake());
			RegisterEffect(new Effects.Vehicle.EffectLights());
			RegisterEffect(new Effects.Vehicle.EffectSpammyLights());
			RegisterEffect(new Effects.Vehicle.EffectToggleIgnition());
			RegisterEffect(new Effects.Vehicle.EffectDropParts());
			RegisterEffect(new Effects.Vehicle.EffectEssentials());

			// World effects.
			RegisterEffect(new Effects.World.EffectLowGravity());
			RegisterEffect(new Effects.World.EffectNegativeGravity());
			RegisterEffect(new Effects.World.EffectRandomiseTime());
			RegisterEffect(new Effects.World.EffectRainingShit());
			RegisterEffect(new Effects.World.EffectSpawnRandomVehicle());
			RegisterEffect(new Effects.World.EffectMunkasInvasion());
			RegisterEffect(new Effects.World.EffectRabbits());
			RegisterEffect(new Effects.World.EffectUFOs());
			RegisterEffect(new Effects.World.EffectSandstorms());
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
			// Call GUI for any active effects with GUI enabled.
			// This is called first so it's first in the GUI stack.
			// This stops effects from drawing over the Chaos Mod UI.
			if (!mainscript.M.menu.Menu.activeSelf)
			{
				foreach (ActiveEffect effect in activeEffects.Where(e => e.Effect.UseGUI))
				{
					effect.Effect.OnGUI();
				}
			}

			// Set styles.
			if (timerBackground == null && timerBar == null)
			{
				timerBackground = new GUIStyle(GUI.skin.box);
				timerBar = new GUIStyle(GUI.skin.box);
				timerBackground.normal.background = Modules.Utilities.GUI.ColorTexture(2, 2, new Color(175, 175, 175));
				timerBar.normal.background = Modules.Utilities.GUI.ColorTexture(2, 2, new Color(200, 0, 80));
			}

			// Messaging.
			if (message != string.Empty)
			{
				GUIExtensions.DrawOutline(new Rect(resolutionX / 2f - resolutionX / 2, 60f, resolutionX, resolutionY - 60f), message, messageStyle, Color.black);
				messageTime -= Time.deltaTime;
				if (messageTime <= 0)
				{
					message = string.Empty;
					messageTime = messageMaxTime;
				}
			}

			// Main UI rendering.
			if (enabled && !mainscript.M.menu.Menu.activeSelf && !hideUI)
			{
				if (!noChaos)
				{
					GUI.Box(new Rect(0, 0, resolutionX, 30f), string.Empty, timerBackground);
					float barFill = effectDelay / currentEffectDelay;
					GUI.Box(new Rect(0, 0, resolutionX - (resolutionX * barFill), 30f), string.Empty, timerBar);
					GUI.Label(new Rect(0, 0, resolutionX, 30f), $"{Mathf.RoundToInt(effectDelay)}s", timerText);
				}

				// Render effect history.
				if (effectHistory.Count != 0)
				{
					effectHistoryY = baseEffectHistoryY;
					GUIExtensions.DrawOutline(new Rect(resolutionX - 450f, effectHistoryY, 400f, resolutionY - baseEffectHistoryY), "<b>Effect history:</b>", effectStyle, Color.black);
					effectHistoryY += 25f;
					foreach (EffectHistory history in effectHistory.Skip(Math.Max(0, effectHistory.Count - 10)).Reverse())
					{
						Effect effect = history.Effect;
						string effectLabel = effect.Name;
						if (history.ActiveEffect != null)
							effectLabel += $" - {Mathf.RoundToInt(history.ActiveEffect.Remaining)}s";
						GUIExtensions.DrawOutline(new Rect(resolutionX - 450f, effectHistoryY, 400f, resolutionY - baseEffectHistoryY), effectLabel, effectStyle, Color.black);
						effectHistoryY += 50f;
					}
				}
			}
			else if (hideUI)
			{
				effectHistoryY = baseEffectHistoryY;
				GUIExtensions.DrawOutline(new Rect(resolutionX - 450f, effectHistoryY, 400f, resolutionY - baseEffectHistoryY), "<b>Effect history:</b>", effectStyle, Color.black);
				effectHistoryY += 25f;
				GUIExtensions.DrawOutline(new Rect(resolutionX - 450f, effectHistoryY, 400f, resolutionY - baseEffectHistoryY), effectHistory.Where(e => e.Effect.GetType().Name == "MetaHideUI").FirstOrDefault().Effect.Name, effectStyle, Color.black);
			}
		}

		public override void Update()
		{
			// Keybinds.
			if (Input.GetKeyDown(toggleBind))
			{
				enabled = !enabled;

				messageStyle.normal.textColor = new Color(0, 100, 0);
				message = $"Chaos mod v{Meta.Version} by M- enabled";
				message += $"\nLoaded {effects.Count} effects";
				if (!enabled)
				{
					DisableActiveEffects();
					effectDelay = currentEffectDelay;
					message = $"Chaos mod v{Meta.Version} by M- disabled";
					messageStyle.normal.textColor = new Color(100, 0, 0);
				}
			}

			if (Input.GetKeyDown(resetBind))
			{
				DisableActiveEffects();

				// Reset everything to defaults.
				activeEffects.Clear();
				effectHistory.Clear();
				currentEffectDelay = baseEffectDelay;
				effectDelay = currentEffectDelay;
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

			// Trigger active effects.
			List<ActiveEffect> removeQueue = new List<ActiveEffect>();
			foreach (ActiveEffect active in activeEffects)
			{
				active.Remaining -= Time.deltaTime;
				bool expired = false;

				if (active.Remaining <= 0)
				{
					removeQueue.Add(active);
					active.Effect.End();
					expired = true;

					if (active.Effect.Type == "meta")
					{
						switch (active.Effect.GetType().Name)
						{
							case "MetaNoChaos":
								noChaos = false;
								break;
							case "Meta2xTimer":
								currentEffectDelay = baseEffectDelay;
								break;
							case "Meta5xTimer":
								currentEffectDelay = baseEffectDelay;
								break;
							case "MetaHideUI":
								hideUI = false;
								break;
						}
					}
				}

				if (active.Effect.Type == "repeated" && !expired)
				{
					active.TriggerRemaining -= Time.deltaTime;
					if (active.TriggerRemaining <= 0)
					{
						active.Effect.Trigger();
						active.TriggerRemaining = active.Effect.Frequency;
					}
				}
			}

			// Clear any expired active effects.
			foreach (ActiveEffect active in removeQueue)
			{
				activeEffects.Remove(active);
			}
			removeQueue.Clear();

			// Return early if no chaos is active.
			if (noChaos)
				return;

			effectDelay -= Time.deltaTime;
			if (effectDelay <= 0)
			{
				// Trigger effect.
				int index = UnityEngine.Random.Range(0, effects.Count);
				Effect effect = effects[index];
				bool addToHistory = true;

				ActiveEffect activeEffect = null;

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
							addToHistory = false;
						}
						break;
					case "timed":
						try
						{
							if (effect.Length > 0)
							{
								activeEffect = new ActiveEffect()
								{
									Effect = effect,
									Remaining = effect.Length,
								};
								activeEffects.Add(activeEffect);
								effect.Trigger();
							}
							else
							{
								logger.Log($"Timed effect {effect.Name} has no Length so will be disabled.", Logger.LogLevel.Error);
								effects.Remove(effect);
								addToHistory = false;
							}
						}
						catch (Exception ex)
						{
							logger.Log($"Effect {effect.Name} errored during trigger and will be disabled. Error - {ex}", Logger.LogLevel.Error);
						}
						break;
					case "repeated":
						try
						{
							if (effect.Length > 0 && effect.Frequency >= 0)
							{
								activeEffect = new ActiveEffect()
								{
									Effect = effect,
									Remaining = effect.Length,
									TriggerRemaining = effect.Frequency,
								};
								activeEffects.Add(activeEffect);
								effect.Trigger();
							}
							else
							{
								logger.Log($"Repeated effect {effect.Name} has no Length/Frequency so will be disabled.", Logger.LogLevel.Error);
								effects.Remove(effect);
								addToHistory = false;
							}
						}
						catch (Exception ex)
						{
							logger.Log($"Effect {effect.Name} errored during trigger and will be disabled. Error - {ex}", Logger.LogLevel.Error);
						}
						break;
					case "meta":
						switch (effect.GetType().Name)
						{
							case "MetaNoChaos":
								noChaos = true;
								// Disable any existing effects.
								DisableActiveEffects();
								break;
							case "Meta2xTimer":
								currentEffectDelay = baseEffectDelay / 2;
								break;
							case "Meta5xTimer":
								currentEffectDelay = baseEffectDelay / 5;
								break;
							case "MetaHideUI":
								hideUI = true;
								break;
						};
						activeEffect = new ActiveEffect()
						{
							Effect = effect,
							Remaining = effect.Length,
						};
						activeEffects.Add(activeEffect);

						break;
				}

				if (addToHistory)
					effectHistory.Add(new EffectHistory()
					{
						Effect = effect,
						ActiveEffect = activeEffect,
					});

				effectDelay = currentEffectDelay;
			}
		}

		/// <summary>
		/// Disables any currently active effects.
		/// </summary>
		private void DisableActiveEffects()
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
		}
	}
}
