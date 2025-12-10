using System;
using System.Windows.Forms;
using Vehicle_Rental_Management_System.Forms;

namespace Vehicle_Rental_Management_System
{
    internal static class Program
    {
        // Add these static properties to store logged-in user info
        public static string CurrentUsername { get; set; }
        public static string CurrentUserRole { get; set; }
        public static int CurrentUserId { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Start with Welcome form
            Application.Run(new Welcome());
        }
    }
}