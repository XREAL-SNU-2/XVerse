using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the single source of truth.
public class Datasource : MonoBehaviour
{
    // put action data in the inspector!
    [SerializeField]
    private GameActionData[] _actionData;

    // action cached as a dictionary with type as key.
    private Dictionary<GameActionType, GameActionData> _actionDataMap;

    public static Datasource Instance { get; private set; }
    public Dictionary<GameActionType, GameActionData> ActionDataByType
    {
        get
        {
            // initialization. should run only once, the first time this Get runs.
            if (_actionDataMap == null)
            {
                _actionDataMap = new Dictionary<GameActionType, GameActionData> ();
                foreach (GameActionData data in _actionData)
                {
                    _actionDataMap[data.ActionType] = data;
                }
            }
            return _actionDataMap;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }
}
