using System;
using System.Collections.Generic;

class Porgram{
    public static void Main(){
        
        if (testIsOnCircle() == true){
            if(testGetOrthocenter() == true){
                if(testCheckPoints() == true){
                
                }
            }
            
        }
    }

    public static bool testCheckPoints(){
        List<double[]> centerTestValues = new List<double[]> {
            new double[] {0.0, 0.0}, new double[] {2.0, 2.0},
            new double[] {-3.0, -3.0}, new double[] {5.0, 6.0},
            new double[] {0, 0}, new double[] {-0.083, 0.083}
        };

        List<double> radiusTestValues = new List<double>{
            1.0, 2.8284271247461903, 3.605551275463989, 7.810249675906654,
            1.0, 1.369
        };

        List<List<double[]>> testPoints = new List<List<double[]>> {
            new List<double[]> {
                new double[] {0, 1},
                new double[] {(Math.Sqrt(3) / 2), -0.5},
                new double[] {(-Math.Sqrt(3) / 2), -0.5}
            }, new List<double[]> {
                new double[] {0, 0},
                new double[] {1, 1},
                new double[] {2, 2}
            }, new List<double[]> {
                new double[] {0, 1},
                new double[] {1, 0},
                new double[] {0, -1},
                new double[] {-1, 0}
            }, new List<double[]> {
                new double[] {0, 0},
                new double[] {1, 1},
                new double[] {0, 2},
                new double[] {-1, 1}
            }, new List<double[]> {
                new double[] {1, 0},
                new double[] {0.309, 0.951},
                new double[] {-0.809, 0.587},
                new double[] {-0.809, -0.587},
                new double[] {0.309, -0.951}
            }, new List<double[]> {
                new double[] {0, 0},
                new double[] {1, 1},
                new double[] {2, 0},
                new double[] {1, -1},
                new double[] {-1, -1}
            }
        };

        List<bool> output = new List<bool>{
            true, false, true, false, true, false
        };

        NinePointCircleChecker checker;

        for (int i = 0; i < centerTestValues.Count; i++){
            print ($"CheckPoints Test {i+1}: ");
            checker = new NinePointCircleChecker(centerTestValues[i], radiusTestValues[i]);
            bool testValue = checker.CheckPoints(testPoints[i]);
            if (testValue == output[i]){
                print("    Pass.");
            }
            else{
                print($"    Fail.");
                print($"    Testing Input: ");
                print($"        centerTestValues[{i}] == ({centerTestValues[i][0]}, {centerTestValues[i][0]})");
                print($"        radiusTestValues[{i}] == {radiusTestValues[i]}");
                int j = 0;
                foreach (double[] da in testPoints[i]){
                    print($"        Point {j} == ({da[0]}, {da[1]})");
                    j ++;
                }
                print($"        output[{i}] == {output[i]}");
                print($"        TestValue == {testValue}");
                print("");
                print($"    Tolerances: ");
                print($"        radiusTolerance == {checker.radiusTolerance}");
                print($"        pointTolerance == {checker.pointTolerance}");
                print("");
                print($"    Logs:");
                j = 0;
                foreach (string s in checker.CheckPointsLog){
                    print($"        {j}) {s}");
                    j ++;
                }

                
                i = centerTestValues.Count;
                return false;
            }
        }
        print($" === CheckPoints Passes All Tests === ");
        print("");
        return true;
    }


    public static bool testIsOnCircle(){
        List<double[]> centerTestValues = new List<double[]> {
            new double[] {0.0, 0.0}, new double[] {0.0, 0.0},
            new double[] {0.0, 0.0}, new double[] {0.0, 0.0},
            new double[] {0.0, 0.0}
        };

        List<double> radiusTestValues = new List<double>{
            1.0, 1.0, 1.0, 1.0, 1.0
        };

        List<double[]> testPoints = new List<double[]> {
            new double[] {1.0, 0.0 }, new double[] {0.5, 0.5 }, 
            new double[] {2.0, 0.0 }, new double[] {1 - 1e-4, 0.0}, 
            new double[] {1 - 1e-2, 0.0}, 
            
        };

        List<double> testPointTolerances = new List<double>{
            0.001, 0.001, 0.001, 1e-3, 1e-3
        };

        List<bool> output = new List<bool>{
            true, false, false, true, false
        };

        NinePointCircleChecker checker;

        for (int i = 0; i < centerTestValues.Count; i++){

            print($"IsOnCircle Test {i+1}:");
            checker = new NinePointCircleChecker(centerTestValues[i], radiusTestValues[i]);
            checker.pointTolerance = testPointTolerances[i];

            bool testValue = checker.IsOnCircle(testPoints[i]);

            if(testValue == output [i]){
                print("    Pass.");
                print("");
            }
            else{
                print($"    Fail");
                print($"    TestValues:");
                print($"        centerTestValues[{i}] == ({centerTestValues[i][0]}, {centerTestValues[i][0]})");
                print($"        radiusTestValues [{i}] == {radiusTestValues[i]}");
                print($"        testPoints[{i}] == ({testPoints[i][0]}, {testPoints[i][0]})");
                print($"        testPointTolerances[{i}] == {testPointTolerances[i]}");
                print($"        output[{i}] == {output[i]}");
                print($"    Logs:");
                int j = 0;
                foreach (string s in checker.IsOnCircleLog){
                    print($"        {j}) {s}");
                    j ++;
                }

                i = centerTestValues.Count;
                return false;
            }
        }

        print($" === IsOnCircle Passes All Tests === ");
        print("");
        return true;
    }


    public static bool testGetOrthocenter(){
        List<double[]> point1InputValues = new List<double[]>{
            new double[] { 0, 0 }, new double[] { 0, 0 },
            new double[] { 0, 0 }, new double[] { 0, 0 },
            new double[] { 0, 0 }
        };

        List<double[]> point2InputValues = new List<double[]>{
            new double[] { 3, 0 }, new double[] { 3, 0 },
            new double[] { 2, 4 }, new double[] { 2, 4 },
            new double[] { 0, 0 }
        };

        List<double[]> point3InputValues = new List<double[]>{
            new double[] { 1, 2 }, new double[] { 4, 0 },
            new double[] { 4, 0 }, new double[] { 4, 4 },
            new double[] { 0, 0 }
        };

        List<double[]> expectedOutput = new List<double[]>{
            new double[] { 1, 0 }, null, new double[] { 2, 0 },
            new double[] { 2, 0 }, null
        };

        NinePointCircleChecker checker;

        for (int i = 0; i < point1InputValues.Count; i++){
            checker= new NinePointCircleChecker();

            print($"GetOrthocenter Test {i+1}:");

            double[] actualOutput = checker.GetOrthocenter(
                point1InputValues[i], point2InputValues[i], point3InputValues[i]
            );

            bool isPass = true;
            if(actualOutput == null){
                if (expectedOutput[i] == null){
                    isPass = true;
                }
                else{
                    isPass = false;
                }
            }
            else{
                if (Math.Abs(actualOutput[0] - expectedOutput[i][0]) < checker.pointTolerance){
                    isPass = true;
                }
                else{
                    isPass = false;
                }

                if (Math.Abs(actualOutput[1] - expectedOutput[i][1]) < checker.pointTolerance){
                    isPass = true;
                }
                else{
                    isPass = false;
                }
            }
            

           

            if (isPass == true){
                print($"    Pass");
            }
            else{
                print($"    Fail");
                print($"    Test Values:");
                print($"        point1Values[{i}] == ({point1InputValues[i][0]}, {point1InputValues[i][1]})");
                print($"        point2Values[{i}] == ({point2InputValues[i][0]}, {point2InputValues[i][1]})");
                print($"        point3Values[{i}] == ({point3InputValues[i][0]}, {point3InputValues[i][1]})");
                print($"        expectedOutput[{i}] == ({expectedOutput[i][0]}, {expectedOutput[i][1]})");
                if (actualOutput == null){
                    print($"        actualOutput == null");
                }
                else{
                    print($"        actualOutput == ({actualOutput[0]}, {actualOutput[1]})");
                }

                print($"    Logs:");
                int j = 0;
                foreach (string s in checker.GetOrthocenterLog){
                    print($"        {j}) {s}");
                    j ++;
                }

                i = point1InputValues.Count;
                return false;
            }

        }
        print($" === GetOrthocenter Passes All Tests === ");
        print("");
        return true;
    }
    
    public static void print(string msg){
        Console.WriteLine(msg);
    }
}

public class NinePointCircleChecker{
    private double[] center;
    private double radius;

    // Public variables for tolerances
    public double radiusTolerance = 0.001;
    public double pointTolerance = 0.001;

    // the logs
    public List<string> CheckPointsLog = new List<string>();
    
    public List<string> IsOnCircleLog = new List<string>();

    public List<string> GetOrthocenterLog = new List<string>();

    

    // Constructor that takes the equation for the circle and calculates its center and radius
    public NinePointCircleChecker(double[] centerInput, double radiusInput) {
        center = centerInput;
        if (radiusInput <= 0){
            throw new Exception("radius must be reater than 0");
        }
        
        radius = radiusInput;
    }

    public NinePointCircleChecker(){
        
    }

    /// <summary>
    /// Checks whether the given list of points lie on a nine-point circle.
    /// </summary>
    /// <param name="points">The list of points to check.</param>
    /// <returns>True if the points lie on a nine-point circle, false otherwise.</returns>
    /// <author>Generated by ChatGPT</author>
    public bool CheckPoints(List<double[]> points) {
        if (points.Count < 3 || points.Count > 9) {
            // The list of points should have between 3 and 9 elements.
            return false;
        }

        // Check that all the points lie on the circle
        foreach (double[] point in points) {
            if (Math.Pow(point[0] - center[0], 2) + Math.Pow(point[1] - center[1], 2) > 
                Math.Pow(radius + pointTolerance, 2) ||
                Math.Pow(point[0] - center[0], 2) + Math.Pow(point[1] - center[1], 2) < 
                Math.Pow(radius - pointTolerance, 2)) {
                return false;
            }
        }

        // Check that the circle passes through the midpoint of each side of the triangle formed by the points
        double[] midpoint1 = GetMidpoint(points[0], points[1]);
        CheckPointsLog.Add($"midpoint1 == ({midpoint1[0]}, {midpoint1[1]})");
        double[] midpoint2 = GetMidpoint(points[1], points[2]);
        CheckPointsLog.Add($"midpoint2 == ({midpoint2[0]}, {midpoint2[1]})");
        double[] midpoint3 = GetMidpoint(points[2], points[0]);
        CheckPointsLog.Add($"midpoint3 == ({midpoint3[0]}, {midpoint3[1]})");

        if (!IsOnCircle(midpoint1) || !IsOnCircle(midpoint2) || !IsOnCircle(midpoint3)) {
            return false;
        }

        // Check that the circle passes through the foot of each altitude of the triangle formed by the points
        double[] foot1 = GetFoot(points[0], points[1], points[2]);
        CheckPointsLog.Add($"foot1 == ({foot1[0]}, {foot1[1]})");
        double[] foot2 = GetFoot(points[1], points[2], points[0]);
        CheckPointsLog.Add($"foot2 == ({foot2[0]}, {foot2[1]})");
        double[] foot3 = GetFoot(points[2], points[0], points[1]);
        CheckPointsLog.Add($"foot3 == ({foot3[0]}, {foot3[1]})");

        if (!IsOnCircle(foot1) || !IsOnCircle(foot2) || !IsOnCircle(foot3)) {
            return false;
        }

        // Check that the circle passes through the midpoint of the line segment connecting each vertex to the orthocenter
        double[] orthocenter = GetOrthocenter(points[0], points[1], points[2]);
        CheckPointsLog.Add($"orthocenter == ({orthocenter[0]}, {orthocenter[1]})");

        double[] midpoint4 = GetMidpoint(points[0], orthocenter);
        CheckPointsLog.Add($"midpoint4 == ({midpoint4[0]}, {midpoint4[1]})");
        double[] midpoint5 = GetMidpoint(points[1], orthocenter);
        CheckPointsLog.Add($"midpoint5 == ({midpoint5[0]}, {midpoint5[1]})");
        double[] midpoint6 = GetMidpoint(points[2], orthocenter);
        CheckPointsLog.Add($"midpoint6 == ({midpoint6[0]}, {midpoint6[1]})");

        if (!IsOnCircle(midpoint4) || !IsOnCircle(midpoint5) || !IsOnCircle(midpoint6)) {
            return false;
        }

        return true;
    }


    /// <summary>
    /// Determines whether the given point lies on the circle.
    /// </summary>
    /// <param name="point">The point to check.</param>
    /// <returns>True if the point lies on the circle, false otherwise.</returns>
    /// <author>Generated by ChatGPT</author>
    public bool IsOnCircle(double[] point) {
        double distance = GetDistance(center, point);
        IsOnCircleLog.Add($"distance == {distance}");
        return Math.Abs(distance - radius) <= pointTolerance;
    }


    /// <summary>
    /// Calculates the coordinates of the orthocenter of a triangle given its three vertices.
    /// </summary>
    /// <param name="point1">The first vertex of the triangle.</param>
    /// <param name="point2">The second vertex of the triangle.</param>
    /// <param name="point3">The third vertex of the triangle.</param>
    /// <returns>The coordinates of the orthocenter as a double array.</returns>
    /// <author>Generated by ChatGPT</author>
    public double[] GetOrthocenter(double[] point1, double[] point2, double[] point3) {
        if ((point2[1] - point1[1]) * (point3[0] - point2[0]) == (point3[1] - point2[1]) * (point2[0] - point1[0])) {
            return null;
        }
       
        // Calculate the slopes of the two lines that form the two altitudes of the triangle.
        double slope1 = (point2[0] - point1[0]) / (point1[1] - point2[1]);
        GetOrthocenterLog.Add($"slope1 == {slope1}");

        double slope2 = (point3[0] - point2[0]) / (point2[1] - point3[1]);
        GetOrthocenterLog.Add($"slope2 == {slope2}");

        // Check if the slopes are equal, in which case the triangle is degenerate and has no orthocenter.
        if (slope1 == slope2) {
            return null;
        }

        // Calculate the x-coordinate of the orthocenter.
        double  x = ((slope1 * slope2 * (point1[1] - point3[1])) + (slope2 * point1[0]) - (slope1 * point3[0])) / (slope2 - slope1);
        GetOrthocenterLog.Add($"x == {x}");

        // Calculate the y-coordinate of the orthocenter.
        double y = -slope1 * (x - point1[0]) + point1[1];;

        // Return the coordinates of the orthocenter as a double array.
        return new double[] { x, y };
    }

    /// <summary>
    /// Calculates the foot of the perpendicular from a given point to the line passing through two other points.
    /// </summary>
    /// <param name="point1">The first point defining the line.</param>
    /// <param name="point2">The second point defining the line.</param>
    /// <param name="point3">The point to calculate the foot for.</param>
    /// <returns>The foot of the perpendicular from the given point to the line passing through the other two points.</returns>
    /// <author>Generated by ChatGPT</author>
    private double[] GetFoot(double[] point1, double[] point2, double[] point3){
        double[] foot = new double[2];

        // Compute the three sides of the triangle
        double a = GetDistance(point2, point3);
        double b = GetDistance(point1, point3);
        double c = GetDistance(point1, point2);

        // Compute the semiperimeter of the triangle
        double s = (a + b + c) / 2;

        // Compute the area of the triangle
        double area = Math.Sqrt(s * (s - a) * (s - b) * (s - c));

        // Compute the height of the triangle
        double height = 2 * area / c;

        // Compute the distance from point1 to the foot of the altitude
        double distance = Math.Sqrt(Math.Pow(height, 2) - Math.Pow(b / 2, 2));

        // Compute the direction from point1 to point2
        double[] direction = new double[] { point2[0] - point1[0], point2[1] - point1[1] };

        // Compute the unit vector in the direction from point1 to point2
        double[] unitVector = new double[] { direction[0] / c, direction[1] / c };

        // Compute the foot of the altitude
        foot[0] = point1[0] + unitVector[0] * distance;
        foot[1] = point1[1] + unitVector[1] * distance;

        return foot;
    }

    /// <summary>
    /// Calculates the Euclidean distance between two points in the plane.
    /// </summary>
    /// <remarks>
    /// The Euclidean distance between two points (x1, y1) and (x2, y2) is
    /// defined as sqrt((x2 - x1)^2 + (y2 - y1)^2).
    /// </remarks>
    /// <param name="point1">The first point, represented as a two-element array [x, y].</param>
    /// <param name="point2">The second point, represented as a two-element array [x, y].</param>
    /// <returns>The Euclidean distance between the two points.</returns>
    /// <author>Generated by ChatGPT</author>
    private double GetDistance(double[] point1, double[] point2) {
        double x1 = point1[0];
        double y1 = point1[1];
        double x2 = point2[0];
        double y2 = point2[1];
        double deltaX = x2 - x1;
        double deltaY = y2 - y1;
        double distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        return distance;
    }

    /// <summary>
    /// Calculates the midpoint of a line segment defined by two points.
    /// </summary>
    /// <param name="point1">The first point defining the line segment.</param>
    /// <param name="point2">The second point defining the line segment.</param>
    /// <returns>The midpoint of the line segment.</returns>
    /// <author>Generated by ChatGPT</author>
    private double[] GetMidpoint(double[] point1, double[] point2) {
        double[] midpoint = new double[2];
        midpoint[0] = (point1[0] + point2[0]) / 2;
        midpoint[1] = (point1[1] + point2[1]) / 2;
        return midpoint;
    }

}
