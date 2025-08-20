using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public enum RoomType
{
    MOB,
    SHOP,
    PUZZLE,
    PREBOSS,
    BOSS
}
[CreateAssetMenu(menuName = "Dungeon Room", fileName = "New Room")]
public class DungeonRoom : ScriptableObject
{
    public RoomType type;
    public GameObject RoomLayout;
    public Vector2 chestLocation;

    public GameObject Obstacles;
    public List<GameObject> Enemies;
    public Vector2[] EnemySpawnPoints;

    public int minEnemies;
    public int maxEnemies;

    public int chanceToSpawn;
    public int floorNumber;


}
