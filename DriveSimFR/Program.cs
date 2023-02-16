using DriveSim;

namespace DriveSimFR
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            
            //change to select what mode, electric fields or drivesim
            Application.Run(new ChassisSim());
        }
    }
}