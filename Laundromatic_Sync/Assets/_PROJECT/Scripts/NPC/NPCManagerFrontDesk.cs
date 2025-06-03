using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCManagerFrontDesk : MonoBehaviour
{
    public static NPCManagerFrontDesk instance;
    [SerializeField]List<GameObject> npcPrefabs = new List<GameObject>();
    public List<GameObject> npcs = new List<GameObject>();
    public List<GameObject> npcsWaiting = new List<GameObject>();
    [SerializeField]List<Transform> lineTransforms = new List<Transform>();
    [SerializeField]List<Transform> waitingArea = new List<Transform>();
    
    [SerializeField] Transform npcSpawnPoint;

    public Transform basketPlacePosition;
    public bool hasBasket = false;
    
    //Spawn a random npc
    //Make them wait in a row
    //Leave when order is completed
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    { 
        
        for (int i = 0; i < 5; i++)
        {
            int npcToSpawnCount = Random.Range(0, 3);
            int currentNpcIndex = npcs.Count;
            npcs.Add(Instantiate(npcPrefabs[npcToSpawnCount], npcSpawnPoint.position, Quaternion.identity));
            npcs[currentNpcIndex].transform.DOMove(lineTransforms[currentNpcIndex].position, 1f) ;
            npcs[currentNpcIndex].GetComponent<NPCBehaviour>().ID = i;
            npcs[currentNpcIndex].GetComponent<NPCBehaviour>().currentLinePosition = i;
            if (currentNpcIndex == 0)
            {
                npcs[currentNpcIndex].GetComponent<NPCBehaviour>().FrontOfLine();
            }
        }
    }

    public void MoveLineForward(int npcLinePosition)
    {
        
        npcs[npcLinePosition].transform.DOMove(lineTransforms[npcLinePosition].position, 1f);
        // for (int i = 0; i < npcs.Count; i++)
        // {
        //     
        //     if (!npcs[i].GetComponent<NPCBehaviour>().hasBasket) return;
        //     if (npcs[i].GetComponent<NPCBehaviour>().currentLinePosition == 0) return;
        //     
        //     if (npcs[i].GetComponent<NPCBehaviour>().currentLinePosition != 0) npcs[i].GetComponent<NPCBehaviour>().currentLinePosition--;
        //     npcs[i].transform.DOMove(lineTransforms[npcs[i].GetComponent<NPCBehaviour>().currentLinePosition].position, 1f);
        //     if(npcs[i].GetComponent<NPCBehaviour>().currentLinePosition == 0 && !npcs[i].GetComponent<NPCBehaviour>().hasBasket) GoToWaitingArea(npcs[i].GetComponent<NPCBehaviour>().ID);
        //     
        // }
        
    }
    
    

    public void GoToWaitingArea(int ID)
    {
        npcsWaiting[ID].transform.DOMove(waitingArea[ID].position, 1f) ;
        
    }

    private void Update()
    {
        if (basketPlacePosition.childCount == 0)
        {
            hasBasket = false;
        }
        else
        {
            hasBasket = true;
        }
    }
    
    
}
