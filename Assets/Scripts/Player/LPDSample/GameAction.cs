using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// an ACTION is anything that a character can do,
// and holds detailed and concrete information about the visual, logical components of the action.
public abstract class GameAction 
{
    // It's very useful to have ServerCharacter component cached.
    protected ServerCharacter _serverCharacter;
    // this request data is used to broadcast the action to all clients.
    // it's the same data that the requesting player has sent to the server.
    // we just cache it at construction and recycle it to replicate the action on the clients.
    protected GameActionRequest _requestData;

    // constructor
    public GameAction(ref GameActionRequest req, ServerCharacter parent)
    {
        _serverCharacter = parent;
        _requestData = req;
    }


    // factory(?)
    public static GameAction CreateActionFromData(ref GameActionRequest req, ServerCharacter character)
    {
        Datasource.Instance.ActionDataByType.TryGetValue(req.ActionType, out var data);
        switch (data.ActionType)
        {
            //case GameActionType.SampleAction: return new WalkAction(ref data, character);

            default: throw new System.NotImplementedException();
        }
    }

    // any intialization of variables should go here. 
    public abstract void BeginAction();

    // anything that must be done every frame;
    public virtual bool UpdateAction()
    {
        // return true if wish to continue on the next frame
        // return false if the action wants to be ended next frame.
        return true;
    }

    // clean up resources and reset everything
    public abstract void EndAction();

    
}

// holds all information about the gameAction.
[CreateAssetMenu(menuName = "Data/GameActionData")]
public class GameActionData : ScriptableObject
{

    // the type of action
    public GameActionType ActionType;

    [Tooltip("The name of the trigger to set to start this action")]
    public string Anim;

    [Tooltip("If there's more than one,,")]
    public string Anim2;

    [Tooltip("any animator variable")]
    public string AnimatorVariable;

    [Tooltip("Cannot move while this action is being played?")]
    public bool FreezeMovement;

    [Tooltip("action's running time in milliseconds, negative if it's indefinite, i.e. must be canceled by other means")]
    public int RunningTime;

}

public enum GameActionType
{
    // example data..
    TurnInPlace,
    Kick3Times,
    Crouch
}

// A lightweight serializable 
public class GameActionRequest
{
    public GameActionType ActionType;

}