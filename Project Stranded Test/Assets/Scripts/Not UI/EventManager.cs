using System.Collections;
using System.Collections.Generic;


static public class EventManager {

    public delegate void ResetDelegate();
    static public event ResetDelegate Reset;

    static public void ResetObjects()
    {
        if (Reset != null)
        {
            Reset();
        }
    }
}
