using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProfiles {

    public int maxHealth = 40;
    public int maxDamage = 10;

    public float baseDistance = 20;
    public float baseSpeed = 2f;
    public float baseSize = 1f;
    public int Level = 1;
    public enum Class
    {
        Archer, Swordie, Mage
    }


    public Class myClass;

    public Dictionary<string, bool> availableThoughts = new Dictionary<string, bool>();


    public class Archer : EnemyProfiles
    {
        public Archer(int level)
        {
            myClass = Class.Archer;
            Level = level;
            maxHealth = 40 * level;
            maxDamage *= level;

            availableThoughts.Add("Aim", true);
            availableThoughts.Add("Rush", true);
            availableThoughts.Add("Shield", true);

            switch (level)
            {
                case 1:
                    baseSize = 1f;
                    break;
                case 2:
                    baseSize = 1.3f;
                    break;
                case 3:
                    baseSize = 1.6f;
                    break;
                case 4:
                    baseSize = 2f;
                    break;
            }
        }
        
    }

    public class Swordie : EnemyProfiles
    {
        public Swordie(int level)
        {
            myClass = Class.Swordie;
            Level = level;
            maxHealth = 20;
            baseDistance = 7;
        }

    }

    public class Mage : EnemyProfiles
    {
        public Mage(int level)
        {
            myClass = Class.Mage;
            Level = level;
            maxHealth = 30;
            availableThoughts.Add("Aim", false);
            availableThoughts.Add("Shield", true);
        }
      
    }

}
