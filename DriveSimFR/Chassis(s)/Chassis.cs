using DriveSimFR;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SimTests")]

/*
 * Chassis interface holds what any type of 4-wheel drive chassis should have. All calculations
 * are made under presumption that 0 of a heading is Vectoring up on x,y cartesian system.
 * **/
public abstract class Chassis
{
    protected static readonly int NUM_WHEELS = 4;
    //pixels-per-second
    public readonly double MAX_SPEED;
    public static readonly double K_ROT;
    protected readonly double wheelLength;
    protected readonly double WHEEL_PROP;
    protected readonly double K_FRIC;
    protected readonly double LOW_CUTOFF = 0.1;
    protected readonly double MASS;
    // in radians, 0 is straight up
    protected double[] wheelDirections;
    //will have each wheel correspond with the index in wheelDirection. position will be from center:
    //center of chassis will be origin of local coordinate system housing wheel positions.
    protected Vector[,] wheelPositions;
    
    //index of each wheel corresponds to wheel direction&wheelPositions
    protected double[] wheelVelos;
    protected double[] wheelPowers;
    protected Vector position;
    protected double header;
    protected Vector linVelocity;
    protected double angVelocity;
    

    public Chassis(double radius, double[] wheelDirections, Vector[] wheelPositions, Vector position, double WHEELS_PROP, int max_speed = 1, double k_fric = .2, double mass = 1)
    {
        wheelVelos= new double[NUM_WHEELS];
        wheelPowers = new double[NUM_WHEELS];
        this.position = position;
        header = 0;
        this.WHEEL_PROP = WHEELS_PROP;
        wheelLength = radius / WHEEL_PROP;
        this.wheelDirections = new double[NUM_WHEELS];
        linVelocity = new Vector();
        angVelocity = 0;
        MAX_SPEED = max_speed;
        K_FRIC = k_fric;
        MASS= mass;
        if (wheelDirections != null)
        {
            for (int i = 0; i < wheelDirections.GetLength(0); i++)
            {
                this.wheelDirections[i] = wheelDirections[i];
            }
            this.wheelPositions = new Vector[NUM_WHEELS, 3];
           
            for (int i = 0; i < NUM_WHEELS; i++)
            {
                this.wheelPositions[i, 0] = wheelPositions[i];
                this.wheelPositions[i, 1] = wheelPositions[i] + Utils.unitVectorFromTheta(wheelDirections[i]) * wheelLength / 2;
                this.wheelPositions[i, 2] = wheelPositions[i] + Utils.unitVectorFromTheta(wheelDirections[i]) * -wheelLength / 2;
            }
        }
        
    }
    
    /*
     * This method is a secondary constructor so that I can call a constructor inside the actual constructor of sub-classes.
     */
    protected void constructor(double[] wheelDirections, Vector[] wheelPositions, Vector position)
    {
        wheelVelos = new double[NUM_WHEELS];
        wheelPowers = new double[NUM_WHEELS];
        this.position = position;
        header = 0;
        this.wheelDirections = new double[wheelDirections.Length];
        for (int i = 0; i < wheelDirections.GetLength(0); i++)
        {
            this.wheelDirections[i] = wheelDirections[i];
        }
        this.wheelPositions = new Vector[NUM_WHEELS, 3];
        for (int i = 0; i < NUM_WHEELS; i++)
        {
            this.wheelPositions[i, 0] = wheelPositions[i];
            this.wheelPositions[i, 1] = wheelPositions[i] + Utils.unitVectorFromTheta(wheelDirections[i]) * wheelLength / 2;
            this.wheelPositions[i, 2] = wheelPositions[i] + Utils.unitVectorFromTheta(wheelDirections[i]) * -wheelLength / 2;
        }
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
     * with respect to the chassis position and header. each wheel will have 2 Vectors, so a line
     * can be drawn in between them.
     */
    public Vector[,] getGlobalWheelPositions()
    {
        Vector[,] globals = new Vector[NUM_WHEELS, 3];

        for (int i = 0; i<globals.GetLength(0); i++)
        {
            for(int r = 1; r < wheelPositions.GetLength(1); r++)
            {
                globals[i, r].x = position.x + (wheelPositions[i, r].x * Math.Cos(-header) - wheelPositions[i, r].y * Math.Sin(-header));
                globals[i, r].y = position.y + (wheelPositions[i, r].x * Math.Sin(-header) + wheelPositions[i, r].y * Math.Cos(-header));
            }
            globals[i, 0] = Vector.avg(globals[i, 1], globals[i, 2]);
        }
        return globals;
    }

    public Vector[,] getWheelPositions() { return wheelPositions; }

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
    public Vector getLinVelo()
    {
        Vector deltaV = new Vector();
        for(int i = 0; i < wheelVelos.GetLength(0);i++) 
        {
            Vector adder = getVeloVector(wheelVelos[i], wheelDirections[i]);
            deltaV += adder;

        }
        return deltaV;
    }

    /*
     * this will calculate the angular velocity of the chassis given the state of the chassis.
     * positive is turning ccw, negative is cw.
     */
    public double getAngVelo()
    {
        double deltaO = 0;
        for (int i = 0; i < wheelVelos.GetLength(0); i++)
        {
            double angTo = wheelDirections[i] - Utils.angleToVector(wheelPositions[i, 0]);
            deltaO += angTo * wheelVelos[i] * K_ROT / wheelPositions[i, 0].dist();
        }
        return deltaO;
    }
    /*
     * Returns a Vector in the global coordinate system given a Vector in the chassis coordinate
     * system.
     * 
     * @param local: a Vector in the chassis coordinate systme.
     * @return: a Vector in the global system.
     */
    public Vector chassisToGlobal(Vector local)
    {
        Vector global = new Vector(position);

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
    private Vector getVeloVector(double magnitude, double heading)
    {
        Vector adder = new Vector(magnitude);
        adder.x *= -Math.Sin(heading);
        adder.y *= Math.Cos(heading);
        return adder;
    }

    /*
     * Returns position of center of this robot globally
     */
    public Vector getPos()
    {
        return position;
    }

    /*
     * Returns heading of robot; 0 is straight up towards Y-axis.
     */
    public double getHeading()
    {
        return header;
    }

    /*
     * Sets heading to the specified angle
     */
    public void setHeading(double heading)
    {
        header = heading;
    }
    /*
     * Returns array of wheel velocities;
     */
    public double[] getWheelVelos()
    {
        return wheelVelos;
    }
    
}