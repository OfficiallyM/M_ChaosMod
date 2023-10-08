using ChaosMod.Core;
using ChaosMod.Effects.Vehicle;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Modules
{
	internal class Keybinds
	{
		// Modules.
		private Config config;

		private GUIStyle labelStyle = new GUIStyle();

		private int rebindAction = -1;
		private readonly Array keyCodes = Enum.GetValues(typeof(KeyCode));

		private Dictionary<string, Vector2> scrollPositions = new Dictionary<string, Vector2>();

		public enum Inputs
		{
			toggle,
			reset,
			config,
			modifier
		}

		public List<Key> keys = new List<Key>();

		[DataContract]
		public class Key
		{
			[DataMember] public KeyCode key = KeyCode.None;
			[DataMember] public bool useModifier = false;
			[DataMember] public int action;
			[DataMember] public string name;
			[DataMember] public KeyCode defaultKey = KeyCode.None;

			public void Unset()
			{
				key = KeyCode.None;
			}

			public void Set(KeyCode _key)
			{
				Unset();
				key = _key;
			}

			public void ToggleUseModifier()
			{
				useModifier = !useModifier;
			}

			public void Reset()
			{
				key = defaultKey;
			}
		}

		public Keybinds(Config _config)
		{
			config = _config;

			try
			{
				// Load defaults.
				int maxInputs = (int)Enum.GetValues(typeof(Inputs)).Cast<Inputs>().Max();
				for (int i = 0; i <= maxInputs; i++)
				{
					keys.Add(new Key() { action = i, useModifier = false });
				}

				// Toggle.
				keys[0].key = KeyCode.F2;
				keys[0].defaultKey = KeyCode.F2;
				keys[0].name = "Toggle on/off";

				// Reset.
				keys[1].key = KeyCode.F3;
				keys[1].defaultKey = KeyCode.F3;
				keys[1].name = "Reset";

				// Config.
				keys[2].key = KeyCode.F2;
				keys[2].useModifier = true;
				keys[2].defaultKey = KeyCode.F2;
				keys[2].name = "Open configuration";

				// Modifier.
				keys[3].key = KeyCode.LeftShift;
				keys[3].defaultKey = KeyCode.LeftShift;
				keys[3].name = "Modifier";
			}
			catch (Exception ex)
			{
				Logger.Log($"Keybind load error: {ex}", Logger.LogLevel.Error);
			}

			labelStyle.alignment = TextAnchor.MiddleLeft;
			labelStyle.normal.textColor = Color.white;
		}

		internal void OnLoad()
		{
			try
			{
				// Load the keybinds from the config.
				Configuration configuration = config.GetConfig();
				if (configuration.keybinds == null || configuration.keybinds.Count == 0)
					// No keybinds in config, write the defaults.
					UpdateBinds(keys);
				else if (configuration.keybinds.Count < keys.Count)
				{
					// Config is missing binds, update missing ones with defaults.
					List<Keybinds.Key> missing = keys.Where(k => !configuration.keybinds.Any(x => x.action == k.action)).ToList();
					foreach (Key key in missing)
					{
						configuration.keybinds.Add(key);
					}
					UpdateBinds(configuration.keybinds);
				}

				// Load the updated binds.
				keys = config.GetConfig().keybinds;
			}
			catch (Exception ex)
			{
				Logger.Log($"Keybinds load error - {ex}", Logger.LogLevel.Error);
			}
		}

		/// <summary>
		/// Find the key for a specified action
		/// </summary>
		/// <param name="action">The action to search by</param>
		/// <returns>The key</returns>
		public Key GetKeyByAction(int action)
		{
			return keys.Where(k => k.action == action).FirstOrDefault();
		}

		/// <summary>
		/// Find the key for a specified action
		/// </summary>
		/// <param name="action">The action to search by</param>
		/// <returns>The key</returns>
		public Key GetKeyByAction(Inputs action)
		{
			return keys.Where(k => k.action == (int)action).FirstOrDefault();
		}

		/// <summary>
		/// Wrapper to update the keybind configuration.
		/// </summary>
		/// <param name="binds">Binds to update</param>
		internal void UpdateBinds(List<Key> binds)
		{
			config.UpdateConfig(new Configuration() { keybinds = binds });
		}

		/// <summary>
		/// Wrapper for Input.GetKeyDown().
		/// </summary>
		/// <param name="action">The action to check</param>
		/// <returns>Returns true if the action is pressed, otherwise false</returns>
		public bool IsActionDown(Inputs action)
		{
			Key key = GetKeyByAction(action);
			Key modifier = GetKeyByAction(Inputs.modifier);

			if (key.useModifier && !Input.GetKey(modifier.key))
				return false;

			if (!key.useModifier && Input.GetKey(modifier.key))
				return false;
				
			if (Input.GetKeyDown(key.key))
				return true;

			return false;
		}

		/// <summary>
		/// <para>Render a rebind menu</para>
		/// <para>This should be called from an OnGUI function</para>
		/// </summary>
		/// <param name="title">The menu title</param>
		/// <param name="actions">Int array of actions to display rebinds for</param>
		/// <param name="x">The X position to display the menu</param>
		/// <param name="y">The Y position to display the menu</param>
		public void RenderRebindMenu(string title, int[] actions, float x, float y, float? widthOverride = null, float? heightOverride = null)
		{
			if (actions.Length == 0)
				return;

			float width = 300f;
			float actionHeight = 40f;
			float actionY = y + 25f;
			float labelWidth = 295f;
			float buttonWidth = 80f;
			float height = 30f + (actions.Length * (actionHeight * 2 + 5f));

			if (widthOverride != null)
				width = widthOverride.Value;

			if (heightOverride != null)
				height = heightOverride.Value;

			float scrollHeight = height - 35f;
			if (height > Screen.height * 0.9f)
				height = Screen.height * 0.9f;

			GUI.Box(new Rect(x, y, width, height), $"<size=16><b>{title}</b></size>");

			Vector2 scrollPosition = GUI.BeginScrollView(new Rect(x + 10f, y + 25f, width - 20f, height - 35f), scrollPositions.ContainsKey(title) ? scrollPositions[title] : new Vector2(0, 0), new Rect(x + 10f, y + 25f, width - 20f, scrollHeight), new GUIStyle(), new GUIStyle());
			if (!scrollPositions.ContainsKey(title))
				scrollPositions.Add(title, scrollPosition);
			else
				scrollPositions[title] = scrollPosition;

			for (int i = 0; i < actions.Length; i++)
			{
				int action = actions[i];
				Key key = GetKeyByAction(action);

				GUI.Label(new Rect(x + 10f, actionY, labelWidth, actionHeight), $"{key.name} - Current ({(key.useModifier ? GetKeyByAction(Inputs.modifier).key.ToString() + " + " : string.Empty)}{key.key})\nDefault ({(key.useModifier ? GetKeyByAction((int)Inputs.modifier).key.ToString() + " + " : string.Empty)}{key.defaultKey})", labelStyle);
				actionY += actionHeight;

				float buttonX = x + 10f;

				string rebindText = rebindAction == action ? "Waiting..." : "Rebind";
				if (GUI.Button(new Rect(buttonX, actionY, buttonWidth, actionHeight / 2), rebindText))
				{
					if (rebindAction == -1)
					{
						rebindAction = action;
					}
				}

				buttonX += buttonWidth + 10f;

				if (GUI.Button(new Rect(buttonX, actionY, buttonWidth, actionHeight / 2), "Reset"))
				{
					key.Reset();
					UpdateBinds(keys);
				}

				buttonX += buttonWidth + 10f;

				if (key.action != 3)
				{
					if (GUI.Button(new Rect(buttonX, actionY, buttonWidth + 20f, actionHeight / 2), $"Modifier - {(key.useModifier ? "On" : "Off")}"))
					{
						key.ToggleUseModifier();
					}
				}

				actionY += actionHeight + 5f;
			}

			GUI.EndScrollView();

			if (rebindAction != -1)
			{
				Key key = GetKeyByAction(rebindAction);
				if (key != null && Input.anyKeyDown)
				{
					foreach (KeyCode keyCode in keyCodes)
					{
						if (Input.GetKey(keyCode) && keyCode != KeyCode.None)
						{
							key.Set(keyCode);
							rebindAction = -1;
							UpdateBinds(keys);
						}
					}
				}
			}
		}
	}
}
