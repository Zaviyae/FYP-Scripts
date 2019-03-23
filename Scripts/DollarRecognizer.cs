﻿/**
* The $1 Unistroke Recognizer
*
*	Jacob O. Wobbrock, Ph.D.
* 	The Information School
*	University of Washington
*	Seattle, WA 98195-2840
*	wobbrock@uw.edu
*
*	Andrew D. Wilson, Ph.D.
*	Microsoft Research
*	One Microsoft Way
*	Redmond, WA 98052
*	awilson@microsoft.com
*
*	Yang Li, Ph.D.
*	Department of Computer Science and Engineering
* 	University of Washington
*	Seattle, WA 98195-2840
* 	yangli@cs.washington.edu
*
* The academic publication for the $1 recognizer, and what should be 
* used to cite it, is:
*
*	Wobbrock, J.O., Wilson, A.D. and Li, Y. (2007). Gestures without 
*	  libraries, toolkits or training: A $1 recognizer for user interface 
*	  prototypes. Proceedings of the ACM Symposium on User Interface 
*	  Software and Technology (UIST '07). Newport, Rhode Island (October 
*	  7-10, 2007). New York: ACM Press, pp. 159-168.
*
* The Protractor enhancement was separately published by Yang Li and programmed 
* here by Jacob O. Wobbrock:
*
*	Li, Y. (2010). Protractor: A fast and accurate gesture
*	  recognizer. Proceedings of the ACM Conference on Human
*	  Factors in Computing Systems (CHI '10). Atlanta, Georgia
*	  (April 10-15, 2010). New York: ACM Press, pp. 2169-2172.
*
* This software is distributed under the "New BSD License" agreement:
*
* Copyright (C) 2007-2012, Jacob O. Wobbrock, Andrew D. Wilson and Yang Li.
* All rights reserved.
*
* Redistribution and use in source and binary forms, with or without
* modification, are permitted provided that the following conditions are met:
*    * Redistributions of source code must retain the above copyright
*      notice, this list of conditions and the following disclaimer.
*    * Redistributions in binary form must reproduce the above copyright
*      notice, this list of conditions and the following disclaimer in the
*      documentation and/or other materials provided with the distribution.
*    * Neither the names of the University of Washington nor Microsoft,
*      nor the names of its contributors may be used to endorse or promote
*      products derived from this software without specific prior written
*      permission.
*
* THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS
* IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
* THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
* PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL Jacob O. Wobbrock OR Andrew D. Wilson
* OR Yang Li BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY,
* OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
* SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
* INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
* STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
* OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
**/

/**
 * Adapted for C# in Unity by Stephen Beeman
 * 
 * The modifications made to the original work are copyright (c) 2014 Gizmocracy LLC
 * and are made available under the MIT License, as follows:
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DollarRecognizer : MonoBehaviour
{
    public class Unistroke
    {
        public int ExampleIndex;
        public string Name { get; private set; }
        public Vector2[] Points { get; private set; }
        public float Angle { get; private set; }
        public List<float> Vector { get; private set; }

        public Unistroke(string name, IEnumerable<Vector2> points)
        {
            Name = string.Intern(name);
            Vector2[] tmp = (Vector2[])points;
            //print("LENGTH OF POINTS " + tmp.Length);
            Points = DollarRecognizer.resample(points, _kNormalizedPoints);
            //print("LENGTH AFTER RESAMPLE " + Points.Length);
            Angle = DollarRecognizer.indicativeAngle(Points);
            DollarRecognizer.rotateBy(Points, -Angle);
            //print("LENGTH AFTER ROTATION " + Points.Length);
            DollarRecognizer.scaleTo(Points, _kNormalizedSize);
            //print("LENGTH AFTER SCALE " + Points.Length);
            DollarRecognizer.translateTo(Points, Vector2.zero);
            //print("LENGTH AFTER TRASNSLATION " + Points.Length);
            Vector = DollarRecognizer.vectorize(Points);
            //print("LENGTH AFTER VECTORIZE " + Points.Length);
        }

        public override string ToString()
        {
            return string.Format("{0} #{1}", Name, ExampleIndex);
        }
    }

    public struct Result
    {
        public Unistroke Match;
        public float Score;
        public float Angle;

        public Result(Unistroke match, float score, float angle)
        {
            Match = match;
            Score = score;
            Angle = angle;
        }

        public static Result None
        {
            get
            {
                return new Result(null, 0, 0);
            }
        }

        public override string ToString()
        {
            return string.Format("{0} @{2} ({1})", Match, Score, Angle);
        }
    }

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


    protected const int _kNormalizedPoints = 128;  //64
    protected const float _kNormalizedSize = 256.0f;  //256
    protected const float _kAngleRange = 45.0f * Mathf.Deg2Rad;
    protected const float _kAnglePrecision = 2.0f * Mathf.Deg2Rad;
    protected static readonly float _kDiagonal = (Vector2.one * _kNormalizedSize).magnitude;
    protected static readonly float _kHalfDiagonal = _kDiagonal * 0.5f;

    protected List<Unistroke> _library;
    protected Dictionary<string, List<int>> _libraryIndex;

    public DollarRecognizer()
    {
        _library = new List<Unistroke>();
        _libraryIndex = new Dictionary<string, List<int>>();
    }

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

    protected static float indicativeAngle(Vector2[] points)
    {
        Vector2 delta = centroid(points) - points[0];
        return Mathf.Atan2(delta.y, delta.x);
    }

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

    protected static void translateTo(Vector2[] points, Vector2 newCentroid)
    {
        Vector2 c = centroid(points);
        Vector2 delta = newCentroid - c;

        for (int i = 0; i < points.Length; i++)
        {
            points[i] = points[i] + delta;
        }
    }

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

    protected static float optimalCosineDistance(List<float> v1, List<float> v2)
    {

        foreach( float f in v1)
        {
          //  //print(f);
        }

        foreach (float f in v2)
        {
          //  //print(f);
        }
        if (v1.Count != v2.Count)
        {
            //print("WTF!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            //print(v1.Count + " - " + v2.Count);
            return float.NaN;
        }
        else
        {
            //print(v1.Count + " - " + v2.Count);
            //print("IT WAS SAME");
        }
        
        float a = 0;
        float b = 0;

        for (int i = 0; i < v1.Count; i += 2)
        {
            a += (v1[i] * v2[i]) + (v1[i + 1] * v2[i + 1]);
            b += (v1[i] * v2[i + 1]) - (v1[i + 1] * v2[i]);
        }
        //print("A = " + a);
        //print("B = " + b);
        
        float angle = Mathf.Atan(b / a);
        float result = Mathf.Acos((a * Mathf.Cos(angle)) + (b * Mathf.Sin(angle)));
        
        //print("COSIGN RESULT :" + result);
        return result;
    }

    protected static float distanceAtAngle(Vector2[] points, Unistroke test, float angle)
    {
        Vector2[] rotated = new Vector2[points.Length];
        rotateBy(rotated, angle);
        return pathDistance(rotated, test.Points);
    }

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