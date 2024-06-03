using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionButton : MonoBehaviour
{
    [SerializeField] private LevelSelectionMain manager;

    [Header("Level Details")]
    [SerializeField] private string levelName;
    [SerializeField] private string levelDescription;
    [SerializeField] private string levelScene;
    [SerializeField] private Sprite levelImage;

    public void UpdateCurrentLevel()
    {
        manager.UpdateCurrentLevel(levelName, levelDescription, levelScene, levelImage);
    }
}
