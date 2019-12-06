using Enums;
using System.Collections.Generic;
using UnityEngine;

public class MachineManager : MonoBehaviour
{
    static private MachineManager _instance;
    static public MachineManager Instance { get { return _instance; } }
    public MachineBehaviour[] machinePrefabs;
    public BoxCollider leftPlayArea;
    public BoxCollider rightPlayArea;

    [HideInInspector]
    public Dictionary<PlayableArea, List<MachineBehaviour>> SpawnedMachines { get; private set; } = new Dictionary<PlayableArea, List<MachineBehaviour>>();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        SpawnMachines(PlayableArea.Left);
        SpawnMachines(PlayableArea.Right);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private BoxCollider GetArea(PlayableArea areaType)
    {
        return (areaType == PlayableArea.Left) ? leftPlayArea : rightPlayArea;
    }

    private void SpawnMachines(PlayableArea areaType)
    {
        BoxCollider area = GetArea(areaType);
        List<MachineBehaviour> machineList = new List<MachineBehaviour>(machinePrefabs.Length);
        foreach (MachineBehaviour prefab in machinePrefabs)
        {
            float xPos = Random.Range(area.bounds.min.x, area.bounds.max.x);
            float zPos = Random.Range(area.bounds.min.z, area.bounds.max.z);
            MachineBehaviour spawned = Instantiate<MachineBehaviour>(prefab);
            spawned.transform.position = new Vector3(xPos, spawned.transform.position.y, zPos);
            machineList.Add(spawned);
        }
        SpawnedMachines.Add(areaType, machineList);
    }
}
