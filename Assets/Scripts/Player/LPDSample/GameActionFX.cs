using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameActionFX
{
    // To play a certain action, the action needs an animator, audio player, particle system,,, etc.
    // these componenets are accessed through the CharacterVisualization  component.
    protected ClientCharacterVisualization _characterVisual;

    public GameActionFX(ref GameActionRequest req, ClientCharacterVisualization character)
    {
        _characterVisual = character;
    }

    public static GameActionFX CreateActionFXFromData(ref GameActionRequest req, ClientCharacterVisualization character)
    {
        Datasource.Instance.ActionDataByType.TryGetValue(req.ActionType, out var data);
        switch (data.ActionType)
        {
            //case GameActionType.Walk: return new WalkActionFX(ref data, character);

            default: throw new System.NotImplementedException();
        }
    }


    // abstract methods!

    // any intialization of variables should go here. 
    public abstract void BeginAction();

    // anything that must be done every frame;
    public virtual bool UpdateAction()
    {
        // return true if wish to continue on the next frame
        // return false if the action wants to be ended next frame.
        return true;
    }

    // clean up resources and stop any effects
    // this should be used for interrupting actions currently being played.
    public abstract void EndAction();

    // sometimes action may need to be queued!~
}
