using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelsContoller : MonoBehaviour
{
    public List<LevelData> levels;

    public int currentLevel;
    //----------------
    [System.Serializable]
    public class LevelData
    {
        public List<GameObject> levelObjects;
    }
    void Start()
    {
        LoadLevel(currentLevel);
    }

    public void NextLevel()
    {
        LoadLevel(currentLevel + 1);
    }

    public void LoadLevel(int num)
    {
        foreach (var obj in levels[currentLevel].levelObjects.ToList())
        {
            obj.SetActive(false);
        }

        currentLevel = num;
        
        foreach (var obj in levels[currentLevel].levelObjects.ToList())
        {
            obj.SetActive(true);
        }

    }
}
