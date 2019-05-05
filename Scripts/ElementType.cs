// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 03-29-2019
//
// Last Modified By : zaviy
// Last Modified On : 03-29-2019
// ***********************************************************************
// <copyright file="ElementType.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
/// <summary>
/// Class ElementType.
/// </summary>
public class ElementType {

    /// <summary>
    /// Enum Type. Holds all element type, and default time neutral.
    /// </summary>
    public enum Type {Purple, Blue, Red, Neutral}

    /// <summary>
    /// Gets the damage modifier.
    /// </summary>
    /// <param name="t1">The t1.</param>
    /// <param name="t2">The t2.</param>
    /// <returns>System.Single.</returns>
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
