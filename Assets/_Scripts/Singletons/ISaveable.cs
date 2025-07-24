using UnityEngine;

public interface ISaveable
{
    public object CaptureState();     //create relevant data-storing object depending 
    public void RestoreState(object data);
}