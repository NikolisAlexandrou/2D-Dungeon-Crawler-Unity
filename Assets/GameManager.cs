using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager i;

    public NavMeshSurface2d nav;

    public GameObject DamageIndicator;

    public int floorNumber;
    public GameObject Chest;
    public List<DungeonRoom> Rooms;
    public DungeonRoom _dungeonRoom;
    GameObject LockedDoor;

    public int enemySum;
    public List<GameObject> enemies;
    public bool spawnedAllEnemies;

    bool isCollider;
    bool hasSpawned;

    public Transform player;
    GameObject enemy;

    public bool alreadyCleared;

    public static GameManager getInstance()
    {
        return i;
    }
    private void Awake()
    {
        i = this;
    }
    private void Start()
    {
        int r = Random.Range(0, Rooms.Count);
        _dungeonRoom = Rooms[r];
        LoadRoomLayout();
        nav.AddData();
    }

    public void LoadRoomLayout()
    {
        GameObject d = Instantiate(_dungeonRoom.RoomLayout, new Vector2(0,0), Quaternion.identity);
        LockedDoor = d.transform.GetChild(0).gameObject;
    }

    public void UnlockDoor()
    {
        if (ClearedRoom() == true && alreadyCleared == false)
        {
            alreadyCleared = true;
            
            LockedDoor.AddComponent<LockedDoor>();
            LockedDoor.GetComponent<SpriteRenderer>().sprite = null;
            if (_dungeonRoom.type == RoomType.MOB)
            {
                spawnChest();
            }
        }
    }
    void spawnChest()
    {
        GameObject chest = Instantiate(Chest, _dungeonRoom.chestLocation, Quaternion.identity);
        chest.GetComponent<Chest>().floorNumber = floorNumber;
    }
    public void NextRoom()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void spawnEnemy(GameObject ob, Vector2 pos)
    {
        GameObject ob1 = Instantiate(ob, pos, Quaternion.identity);
        ob1.GetComponent<EnemyAI>().player = player;
        enemies.Add(ob1);
    }

    void SpawnEnemiesUsingSpawnPoints()
    {
        if (spawnedAllEnemies == false)
        {
            if (enemySum < _dungeonRoom.maxEnemies)
            {
                if (hasSpawned == false)
                {
                    int pos = Random.Range(0, _dungeonRoom.EnemySpawnPoints.Length - 1);
                    Vector2 position = _dungeonRoom.EnemySpawnPoints[pos];
                    hasSpawned = true;
                    enemySum += 1;
                    int r = Random.Range(0, _dungeonRoom.Enemies.Count);
                    enemy = _dungeonRoom.Enemies[r];

                    spawnEnemy(enemy, position);
                    //ob1.transform.SetParent(trashParent.transform);
                    //AllEnemies.Add(ob1.transform);
                    StartCoroutine(resetTimer());
                }
            }
            else
            {
                spawnedAllEnemies = true;
            }
        }
    }

    void SpawnEnemies()
    {
        if (spawnedAllEnemies == false)
        {
            if (enemySum < _dungeonRoom.maxEnemies)
            {
                if (hasSpawned == false)
                {
                    Vector2 position = new Vector3(Random.Range(-15, 15), Random.Range(-12, 12));
                    foreach (Collider2D collider in Physics2D.OverlapCircleAll(position, 1))
                    {
                        if (collider.CompareTag("Collider"))
                        {
                            isCollider = true;
                        }
                        else
                        {
                            isCollider = false;


                        }
                    }
                    if (isCollider == false)
                    {
                        hasSpawned = true;
                        enemySum += 1;
                        int r = Random.Range(0, _dungeonRoom.Enemies.Count);
                        enemy = _dungeonRoom.Enemies[r];

                        spawnEnemy(enemy, position);
                        //ob1.transform.SetParent(trashParent.transform);
                        //AllEnemies.Add(ob1.transform);
                        StartCoroutine(resetTimer());
                    }
                }
            }
            else
            {
                spawnedAllEnemies = true;
            }
        }
    }

    private void Update()
    {
        if (_dungeonRoom.type == RoomType.MOB)
        {
            SpawnEnemiesUsingSpawnPoints();
        }
        UnlockDoor();
    }
    IEnumerator resetTimer()
    {
        yield return new WaitForSeconds(0f);
        hasSpawned = false;
    }

    public bool ClearedRoom()
    {
        bool cleared = false;
        if ((enemies.Count <= 0 && spawnedAllEnemies == true) || _dungeonRoom.type == RoomType.PUZZLE)
        {
            cleared = true;
        }

        return cleared;
    }
}
