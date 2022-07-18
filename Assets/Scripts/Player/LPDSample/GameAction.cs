using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// an ACTION is anything that a character can do,
// and holds detailed and concrete information about the visual, logical components of the action.
public abstract class GameAction : MonoBehaviour
{
    // any intialization of variables should go here. 
    public abstract void BeginAction();


    // clean up resources and stop any effects
    // this should be used for interrupting actions currently being played.
    public abstract void EndAction();

    // sometimes action may need to be queued!~
    
}
