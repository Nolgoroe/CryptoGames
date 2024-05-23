using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChainAction
{
    void NotifyObserver(int currentComboReached);
}
