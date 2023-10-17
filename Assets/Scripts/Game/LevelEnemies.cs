using RubyGame.GamePlay;
using RubyGame.Managers;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnemies : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();

    private void Start()
    {
        BattleManager.Instance.battleEnemies = enemies;
    }
}
