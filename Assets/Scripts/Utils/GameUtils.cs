using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtils 
{
   public static readonly int MaxCharacters = 4;

   public static  int SelectedSkin = 0;

   public static float MaxBoostSpeedMultiplier = .5f;

   public static float BoostSpeedMultiplier = 0.2f;
  
#if UNITY_EDITOR
   /// <summary>
   /// Disables rendering debug manager in Unity Editor
   /// https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@14.0/manual/Render-Pipeline-Debug-Window.html#how-to-access-the-rendering-debugger
   ///
   /// <br/><br/>
   ///
   /// Prevent usable hotkeys from opening the rendering debug manager:
   /// <br/>
   /// PC: Left Ctrl + Backspace
   /// <br/>
   /// Gamepad: L3 + R3 (press the left and right sticks)
   /// <br/>
   /// Mobile: Three-finger tap
   /// </summary>
   /// ReSharper disable once CheckNamespace
   public static class DisableRenderingDebugManager // renders Display Stats window
   {
      [UnityEditor.InitializeOnLoadMethod]
      private static void OnEditorLoaded()
      {
         UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false;
      }
   }
#endif
}
