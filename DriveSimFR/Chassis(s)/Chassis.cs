using DriveSimFR;
using System.Runtime.CompilerServices;
using System.Threading;
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
    public static readonly double K_ROT = 10;
    protected readonly double wheelLength;
    protected readonly double WHEEL_PROP;
    protected readonly double K_FRIC_FOR;
    protected readonly double K_FRIC_LAT;
    protected readonly double LOW_CUTOFF = 0.1;
    protected readonly double MASS;
    // in radians, 0 is straight up
    protected double[] wheelDirections;
    //will have each wheel correspond with the index in wheelDirection. position will be from center:
    //center of chassis will be origin of local coordinate system housing wheel positions.
    protected Vector[,] wheelPositions;
    Vector[,] globals;
    //index of each wheel corresponds to wheel direction&wheelPositions
    protected double[,] wheelVelos;
    protected double[] wheelPowers;
    protected Vector position;
    protected double header;
    protected Vector linVelocity;
    protected double angVelocity;
    protected Vector deltaA;

    private Mutex mut;
    

    public Chassis(double radius, double[] wheelDirections, Vector[] wheelPositions, Vector position, double WHEELS_PROP, int max_speed = 1, double k_fric_for = .2, double k_fric_lat = .2, double mass = 1)
    {
        wheelVelos= new double[NUM_WHEELS,2];
        wheelPowers = new double[NUM_WHEELS];
        this.position = position;
        header = 0;
        this.WHEEL_PROP = WHEELS_PROP;
        wheelLength = radius / WHEEL_PROP;
        this.wheelDirections = new double[NUM_WHEELS];
        linVelocity = new Vector();
        angVelocity = 0;
        MAX_SPEED = max_speed;
        K_FRIC_FOR = k_fric_for;
        K_FRIC_LAT = k_fric_lat;

        MASS = mass;
        mut = new Mutex();
        globals = new Vector[NUM_WHEELS, 3];
        deltaA = new Vector();
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
        wheelVelos = new double[NUM_WHEELS,2];
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
            this.wheelPowers[i] = wheelPowers[i];
        }
    }

    /*
     * This method will return the position of wheels on chassis in the global coordinate system,
     * with respect to the chassis position and header. each wheel will have 2 Vectors, so a line
     * can be drawn in between them.
     */
    public Vector[,] getGlobalWheelPositions()
    {
        for (int i = 0; i<globals.GetLength(0); i++)
        {
            for(int r = 0; r < wheelPositions.GetLength(1); r++)
            {
                globals[i, r] = chassisToGlobal(wheelPositions[i, r]);
            }
        }
        return globals;
    }


    /*
     * This will calculate out the next position given the current state of the chassis as well as
     * time gone forward. Don't want to solve differential equations so this will be used like Euler's method 
     * with small step sizes and assuming constant acceleration in between steps(newtonian mechanics).
     */
    public void step(double time)
    {
        if (time < 0)
        {
            throw new Exception("negative time");
        }
        Vector accel = Utils.rotateVector(header,getNetAccel());
        double alpha = getNetAlpha();
        using (mut)
        {
            mut = new Mutex();
            mut.WaitOne();
            position.x += linVelocity.x * time + .5 * accel.x * time * time;
            position.y += linVelocity.y * time + .5 * accel.y * time * time;
            linVelocity.x += accel.x * time;
            linVelocity.y += accel.y * time;
            header = Utils.mod2PI(header + angVelocity * time + .5 * alpha * time * time);
            angVelocity += alpha * time;
            calcWheelVelos();
            mut.ReleaseMutex();

        }
        

    }

    /*
     * this will calculate the linear velocity of the chassis given the state of the chassis, in
     * the local coordinate system of the chassis.
     */
    public Vector getNetAccel()
    {
        deltaA.x = 0;
        deltaA.y = 0;
        for(int i = 0; i < wheelVelos.GetLength(0);i++) 
        {
            deltaA += getVeloVector((wheelPowers[i]) * MAX_SPEED * K_FRIC_FOR - wheelVelos[i, 0] * K_FRIC_FOR, wheelDirections[i]); ;
            deltaA += getVeloVector(-wheelVelos[i, 1] * K_FRIC_LAT, wheelDirections[i]-Math.PI/2); ;
        }
        return deltaA/MASS;
    }

    /*
     * this will calculate the angular velocity of the chassis given the state of the chassis.
     * positive is turning ccw, negative is cw.
     */
    public double getNetAlpha()
    {
        double deltaO = 0;
        for (int i = 0; i < wheelVelos.GetLength(0)&&wheelPositions[i,0].dist()!=0; i++) 
        {
            if(wheelPositions[i, 0].dist() != 0)
            {
                double angTo = Utils.mod2PI(wheelDirections[i] - Utils.angleToVector(wheelPositions[i, 0]));
                deltaO += (wheelPowers[i] * MAX_SPEED * K_FRIC_FOR - wheelVelos[i, 0] * K_FRIC_FOR) * Math.Sin(angTo) / wheelPositions[i, 0].dist() * K_ROT / MASS;
                deltaO += (wheelVelos[i, 1] * K_FRIC_LAT) * Math.Cos(angTo) / wheelPositions[i, 0].dist() * K_ROT / MASS;
            }
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

        global.x += local.x * Math.Cos(header) - local.y * Math.Sin(header);
        global.y += local.x * Math.Sin(header) + local.y * Math.Cos(header);
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
        return Utils.unitVectorFromTheta(heading)*magnitude;
    }

    /*
     * Given the state of the chassis (angular and linear velocities), returns the linear speed of each
     * wheel.
     */
    protected void calcWheelVelos()
    {
        for(int i = 0; i<wheelVelos.GetLength(0); i++)
        {
            double angTo = wheelDirections[i] - Utils.angleToVector(wheelPositions[i, 0]);
            wheelVelos[i,0] = Math.Sin(angTo) * angVelocity * wheelPositions[i, 0].dist();
            wheelVelos[i, 1] = Math.Cos(angTo) * -angVelocity * wheelPositions[i, 0].dist(); //ccw is positive angle velocity, to the right of the wheel should be positive direction of omni-wheel spin
            if (linVelocity.dist() > 0)
            {
                double linTo = Utils.mod2PI(wheelDirections[i]+header) - Utils.angleToVector(linVelocity); //Adds heading to wheelDirection to account for fact that linVelocity is in global coordinate system
                wheelVelos[i,0] += Math.Cos(linTo) * linVelocity.dist();
                wheelVelos[i,1] += Math.Sin(linTo) * linVelocity.dist();

            }
        }
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
    public void setPos(Vector pos)
    {
        this.position = pos;
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
    public double[,] getWheelVelos()
    {
        return wheelVelos;
    }

    /*
     * Returns linear velocity; in globabl coordinates.
     */
    public Vector getLinVelo()
    {
        return linVelocity;
    }

    /*
     * Returns angular velocity; positive number is ccw
     */
    public double getAngVelo()
    {
        return angVelocity;
    }

    /*
     * Returns array of wheel positions;
     */
    public Vector[,] getWheelPositions() { return wheelPositions; }

    /*
     * Adjusts state of chassis by the specified velocities.
     */
    public void setVelos(Vector linVelo, double angVelo)
    {
        linVelocity = linVelo;
        angVelocity = angVelo;
        calcWheelVelos();
    }

    /*
     * Returns line on chassis pointing in direction of heading
     */
    public abstract Vector[] getHeaderLine();

    /*
     * Returns the array holding the points of the body
     */
    public abstract Vector[] getBody();



}