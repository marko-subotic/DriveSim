using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SimTests")]

/*
 *Chassis interface holds what any type of 4-wheel drive chassis should have
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
    protected double[,,] wheelPositions;
    //for now radius is not necessary here, should be used to position the wheels for ordinary
    //chassis such as tank drive or X-drive.
    protected double radius;
    //index of each wheel corresponds to wheel direction&wheelPositions
    protected double[] wheelVelos;
    protected double[] wheelPowers;
    protected double frictionCoeff;
    protected double[] position;
    protected double header;
    protected double wheelLength;

    public Chassis(int radius)
    {
        wheelVelos = new double[NUM_WHEELS];
        wheelPowers = new double[NUM_WHEELS];
        wheelPositions = new double[NUM_WHEELS, 2,2];
        wheelDirections = new double[NUM_WHEELS];
        this.radius = radius;
        position = new double[2];
        wheelLength = radius / WHEEL_PROP;
        header = 0;
    }

    /*
     *This method takes input and sets the internal wheelpowers to input with some scaling.
     *@param wheelPowers: array with each wheel having an input from -1 to 1. Chassis will
     *internally scale according to constants.
     *
     */
    public void inputWheelPowers(double[] wheelPowers)
    {
        for(int i =0; i < NUM_WHEELS; i++)
        {
            this.wheelPowers[i] = wheelPowers[i]*MAX_SPEED;
        }
    }

    /*
     * This method will return the position of wheels on chassis in the global coordinate system,
     * with respect to the chassis position and header. each wheel will have 2 points, so a line
     * can be drawn in between them.
     */
    public double[,,] getGlobalWheelPositions()
    {
        double[,,] globals = new double[NUM_WHEELS, 2,2];

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

}