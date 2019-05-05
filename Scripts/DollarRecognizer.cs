// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 02-19-2019
//
// Last Modified By : zaviy
// Last Modified On : 04-01-2019
// ***********************************************************************
// <copyright file="DollarRecognizer.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class DollarRecognizer. Implementation of the $1 recognizer.
/// </summary>
public class DollarRecognizer : MonoBehaviour
{
    /// <summary>
    /// Class Unistroke.
    /// </summary>
    public class Unistroke
    {
        /// <summary>
        /// The example index
        /// </summary>
        public int ExampleIndex;
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }
        /// <summary>
        /// Gets the points.
        /// </summary>
        /// <value>The points.</value>
        public Vector2[] Points { get; private set; }
        /// <summary>
        /// Gets the angle.
        /// </summary>
        /// <value>The angle.</value>
        public float Angle { get; private set; }
        /// <summary>
        /// Gets the vector.
        /// </summary>
        /// <value>The vector.</value>
        public List<float> Vector { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Unistroke"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="points">The points.</param>
        public Unistroke(string name, IEnumerable<Vector2> points)
        {
            Name = string.Intern(name);
            Vector2[] tmp = (Vector2[])points;
            Points = DollarRecognizer.resample(points, _kNormalizedPoints);
            Angle = DollarRecognizer.indicativeAngle(Points);
            DollarRecognizer.rotateBy(Points, -Angle);
            DollarRecognizer.scaleTo(Points, _kNormalizedSize);
            DollarRecognizer.translateTo(Points, Vector2.zero);
            Vector = DollarRecognizer.vectorize(Points);
  
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("{0} #{1}", Name, ExampleIndex);
        }
    }

    /// <summary>
    /// Struct Result
    /// </summary>
    public struct Result
    {
        /// <summary>
        /// The match
        /// </summary>
        public Unistroke Match;
        /// <summary>
        /// The score
        /// </summary>
        public float Score;
        /// <summary>
        /// The angle
        /// </summary>
        public float Angle;

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> struct.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <param name="score">The score.</param>
        /// <param name="angle">The angle.</param>
        public Result(Unistroke match, float score, float angle)
        {
            Match = match;
            Score = score;
            Angle = angle;
        }

        /// <summary>
        /// Gets none.
        /// </summary>
        /// <value>The none.</value>
        public static Result None
        {
            get
            {
                return new Result(null, 0, 0);
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("{0} @{2} ({1})", Match, Score, Angle);
        }
    }

    /// <summary>
    /// Enumerates the gestures.
    /// </summary>
    /// <returns>System.String[].</returns>
    public string[] EnumerateGestures()
    {
        List<string> result = new List<string>();

        for (int i = 0; i < _library.Count; i++)
        {
            if (!result.Contains(_library[i].Name))
                result.Add(_library[i].Name);
        }

        return result.ToArray();
    }


    /// <summary>
    /// The k normalized points
    /// </summary>
    protected const int _kNormalizedPoints = 128;  //64
    /// <summary>
    /// The k normalized size
    /// </summary>
    protected const float _kNormalizedSize = 256.0f;  //256
    /// <summary>
    /// The k angle range
    /// </summary>
    protected const float _kAngleRange = 45.0f * Mathf.Deg2Rad;
    /// <summary>
    /// The k angle precision
    /// </summary>
    protected const float _kAnglePrecision = 2.0f * Mathf.Deg2Rad;
    /// <summary>
    /// The k diagonal
    /// </summary>
    protected static readonly float _kDiagonal = (Vector2.one * _kNormalizedSize).magnitude;
    /// <summary>
    /// The k half diagonal
    /// </summary>
    protected static readonly float _kHalfDiagonal = _kDiagonal * 0.5f;

    /// <summary>
    /// The library
    /// </summary>
    protected List<Unistroke> _library;
    /// <summary>
    /// The library index
    /// </summary>
    protected Dictionary<string, List<int>> _libraryIndex;

    /// <summary>
    /// Initializes a new instance of the <see cref=".DollarRecognizer"/> class.
    /// </summary>
    public DollarRecognizer()
    {
        _library = new List<Unistroke>();
        _libraryIndex = new Dictionary<string, List<int>>();
    }

    /// <summary>
    /// Saves the pattern.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="points">The points.</param>
    /// <returns>Unistroke.</returns>
    public Unistroke SavePattern(string name, IEnumerable<Vector2> points)
    {
        Unistroke stroke = new Unistroke(name, points);

        int index = _library.Count;
        _library.Add(stroke);

        List<int> examples = null;
        if (_libraryIndex.ContainsKey(stroke.Name))
            examples = _libraryIndex[stroke.Name];
        if (examples == null)
        {
            examples = new List<int>();
            _libraryIndex[stroke.Name] = examples;
        }
        stroke.ExampleIndex = examples.Count;
        examples.Add(index);

        return stroke;
    }

    /// <summary>
    /// Recognizes the specified points.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <returns>Result.</returns>
    public Result Recognize(IEnumerable<Vector2> points)
    {
        //print("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<RECOGNIZE GESTURE>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
        Vector2[] working = resample(points, _kNormalizedPoints);

        float angle = indicativeAngle(working);
        ////print("ANGLE = " + -angle);
        rotateBy(working, -angle);
       // //print("ROTATED VECTORS");
        foreach (Vector2 f in working)
        {
        //    //print(f);
        }
        scaleTo(working, _kNormalizedSize);
       // //print("SCALED VECTORS");
        foreach (Vector2 f in working)
        {
           // //print(f);
        }
        translateTo(working, Vector2.zero);
       // //print("TRANSLATED VECTORS");
        foreach (Vector2 f in working)
        {
          //  //print(f);
        }
        List<float> v = vectorize(working);

        float bestDist = float.PositiveInfinity;
        int bestIndex = -1;
      //  print("library count : " + _library.Count);

        for (int i = 0; i < _library.Count; i++)
        {
            //print("GESTURE CHECK : " + _library[i].Name);
            float dist = optimalCosineDistance(_library[i].Vector, v);
           // print(dist + " > " + _library[i].Name);
            if (System.Single.IsNaN(dist)) dist = 0;
            if (bestDist == float.PositiveInfinity)
            {
                //print("best dist is infinity");
                bestDist = dist;
                //print(bestDist);
                bestIndex = i;
            }
            else
            {
                if (dist < bestDist)
                {
                    bestDist = dist;
                    //print("new best dist : " + bestDist);
                    bestIndex = i;
                }
            }
        }

        if (bestIndex < 0)
            return Result.None;
        else
            return new Result(_library[bestIndex], bestDist, (_library[bestIndex].Angle - angle) * Mathf.Rad2Deg);
    }

    /// <summary>
    /// Resamples the specified points.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <param name="targetCount">The target count.</param>
    /// <returns>Vector2[].</returns>
    protected static Vector2[] resample(IEnumerable<Vector2> points, int targetCount)
    {
        
        List<Vector2> result = new List<Vector2>();
  

        float interval = pathLength(points) / (targetCount );
        //print("INTERVAL - " + interval);
        float accumulator = 0;

        Vector2 previous = Vector2.zero;

        IEnumerator<Vector2> stepper = points.GetEnumerator();

        Vector2[] temp = (Vector2[])points;
        //print("PASSED POINTS BEFORE RESAMPLE: " + temp.Length);
        //print(stepper.MoveNext());
      
        for(int i = 1; i < temp.Length; i++)
        {
            Vector2 currentLength = temp[i - 1] - temp[i];
            if(accumulator + currentLength.magnitude >= interval)
            {
             
                float qx = temp[i - 1].x + ((interval - accumulator) / currentLength.magnitude * (temp[i].x - temp[i - 1].x));
                float qy = temp[i - 1].y + ((interval - accumulator) / currentLength.magnitude * (temp[i].y - temp[i - 1].y));
                Vector2 qxy = new Vector2(qx, qy);
                result.Add(qxy);
                List<Vector2> t = new List<Vector2>(temp);
                t.Insert(i, qxy);
                temp = t.ToArray();
                accumulator = 0;
            }
            else
            {
             
                accumulator += currentLength.magnitude;
            }
        }
        //print("PASSED POINTS AFTER RESAMPLE: " + result.Count);
        if (result.Count == targetCount-1)
            {
                // sometimes we fall a rounding-error short of adding the last point, so add it if so
                result.Add(previous);
            }

        return result.ToArray();
    }

    /// <summary>
    /// Centroids the specified points.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <returns>Vector2.</returns>
    protected static Vector2 centroid(Vector2[] points)
    {
        Vector2 result = Vector2.zero;

        for (int i = 0; i < points.Length; i++)
        {
            result += points[i];
        }

        result = result / (float)points.Length;
        return result;
    }

    /// <summary>
    /// Indicatives the angle.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <returns>System.Single.</returns>
    protected static float indicativeAngle(Vector2[] points)
    {
        Vector2 delta = centroid(points) - points[0];
        return Mathf.Atan2(delta.y, delta.x);
    }

    /// <summary>
    /// Rotates the by.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <param name="angle">The angle.</param>
    protected static void rotateBy(Vector2[] points, float angle)
    {
        Vector2 c = centroid(points);
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);

        for (int i = 0; i < points.Length; i++)
        {
            Vector2 delta = points[i] - c;
            points[i].x = (delta.x * cos) - (delta.y * sin);
            points[i].y = (delta.x * sin) + (delta.y * cos);
            points[i] += c;
        }
    }

    /// <summary>
    /// Boundings the box.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <returns>Rect.</returns>
    protected static Rect boundingBox(Vector2[] points)
    {
        Rect result = new Rect();
        result.xMin = float.PositiveInfinity;
        result.xMax = float.NegativeInfinity;
        result.yMin = float.PositiveInfinity;
        result.yMax = float.NegativeInfinity;

        for (int i = 0; i < points.Length; i++)
        {
            result.xMin = Mathf.Min(result.xMin, points[i].x);
            result.xMax = Mathf.Max(result.xMax, points[i].x);
            result.yMin = Mathf.Min(result.yMin, points[i].y);
            result.yMax = Mathf.Max(result.yMax, points[i].y);
        }

        return result;
    }

    /// <summary>
    /// Scales to.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <param name="normalizedSize">Size of the normalized.</param>
    protected static void scaleTo(Vector2[] points, float normalizedSize)
    {
        //print("SCALE PASSED POINTS");
        foreach (Vector2 f in points)
        {
           // //print(f);
        }
        Rect bounds = boundingBox(points);
        //print("SCALE TO METHOD");
        //print(bounds.width);
        //print(bounds.height);
        //Vector2 scale = new Vector2(bounds.width, bounds.height) * (1.0f * normalizedSize);
        for (int i = 0; i < points.Length; i++)
        {
            // points[i].x = points[i].x * scale.x;
            points[i].x = points[i].x * (normalizedSize / bounds.width);
            // points[i].y = points[i].y * scale.y;
            points[i].y = points[i].y * (normalizedSize / bounds.height);
        }
    }

    /// <summary>
    /// Translates to.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <param name="newCentroid">The new centroid.</param>
    protected static void translateTo(Vector2[] points, Vector2 newCentroid)
    {
        Vector2 c = centroid(points);
        Vector2 delta = newCentroid - c;

        for (int i = 0; i < points.Length; i++)
        {
            points[i] = points[i] + delta;
        }
    }

    /// <summary>
    /// Vectorizes the specified points.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <returns>List&lt;System.Single&gt;.</returns>
    protected static List<float> vectorize(Vector2[] points)
    {
        float sum = 0;
        List<float> result = new List<float>();

        for (int i = 0; i < points.Length; i++)
        {
            result.Add(points[i].x);
            result.Add(points[i].y);
            sum += points[i].sqrMagnitude;
        }

        float mag = Mathf.Sqrt(sum);
        for (int i = 0; i < result.Count; i++)
        {
            result[i] /= mag;
        }

        return result;
    }

    /// <summary>
    /// Optimals the cosine distance.
    /// </summary>
    /// <param name="v1">The v1.</param>
    /// <param name="v2">The v2.</param>
    /// <returns>System.Single.</returns>
    protected static float optimalCosineDistance(List<float> v1, List<float> v2)
    {


        if (v1.Count != v2.Count)
        {

            return float.NaN;
        }

        
        float a = 0;
        float b = 0;

        for (int i = 0; i < v1.Count; i += 2)
        {
            a += (v1[i] * v2[i]) + (v1[i + 1] * v2[i + 1]);
            b += (v1[i] * v2[i + 1]) - (v1[i + 1] * v2[i]);
        }

        
        float angle = Mathf.Atan(b / a);
        float result = Mathf.Acos((a * Mathf.Cos(angle)) + (b * Mathf.Sin(angle)));

        return result;
    }

    /// <summary>
    /// Distances at angle.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <param name="test">The test.</param>
    /// <param name="angle">The angle.</param>
    /// <returns>System.Single.</returns>
    protected static float distanceAtAngle(Vector2[] points, Unistroke test, float angle)
    {
        Vector2[] rotated = new Vector2[points.Length];
        rotateBy(rotated, angle);
        return pathDistance(rotated, test.Points);
    }

    /// <summary>
    /// Pathes the distance.
    /// </summary>
    /// <param name="pts1">The PTS1.</param>
    /// <param name="pts2">The PTS2.</param>
    /// <returns>System.Single.</returns>
    protected static float pathDistance(Vector2[] pts1, Vector2[] pts2)
    {
        if (pts1.Length != pts2.Length)
            return float.NaN;

        float result = 0;
        for (int i = 0; i < pts1.Length; i++)
        {
            result += (pts2[i] - pts1[i]).magnitude;
        }

        return result / (float)pts1.Length;
    }

    /// <summary>
    /// Pathes the length.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <returns>System.Single.</returns>
    protected static float pathLength(IEnumerable<Vector2> points)
    {
        float result = 0;
        Vector2 previous = new Vector2(0, 0); 

        bool first = true;
        foreach (Vector2 point in points)
        {
            if (first)
                first = false;
            else
            {
                try { 
                result += (point - previous).magnitude;
                }
                catch (System.Exception e)
                {

                }
            }

            previous = point;
        }

        return result;
    }
}