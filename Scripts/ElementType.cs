public class ElementType {

    public enum Type {Purple, Blue, Red, Neutral}

    public static float getDamageModifier(Type t1, Type t2)
    {
        switch (t1)
        {
            case Type.Purple:

                switch (t2)
                {
                    case Type.Purple:
                        return 0.1f;
                    case Type.Blue:
                        return 1f;
                    case Type.Red:
                        return 1f;
                    case Type.Neutral:
                        return .1f;
                }

                break;

            case Type.Blue:

                switch (t2)
                {
                    case Type.Purple:
                        return 1f;
                    case Type.Blue:
                        return 0.1f;
                    case Type.Red:
                        return 1f;
                    case Type.Neutral:
                        return .1f;
                }

                break;

            case Type.Red:

                switch (t2)
                {
                    case Type.Purple:
                        return 1f;
                    case Type.Blue:
                        return 1f;
                    case Type.Red:
                        return 0.1f;
                    case Type.Neutral:
                        return .1f;
                }

                break;

            case Type.Neutral:

                switch (t2)
                {
                    case Type.Purple:
                        return 0.1f;
                    case Type.Blue:
                        return 0.1f;
                    case Type.Red:
                        return 0.1f;
                    case Type.Neutral:
                        return 1f;
                }

                break;
        }

        return 0;
    }
}
