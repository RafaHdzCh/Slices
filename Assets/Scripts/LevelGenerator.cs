using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public LevelBlock firstBlock;
    public static LevelGenerator sharedInstance;
    public List<LevelBlock> allTheLevelBlocks = new List<LevelBlock>();
    public Transform LevelStartPoint;
    public List<LevelBlock> currentBlocks = new List<LevelBlock>();

    private void Awake()
    {
        sharedInstance = this;
    }

    private void Start()
    {
        GenerateInitialBlocks();
    }

    public void AddLevelBlock()
    {
        int randomIndex = Random.Range(0, allTheLevelBlocks.Count);                             //Random.Range(a,b) genera un numero aleatorio emtero entre a<= y >b.
        LevelBlock currentBlock;
        Vector3 spawnPosition = Vector3.zero;
        if(currentBlocks.Count==0)
        {
            currentBlock = (LevelBlock) Instantiate(firstBlock);
            currentBlock.transform.SetParent(this.transform , false);
            spawnPosition = LevelStartPoint.position;
        }
        else
        {
            currentBlock = (LevelBlock) Instantiate(allTheLevelBlocks[randomIndex]);     //Convierte en LevelBlock la instanciacion que se encuentre en la posicion RandomIndex en allTheLevelBlocks.
            currentBlock.transform.SetParent(this.transform,false);
            spawnPosition = currentBlocks[currentBlocks.Count-1].exitPoint.position;
        }

        Vector3 correction = new Vector3(spawnPosition.x-currentBlock.startPoint.position.x,
                                         spawnPosition.y-currentBlock.startPoint.position.y,
                                         0);

        currentBlock.transform.position = correction;
        currentBlocks.Add(currentBlock);
    }
    public void RemoveOldestLevelBlock()
    {
        //Debug.Log("Vamos a destruir un bloque, de momento hay: " + currentBlocks.Count);
        LevelBlock oldestBlock = currentBlocks[0];
        currentBlocks.Remove(oldestBlock);
        Destroy(oldestBlock.gameObject);
        //Debug.Log("Hemos destruido un bloque y ahora quedan: " + currentBlocks.Count);
    }
    public void RemoveAllTheBlocks()
    {
        while(currentBlocks.Count>0)
        {
            RemoveOldestLevelBlock();
        }
    }
    public void GenerateInitialBlocks()
    {
        for(int i=0; i<2; i++)
        {
            AddLevelBlock();
        }
    }
}
