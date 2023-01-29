using DriveSim;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SimTests")]

/*
 * Chassis interface holds what any type of 4-wheel drive chassis should have. All calculations
 * are made under presumption that 0 of a heading is pointing up on x,y cartesian system.
 * **/
public abstract class Chassis
{
    private readonly int NUM_WHEELS = 4;
    private readonly double WHEEL_PROP = 5;
    public static readonly double MAX_SPEED = 20;
    // in radians, 0 is straight up
    protected double[] wheelDirections;
    //will have each wheel correspond with the index in wheelDirection. position will be from center:
    //center of chassis will be origin of local coordinate system housing wheel positions.
    protected Point[,] wheelPositions;
    //for now radius is not necessary here, should be used to position the wheels for ordinary
    //chassis such as tank drive or X-drive.
    protected double radius;
    //index of each wheel corresponds to wheel direction&wheelPositions
    protected double[] wheelVelos;
    protected double[] wheelPowers;
    protected double frictionCoeff;
    protected Point position;
    protected double header;
    protected double wheelLength;

    public Chassis(int radius)
    {
        wheelVelos = new double[NUM_WHEELS];
        wheelPowers = new double[NUM_WHEELS];
        wheelPositions = new Point[NUM_WHEELS,2];
        wheelDirections = new double[NUM_WHEELS];
        this.radius = radius;
        position = new Point();
        wheelLength = radius / WHEEL_PROP;
        header = 0;
    }

    /*
     *This method takes input and sets the internal wheelpowers to input with some scaling.
     *
     *@throws Exception if any power given is over magnitude of 1
     *
     *@param wheelPowers: array with each wheel having an input from -1 to 1. 
     */
    public void inputWheelPowers(double[] wheelPowers)
    {
        for(int i =0; i < NUM_WHEELS; i++)
        {
            if (Math.Abs(wheelPowers[i]) > 1) 
            {
                throw new Exception("Too much power");
            }
            this.wheelPowers[i] = wheelPowers[i]*MAX_SPEED;
        }
    }

    /*
     * This method will return the position of wheels on chassis in the global coordinate system,
     * with respect to the chassis position and header. each wheel will have 2 points, so a line
     * can be drawn in between them.
     */
    public Point[,] getGlobalWheelPositions()
    {
        Point[,] globals = new Point[NUM_WHEELS, 2];

        for (int i = 0; i<globals.GetLength(0); i++)
        {
            for(int r = 0; r < globals.GetLength(1); r++)
            {
                globals[i, r].x = position.x + (wheelPositions[i, r].x * Math.Cos(-header) - wheelPositions[i, r].y * Math.Sin(-header));
                globals[i, r].y = position.y + (wheelPositions[i, r].x * Math.Sin(-header) + wheelPositions[i, r].y * Math.Cos(-header));

            }
        }
        return globals;
    }

    /*
     * This will calculate out the next position given the current state of the chassis as well as
     * time gone forward. Don't want to solve differential equations so this will be used kind of
     * like Euler's method with small step sizes.
     */
    public void step(double time)
    {

    }

    /*
     * this will calculate the linear velocity of the chassis given the state of the chassis, in
     * the local coordinate system of the chassis.
     */
    public Point getLinVelo()
    {
        Point deltaV = new Point();
        for(int i = 0; i < wheelVelos.GetLength(0);i++) 
        {
            Point adder = getVeloVector(wheelVelos[i], wheelDirections[i]);
            deltaV += adder;

        }
        return deltaV;
    }

    /*
     * this will calculate the angular velocity of the chassis given the state of the chassis.
     */
    public Point getAngVelo()
    {
        Point deltaV = new Point();
        for (int i = 0; i < wheelVelos.GetLength(0); i++)
        {

        }
        return deltaV;
    }
    /*
     * Returns a point in the global coordinate system given a point in the chassis coordinate
     * system.
     * 
     * @param local: a point in the chassis coordinate systme.
     * @return: a point in the global system.
     */
    public Point chassisToGlobal(Point local)
    {
        Point global = new Point(position);

        global.x += local.x * Math.Cos(-header) -local.y * Math.Sin(-header);
        global.y += local.x * Math.Sin(-header) + local.y * Math.Cos(-header);
        return global;
    }

    /*
     * Returns a vector of velocity given a direction and magnitude.
     * 
     * 
     * @param magnitude: magnitude of velocity
     * @param heading: direction of velocity
     * **/
    private Point getVeloVector(double magnitude, double heading)
    {
        Point adder = new Point(magnitude);
        adder.x *= -Math.Sin(heading);
        adder.y *= Math.Cos(heading);
        return adder;
    }


    /*
     * Similar to atan2 function, but returns an angle according to theta = 0
     * meaning the point is directly above the origin and positive direction of theta being ccw.
     * **/
    public static double angleToPoint(Point target)
    {
        if (target.y == 0)
        {
            if (target.x == 0)
            {
                throw new Exception("Illegal Argument");
            }
            else if (target.x > 0)
            {
                return Math.PI * 3 / 2;
            }
            else if (target.x < 0)
            {
                return Math.PI / 2;
            }
        }
        double startAngle = Math.Abs(Math.Atan(target.x / target.y));
        if (target.y > 0)
        {
            if (target.x <= 0)
            {
                return startAngle;
            }
            else
            {
                return 2 * Math.PI - startAngle;
            }
        }
        else
        {
            if (target.x >= 0)
            {
                return Math.PI + startAngle;
            }
            else
            {
                return Math.PI - startAngle;
            }
        }
    }
}