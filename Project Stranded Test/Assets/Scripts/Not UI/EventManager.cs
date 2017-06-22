using System.Collections;
using System.Collections.Generic;


static public class EventManager {

    public delegate void Reset();
    static public event Reset reset;

    static public void ResetObjects()
    {
        reset();
    }
}
