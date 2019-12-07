using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    static private ToolManager _instance;
    static public ToolManager Instance { get { return _instance; } }

    public Tool[] toolPrefabs;
    [HideInInspector]
    public List<Tool> spawnedTools;
    public float toolSpawnY = 20f;
    public float toolMinY = -15f;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(_instance.gameObject);
        }
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnedTools = new List<Tool>(toolPrefabs.Length);
        foreach (Tool prefab in toolPrefabs)
        {
            Tool spawned = Instantiate<Tool>(prefab);
            spawnedTools.Add(spawned);
            RepositionTool(spawned);
        }
    }

    void Update()
    {
        foreach (Tool tool in spawnedTools)
        {
            if (tool.transform.position.y < toolMinY)
            {
                RepositionTool(tool);
            }
        }
    }

    void RepositionTool(Tool tool)
    {
        BoxCollider area = Random.Range(0, 2) == 0 ? MachineManager.Instance.leftPlayArea : MachineManager.Instance.rightPlayArea;
        float posX = Random.Range(area.bounds.min.x, area.bounds.max.x);
        float posZ = Random.Range(area.bounds.min.z, area.bounds.max.z);
        tool.transform.position = new Vector3(posX, toolSpawnY, posZ);
        Rigidbody rb = tool.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
        }
    }
}
