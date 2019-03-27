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

    public float randomDistance;

    public string attack1 = "AttackTrigger";

    public enum Class
    {
        Archer, Swordie, Mage, Healer
    }


    public Class myClass;


    public class Healer : EnemyProfiles
    {
        public Healer(int level)
        {
            myClass = Class.Healer;
            Level = level;
            maxHealth = 40 * level;
            maxDamage = 1 * level;

            switch (level)
            {
                case 1:
                    baseSize = .6f;
                    break;
                case 2:
                    baseSize = .7f;
                    break;
                case 3:
                    baseSize = .8f;
                    break;
                case 4:
                    baseSize = 1f;
                    break;
            }
        }
    }

    public class Archer : EnemyProfiles
    {
        public Archer(int level)
        {
            attack1 = "Longbow";
            myClass = Class.Archer;
            Level = level;
            maxHealth = 40 * level;
            maxDamage *= level;


            randomDistance = Random.Range(baseDistance - 5, baseDistance + 10);
            switch (level)
            {
                case 1:
                    baseSize = .6f;
                    break;
                case 2:
                    baseSize = .7f;
                    break;
                case 3:
                    baseSize = .8f;
                    break;
                case 4:
                    baseSize = 1f;
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
            maxHealth = 20 * level;
            baseDistance = 7;

            randomDistance = Random.Range(baseDistance - 5, baseDistance + 10);
            switch (level)
            {
                case 1:
                    baseSize = .6f;
                    break;
                case 2:
                    baseSize = .7f;
                    break;
                case 3:
                    baseSize = .8f;
                    break;
                case 4:
                    baseSize = 1f;
                    break;
            }

        }

    }

    public class Mage : EnemyProfiles
    {
        public Mage(int level)
        {
            attack1 = "Cast1";
            myClass = Class.Mage;
            Level = level;
            maxHealth = 30 * level;

            randomDistance = Random.Range(baseDistance - 5, baseDistance + 10);

            switch (level)
            {
                case 1:
                    baseSize = .6f;
                    break;
                case 2:
                    baseSize = .7f;
                    break;
                case 3:
                    baseSize = .8f;
                    break;
                case 4:
                    baseSize = 1f;
                    break;
            }
        }
      
    }

}
