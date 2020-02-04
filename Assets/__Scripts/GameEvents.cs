using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    // decalring all of the relevant gameobjects => pickups, mines, and boosts
    public GameObject PickupPrefab;
    public GameObject MinePrefab;
    public GameObject BoostPrefab;
    public int PCcount, MCcount, BCcount, PLcount, MLcount, BLcount, POcount, MOcount, BOcount; // counts number for object spawns

    // centers of each of the three islands as coordinates
    private Vector3 Ccenter = new Vector3 (0, 0, 0);
    private Vector3 LcenterA = new Vector3 (-68, 0, -65);
    private Vector3 LcenterB = new Vector3 (-63, 0, -70);
    private Vector3 LcenterC = new Vector3 (-68, 0, -65);
    private Vector3 Ocenter = new Vector3 (71, 0, 77);

    // Start is called before the first frame update
    void Start()
    {  
        // function to spawn the three types of items on the three islands
        SpawnItems(PickupPrefab, PCcount, 14, 0.4f, 14, Ccenter);
        SpawnItems(PickupPrefab, PLcount, 9, 1.0f, 4, LcenterA);
        SpawnItems(PickupPrefab, POcount, 8, 1.0f, 8, Ocenter);
        SpawnItems(MinePrefab, MCcount, 14, 0.1f, 14, Ccenter);
        SpawnItems(MinePrefab, MLcount, 4, 0.7f, 6, LcenterB);
        SpawnItems(MinePrefab, MOcount, 8, 0.7f, 8, Ocenter);
        SpawnItems(BoostPrefab, BCcount, 14, 0.2f, 14, Ccenter);
        SpawnItems(BoostPrefab, BLcount, 4, 0.8f, 4, LcenterC);
        SpawnItems(BoostPrefab, BOcount, 8, 0.8f, 8, Ocenter);
    }

    // random spawn generation 
    void SpawnItems(GameObject item, int count, int xRange, float yRange, int zRange, Vector3 center)
    {
        for (int i = 0; i < count; i++) // spawns as many as the count is
        {   // choosing range of position values that the item will spawn in
            Vector3 pos = center + new Vector3(Random.Range(-xRange, xRange), yRange, Random.Range(-zRange, zRange));
            Instantiate(item, pos, Quaternion.identity); // creates an instance of the given item
        }
    }
}
