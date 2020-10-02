using ExitGames.Client.Photon.Encryption;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyComponent _prefab;
    public Enemy EnemyToSpawn;

    public List<EnemyComponent> enemies_list = new List<EnemyComponent>();

    public float randomRange_lowerbound = 1;
    public float randomRange_upperbound = 6;
    public float waitTimeBetweenSpawning = 3.0f; //The time between spawning a new enemy, if one can be spawned.
    public int noOfEnemiesSpawnable = 5; //The total number of enemies that can be spawned at any given time.
    float startX = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        spawnEnemy(5.0f);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if ((enemies_list.Count < noOfEnemiesSpawnable) && (waitTimeBetweenSpawning <= 0.0f))
            {
                spawnEnemy(Random.Range(0.0f, this.transform.localScale.x));
                waitTimeBetweenSpawning = Random.Range(randomRange_lowerbound, randomRange_upperbound);
            }
            if (waitTimeBetweenSpawning > -1)
            {
                waitTimeBetweenSpawning -= Time.deltaTime;
            }

        }
    }

    void spawnEnemy (float x)
    {
        startX = (this.transform.position.x) - (this.transform.localScale.x / 2.0f);

        GameObject newGO = PhotonNetwork.Instantiate("enemy", new Vector2((startX + x), (this.transform.position.y - (this.transform.localScale.y / 3.5f))), Quaternion.identity, 0);

        newGO.GetComponent<EnemyComponent>().enemyReference = EnemyToSpawn;
        newGO.GetComponent<EnemyComponent>().parentSpawner = this;

        enemies_list.Add(newGO.GetComponent<EnemyComponent>());

    }


    // when enemy leaves the trigger remove them from list
    // only if their enemyReference.identifier == EnemyToSpawn.identifier


    // when enemy enters the trigger add them from list
    // only if their enemyReference.identifier == EnemyToSpawn.identifier

}
