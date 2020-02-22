using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Contains scene management code for use with UI buttons.
/// Attach this script to the canvas that contains the button you want to do scene management with.
/// Go into said button, and under the light grey "OnClick ()" panel, click the little plus in the lower right.
/// Now drag the canvas the button is attached to into the slot that appears.
/// </summary>
public class MenuLoader : MonoBehaviour
{
    /// <summary>
    /// Loads a scene based on a given index. The scene must be added to the BuildSettings.
    /// </summary>
    /// <param name="sceneIndex">The index of the scene to load.</param>
    public void LoadSceneOnClick(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    public void ExitGameOnClick()
    {
        Application.Quit();
    }
}
