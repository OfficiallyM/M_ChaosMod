using ChaosMod.Core;
using ChaosMod.Effects.Vehicle;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using TLDLoader;
using UnityEngine;
using static ChaosMod.Modules.Keybinds;
using Settings = ChaosMod.Core.Settings;

namespace ChaosMod.Modules
{
	internal class Config
	{
		private Settings settings = new Settings();

		// GUI variables.
		private bool configActive = false;
		private int resolutionX;
		private int resolutionY;
		private float menuX;
		private float menuY;
		private float menuWidth;
		private float menuHeight;
		private Vector2 configScrollPosition = Vector2.zero;
		private GUIStyle labelStyle = new GUIStyle();

		// Configuration variables.
		private string configPath = string.Empty;
		private Configuration config = new Configuration();

		private List<Effect> effects = new List<Effect>();
		private List<Effect> enabledEffects = new List<Effect>();
		private float timerLength = 30f;

		/// <summary>
		/// Initialise config module.
		/// </summary>
		/// <param name="path">Config file path</param>
		/// <param name="_effects">Full effects list</param>
		/// <param name="_effects">Enabled effects list</param>
		public void Initialise(string path, List<Effect> _effects, List<Effect> _enabledEffects)
		{
			try
			{
				configPath = path;
				effects = _effects;
				enabledEffects = _enabledEffects;

				// Load existing config.
				config = GetConfig();

				// Check all effects.
				List<string> all = EffectsToConfig(effects);
				List<string> newEffects = new List<string>();
				if (config.allEffects == null || config.allEffects.Count == 0)
					config.allEffects = all;
				else
					newEffects = all.Where(e => !config.allEffects.Any(e1 => e1 == e)).ToList();

				foreach (string newEffect in newEffects)
					config.allEffects.Add(newEffect);

				// Check enabled effects.
				List<string> enabled = EffectsToConfig(enabledEffects);
				// No current value, set to default.
				if (config.enabledEffects == null || config.enabledEffects.Count == 0)
					config.enabledEffects = enabled;

				// Check for new effects which should be enabled by default.
				List<string> newEnabledEffects = enabled.Where(e => newEffects.Contains(e)).ToList();
				if (newEnabledEffects.Count > 0)
				{
					foreach (string newEffect in newEnabledEffects)
					{
						config.enabledEffects.Add(newEffect);
					}
				}

				// Set default timer length.
				if (config.timerLength == null)
					config.timerLength = settings.timerLength;

				// Commit new configuration.
				CommitConfiguration();

				// Load defaults.
				if (config.timerLength != null)
					timerLength = config.timerLength.Value;

				// Reload enabled effects.
				enabledEffects = ConfigToEffects(config.enabledEffects);

				// GUI stuff.
				resolutionX = mainscript.M.SettingObj.S.IResolutionX;
				resolutionY = mainscript.M.SettingObj.S.IResolutionY;

				menuX = 10f;
				menuY = 10f;
				menuWidth = resolutionX * 0.25f;
				menuHeight = resolutionY - 20f;

				// Set label styling.
				labelStyle.alignment = TextAnchor.UpperLeft;
				labelStyle.normal.textColor = Color.white;

				// Set settings.
				UpdateSettings();
			}
			catch (Exception ex)
			{
				Logger.Log($"Config initialisation failed - {ex}", Logger.LogLevel.Critical);
			}
		}

		internal void OnGUI()
		{
			if (configActive)
			{
				RenderConfigGUI();
			}
		}

		/// <summary>
		/// Render the config GUI.
		/// </summary>
		private void RenderConfigGUI()
		{
			GUI.Box(new Rect(menuX, menuY, menuWidth, menuHeight), $"<color=#f87ffa><size=18><b>{Meta.Name} - Configuration</b></size>\n<size=16>v{Meta.Version} - made with ❤️ by {Meta.Author}</size></color>");

			float fieldX = menuX + 10f;
			float fieldY = 60f;
			float fieldWidth = 200f;
			float fieldHeight = 20f;
			float x = fieldX;
			float y = fieldY;

			float effectLabelWidth = menuWidth - 250f;

			int configs = 2;

			float scrollHeight = (configs * (fieldHeight + 10f)) + (effects.Count * (fieldHeight + 10f));

			configScrollPosition = GUI.BeginScrollView(new Rect(x, y, menuWidth - 20f, menuHeight - 80f), configScrollPosition, new Rect(x, y, menuWidth - 20f, scrollHeight), new GUIStyle(), GUI.skin.verticalScrollbar);

			// Timer length.
			GUI.Label(new Rect(x, y, fieldWidth, fieldHeight), "Timer length (seconds):", labelStyle);
			x += fieldWidth + 10f;
			float currentTimerLength = timerLength;
			if (float.TryParse(GUI.TextField(new Rect(x, y, 100f, fieldHeight), timerLength.ToString(), labelStyle), out timerLength))
			{
				if (currentTimerLength != timerLength)
					UpdateConfig(new Configuration() { timerLength = timerLength });
			}
			x += 110f;

			if (GUI.Button(new Rect(x, y, 100f, fieldHeight), "Reset"))
			{
				timerLength = 30f;
				UpdateConfig(new Configuration() { timerLength = timerLength });
			}

			x = fieldX;
			y += fieldHeight + 10f;

			if (GUI.Button(new Rect(x, y, 100, fieldHeight), "Enable all"))
			{
				enabledEffects = effects.ToList();
				UpdateConfig(new Configuration() { enabledEffects = EffectsToConfig(enabledEffects) });
			}

			x += 110f;

			if (GUI.Button(new Rect(x, y, 100, fieldHeight), "<color=#F00>Disable all</color>"))
			{
				enabledEffects = new List<Effect>();
				UpdateConfig(new Configuration() { enabledEffects = EffectsToConfig(enabledEffects) });
			}

			x += 245f;

			GUI.Label(new Rect(x, y, fieldWidth, fieldHeight), $"{enabledEffects.Count}/{effects.Count} enabled", labelStyle);

			x = fieldX;
			y += fieldHeight + 10f;

			// Effects.
			foreach (Effect effect in effects)
			{
				GUI.Label(new Rect(x, y, effectLabelWidth, fieldHeight), effect.Name, labelStyle);
				x += effectLabelWidth + 10f;

				bool active = false;
				if (enabledEffects.Contains(effect))
					active = true;
				if (GUI.Button(new Rect(x, y, fieldWidth, fieldHeight), $"{(active ? "Activated" : "<color=#F00>Deactivated</color>")}"))
				{
					if (active)
						enabledEffects.Remove(effect);
					else
						enabledEffects.Add(effect);

					UpdateConfig(new Configuration() { enabledEffects = EffectsToConfig(enabledEffects) });
				}

				x = fieldX;
				y += fieldHeight + 10f;
			}

			GUI.EndScrollView();
		}

		/// <summary>
		/// Toggle config display.
		/// </summary>
		internal void ToggleShowConfig(bool? force = null)
		{
			configActive = !configActive;

			if (force != null)
				configActive = force.Value;

			mainscript.M.crsrLocked = !configActive;
			mainscript.M.SetCursorVisible(configActive);
			mainscript.M.menu.gameObject.SetActive(!configActive);
		}

		/// <summary>
		/// Get if config is currently displayed.
		/// </summary>
		/// <returns>True if yes, otherwise false</returns>
		internal bool IsConfigActive()
		{
			return configActive;
		}

		/// <summary>
		/// Convert effects list to class names for easier storage.
		/// </summary>
		/// <param name="_effects">Effects list</param>
		/// <returns>List of class names</returns>
		private List<string> EffectsToConfig(List<Effect> _effects)
		{
			List<string> effectClasses = new List<string>();
			foreach (Effect effect in _effects)
			{
				effectClasses.Add(effect.GetType().Name);
			}

			return effectClasses;
		}

		/// <summary>
		/// Convert config effects to class references.
		/// </summary>
		/// <param name="configEffects"></param>
		/// <returns></returns>
		internal List<Effect> ConfigToEffects(List<string> configEffects)
		{
			List<Effect> enabled = new List<Effect>();

			foreach (string effectName in configEffects)
			{
				enabled.Add(effects.Where(e => e.GetType().Name == effectName).First());
			}

			return enabled;
		}

		private void UpdateSettings()
		{
			settings.timerLength = config.timerLength.Value;
			settings.currentEffectDelay = config.timerLength.Value;
			settings.effectDelay = config.timerLength.Value;
			settings.enabledEffects = ConfigToEffects(config.enabledEffects);
		}

		/// <summary>
		/// Update configuration and commit changes.
		/// </summary>
		/// <param name="_config">New configuration</param>
		internal void UpdateConfig(Configuration _config)
		{
			// Check for changes.

			// Keybinds.
			if (_config.keybinds != null)
			{
				config.keybinds = _config.keybinds;
			}

			// Enabled effects.
			if (_config.enabledEffects != null)
			{
				config.enabledEffects = _config.enabledEffects;
			}

			// Timer length.
			if (_config.timerLength != null)
			{
				config.timerLength = _config.timerLength;
			}

			UpdateSettings();
			CommitConfiguration();
		}

		/// <summary>
		/// Get the current configuration.
		/// </summary>
		/// <returns>Current configuration</returns>
		internal Configuration GetConfig()
		{
			FetchConfiguration();

			return config;
		}

		/// <summary>
		/// Get configuration from file.
		/// </summary>
		private void FetchConfiguration()
		{
			// Attempt to load the config file.
			try
			{
				// Config already loaded, return early.
				if (config == new Configuration()) return;

				if (File.Exists(configPath))
				{
					string json = File.ReadAllText(configPath);
					MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
					DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Configuration));
					config = jsonSerializer.ReadObject(ms) as Configuration;
					ms.Close();
				}
			}
			catch (Exception ex)
			{
				Logger.Log($"Error loading config file: {ex}", Logger.LogLevel.Error);
			}
		}

		/// <summary>
		/// Write configuration to file.
		/// </summary>
		private void CommitConfiguration()
		{
			if (configPath == String.Empty)
			{
				Logger.Log("Config path not found", Logger.LogLevel.Error);
				return;
			}

			try
			{
				MemoryStream ms = new MemoryStream();
				DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Configuration));
				jsonSerializer.WriteObject(ms, config);
				using (FileStream file = new FileStream(configPath, FileMode.Create, FileAccess.Write))
				{
					ms.WriteTo(file);
					ms.Dispose();
				}

			}
			catch (Exception ex)
			{
				Logger.Log($"Config write error: {ex}", Logger.LogLevel.Error);
			}
		}
	}
}
