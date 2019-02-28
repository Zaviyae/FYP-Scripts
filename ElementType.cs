public class ElementType {

    public enum Type {Force, Water, Lightning, Neutral}

    public static float getDamageModifier(Type t1, Type t2)
    {
        switch (t1)
        {
            case Type.Force:

                switch (t2)
                {
                    case Type.Force:
                        return 0.1f;
                    case Type.Water:
                        return 1f;
                    case Type.Lightning:
                        return 1f;
                    case Type.Neutral:
                        return .1f;
                }

                break;

            case Type.Water:

                switch (t2)
                {
                    case Type.Force:
                        return 1f;
                    case Type.Water:
                        return 0.1f;
                    case Type.Lightning:
                        return 1f;
                    case Type.Neutral:
                        return .1f;
                }

                break;

            case Type.Lightning:

                switch (t2)
                {
                    case Type.Force:
                        return 1f;
                    case Type.Water:
                        return 1f;
                    case Type.Lightning:
                        return 0.1f;
                    case Type.Neutral:
                        return .1f;
                }

                break;

            case Type.Neutral:

                switch (t2)
                {
                    case Type.Force:
                        return 0.1f;
                    case Type.Water:
                        return 0.1f;
                    case Type.Lightning:
                        return 0.1f;
                    case Type.Neutral:
                        return 1f;
                }

                break;
        }

        return 0;
    }
}
