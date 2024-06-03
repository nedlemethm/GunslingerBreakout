using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSignals
{
	public static readonly Signal TOOLBAR_ENABLED = new("ToolbarEnabled");
	public static readonly Signal TOOLBAR_DISABLED = new("ToolbarDisabled");
	public static readonly Signal PAUSE_TOGGLED = new("PauseToggled");
}