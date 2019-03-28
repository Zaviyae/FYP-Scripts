using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level {

    public int levelnum;
    public List<Wave> waves;

    public Level(int levelnum, List<Wave> waves)
    {
        this.levelnum = levelnum;
        this.waves = waves;
    }

}
