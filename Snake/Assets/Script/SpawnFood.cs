using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpawnFood : MonoBehaviour {
    // Food Prefab
    //public GameObject foodPrefab;
    public List<GameObject> spawnables = new List<GameObject>();

    // Borders
    public Transform borderTop;
    public Transform borderBottom;
    public Transform borderLeft;
    public Transform borderRight;

    // Use this for initialization
    void Start () {
        // Spawn food every 4 seconds, starting in 3
        InvokeRepeating("Spawn", 3, 4);
    }

    // Spawn one piece of food
    void Spawn() {
        // x position between left & right border
        int x = (int)Random.Range(borderLeft.position.x,
                                  borderRight.position.x);

        // y position between top & bottom border
        int y = (int)Random.Range(borderBottom.position.y,
                                  borderTop.position.y);

        // Instantiate the food at (x, y)
        Instantiate(spawnables[0],
                    new Vector2(x, y),
                    Quaternion.identity); // default rotation
    }
}