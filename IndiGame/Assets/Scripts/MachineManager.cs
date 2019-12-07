using Enums;
using System.Collections.Generic;
using UnityEngine;

public class MachineManager : MonoBehaviour
{
    [System.Serializable]
    public class TimingFloatValuePair
    {
        public float timing;
        public float value;
    }
    [System.Serializable]
    public class TimingIntValuePair
    {
        public float timing;
        public int value;
    }
    static private MachineManager _instance;
    static public MachineManager Instance { get { return _instance; } }
    public MachineBehaviour[] machinePrefabs;
    public BoxCollider leftPlayArea;
    public BoxCollider rightPlayArea;
    public int currentPossibleDamagedMachineCount = 2;
    public float machineDamageCheckInterval = 3f;
    public float machineDamageProbability = 0.05f;
    public float machineMaxHp = 100f;
    public float machineBrokePenaltyTime = 2f;
    public float machineRecoverStopTime = 2f;
    public float machineDamagePerSec = 10f;
    public float machineRecoverPerSec = 15f;
    public float machineMinGreenHp = 70f;
    public float machineMinYellowHp = 30f;
    public float machineMinRedHp = 0f;
    public TimingFloatValuePair[] machineDamageProbabilityWithTime;
    public TimingIntValuePair[] machinePossibleDamagedCountWithTime;

    private float _machineDamageCheckCounter;
    private int _damageProbListIndex = 0;
    private int _possibleDamagedCountListIndex = 0;
    private float _elapsedTime = 0;

    [HideInInspector]
    public Dictionary<PlayableArea, List<MachineBehaviour>> SpawnedMachines { get; private set; } = new Dictionary<PlayableArea, List<MachineBehaviour>>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(_instance.gameObject);
        }
        _instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        SpawnMachines(PlayableArea.Left);
        SpawnMachines(PlayableArea.Right);
        _machineDamageCheckCounter = machineDamageCheckInterval;
    }

    // Update is called once per frame
    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_damageProbListIndex < machineDamageProbabilityWithTime.Length && machineDamageProbabilityWithTime[_damageProbListIndex].timing <= _elapsedTime)
        {
            machineDamageProbability = machineDamageProbabilityWithTime[_damageProbListIndex].value;
            _damageProbListIndex++;
        }
        if (_possibleDamagedCountListIndex < machinePossibleDamagedCountWithTime.Length && machinePossibleDamagedCountWithTime[_possibleDamagedCountListIndex].timing <= _elapsedTime)
        {
            currentPossibleDamagedMachineCount = machinePossibleDamagedCountWithTime[_possibleDamagedCountListIndex].value;
            _possibleDamagedCountListIndex++;
        }

        if (!CanDamageMachine())
        {
            return;
        }

        _machineDamageCheckCounter -= Time.deltaTime;
        if (_machineDamageCheckCounter <= 0)
        {
            _machineDamageCheckCounter = machineDamageCheckInterval;
            List<MachineBehaviour> list = new List<MachineBehaviour>(machinePrefabs.Length * 2);
            list.AddRange(SpawnedMachines[PlayableArea.Left]);
            list.AddRange(SpawnedMachines[PlayableArea.Right]);

            System.Random random = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                MachineBehaviour value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            foreach (MachineBehaviour mb in list)
            {
                if (mb.State == MachineState.Normal && Random.Range(0, 1) <= machineDamageProbability)
                {
                    mb.State = MachineState.Damaged;
                    break;
                }
            }
        }
    }

    private BoxCollider GetArea(PlayableArea areaType)
    {
        return (areaType == PlayableArea.Left) ? leftPlayArea : rightPlayArea;
    }

    private void SpawnMachines(PlayableArea areaType)
    {
        // 이 로직 공통으로 빼자...
        List<int> list = new List<int> { 0, 1, 2, 3 };
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            int value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        BoxCollider area = GetArea(areaType);
        List<MachineBehaviour> machineList = new List<MachineBehaviour>(machinePrefabs.Length);
        for (int i = 0; i < list.Count; i++)
        {
            int index = list[i];
            // 2 3 
            // 0 1 이 위치
            Vector2 minPos = new Vector2(area.bounds.min.x, area.bounds.min.z);
            if (i % 2 == 1)
            {
                minPos.x += area.bounds.size.x * 0.5f;
            }
            if (i >= 2)
            {
                minPos.y += area.bounds.size.z * 0.5f;
            }
            float xPos = Random.Range(minPos.x, minPos.x + area.bounds.size.x * 0.5f);
            float zPos = Random.Range(minPos.y, minPos.y + area.bounds.size.z * 0.5f);
            MachineBehaviour spawned = Instantiate<MachineBehaviour>(machinePrefabs[index]);
            spawned.areaType = areaType;
            spawned.transform.position = new Vector3(xPos, area.bounds.max.y, zPos);
            machineList.Add(spawned);
        }
        SpawnedMachines.Add(areaType, machineList);
    }

    private bool CanDamageMachine()
    {
        int damagedCount = 0;
        foreach (MachineBehaviour mb in SpawnedMachines[PlayableArea.Left])
        {
            if (mb.State == MachineState.Broken || mb.State == MachineState.Normal)
            {
                continue;
            }
            damagedCount++;
        }
        foreach (MachineBehaviour mb in SpawnedMachines[PlayableArea.Right])
        {
            if (mb.State == MachineState.Broken || mb.State == MachineState.Normal)
            {
                continue;
            }
            damagedCount++;
        }
        return damagedCount < currentPossibleDamagedMachineCount;
    }
}
