using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text.RegularExpressions;

namespace HospitalManagementSystem
{
    /// <summary>
    /// Represents a base abstract class for all users in the hospital system
    /// </summary>
    public abstract class User
    {
        /// <summary>
        /// Gets or sets the user's name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Get ot sets the user's age
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// Gets or sets the user's email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the user's mobile number
        /// </summary>
        public string MobileNumber { get; set; }
        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        private string Password { get; set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="User"/> class. 
        /// </summary>
        /// <param name="name">The user's name</param>
        /// <param name="age">The user's age</param>
        /// <param name="email">The user's email</param>
        /// <param name="mobileNumber">The user's mobile number</param>
        /// <param name="password">The user's password</param>
        public User(string name, int age, string email, string mobileNumber, string password)
        {
            Name = name;
            Age = age;
            Email = email;
            MobileNumber = mobileNumber;
            Password = password;
        }
        /// <summary>
        /// Authenticates the user with the given email and password
        /// </summary>
        /// <param name="email">The user's email address</param>
        /// <param name="password">The user's password</param>
        /// <returns>True if the authentication is successful; otherwise, false</returns>
        public bool Authenticate(string email, string password)
        {
            return Email == email && Password == password;
        }

        /// <summary>
        /// Changes the user's password
        /// </summary>
        /// <param name="newPassword">The new password</param>
        public void ChangePassword(string newPassword)
        {
            Password = newPassword;
        }

        /// <summary>
        /// Displays the user's information
        /// </summary>
        public virtual void DisplayInfo()
        {
            Console.WriteLine("Your details.");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Age: {Age}");
            Console.WriteLine($"Mobile phone: {MobileNumber}");
            Console.WriteLine($"Email: {Email}");
        }

        /// <summary>
        /// Performs user-specific actions within the hospital system
        /// </summary>
        /// <param name="hospital">Hospital instance</param>
        public abstract void PerformActions(Hospital hospital);
    }

    /// <summary>
    /// Represents a patient in the hospital system
    /// </summary>
    public class Patient : User
    {
        /// <summary>
        /// Gets a value indicating whether or not the patient is checked in
        /// </summary>
        public bool IsCheckedIn { get; private set; }

        /// <summary>
        /// Gets the room number assigned to the patient
        /// </summary>
        public string RoomNumber { get; private set; }

        /// <summary>
        /// Gets the floor number of where the patient is located
        /// </summary>
        public string FloorNumber { get; private set; }

        /// <summary>
        /// Gets the name of the surgeon assigned to the patient
        /// </summary>
        public string SurgeonAssigned { get; private set; }
        /// <summary>
        /// Gets the date and time of the patient's surgery
        /// </summary>
        public DateTime? SurgeryDateTime { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the patient's surgery is completed
        /// </summary>
        public bool SurgeryCompleted { get; private set; } = false;

        /// <summary>
        /// Initialises a new instance of the <see cref="Patient"/> class 
        /// </summary>
        /// <param name="name">The patient's name</param>
        /// <param name="age">The patient's age </param>
        /// <param name="email">The patient's email</param>
        /// <param name="mobileNumber">The patient's mobile number</param>
        /// <param name="password">The patient's password</param>
        public Patient(string name, int age, string email, string mobileNumber, string password)
            : base(name, age, email, mobileNumber, password)
        {
        }

        /// <summary>
        /// Checks the patient into the hospital
        /// </summary>
        public void CheckIn()
        {
            IsCheckedIn = true;
        }

        /// <summary>
        /// Marks the patient's surgery as completed
        /// </summary>
        public void MarkSurgeryCompleted()
        {
            SurgeryCompleted = true;
        }

        /// <summary>
        /// Assigns a room to a patient
        /// </summary>
        /// <param name="roomNumber">The room number</param>
        /// <param name="floorNumber">The floor number</param>
        public void AssignRoom(string roomNumber, string floorNumber)
        {
            RoomNumber = roomNumber;
            FloorNumber = floorNumber;
        }

        /// <summary>
        /// Assigns a surgeon and a surgery date/time to the patient
        /// </summary>
        /// <param name="surgeonName">The name of the surgeon</param>
        /// <param name="surgeryDateTime">The date and time of the surgery</param>
        public void AssignSurgeon(string surgeonName, DateTime? surgeryDateTime)
        {
            SurgeonAssigned = surgeonName;
            SurgeryDateTime = surgeryDateTime;
        }

        /// <summary>
        /// Checks the patient out of the hospital
        /// </summary>
        public void CheckOut()
        {
            IsCheckedIn = false;
            SurgeonAssigned = null;
            SurgeryDateTime = null;
            // RoomNumber and FloorNumber remain assigned until unassigned by floor manager
        }

        /// <summary>
        /// Performs patient-specific actions within the hospital system
        /// </summary>
        /// <param name="hospital">Hospital instance</param>
        public override void PerformActions(Hospital hospital)
        {
            while (true)
            {   // Displays menu option to the patient
                Console.WriteLine("Patient Menu.");
                Console.WriteLine("Please choose from the menu below:");
                //Menu options
                Console.WriteLine("1. Display my details");
                Console.WriteLine("2. Change password");
                Console.WriteLine(!IsCheckedIn ? "3. Check in" : "3. Check out");
                Console.WriteLine("4. See room");
                Console.WriteLine("5. See surgeon");
                Console.WriteLine("6. See surgery date and time");
                Console.WriteLine("7. Log out");
                Console.Write($"Please enter a choice between {Program.MIN_MENU_OPTION} and {Program.MAX_MENU_OPTION_PATIENT}.\n");
                Console.WriteLine();
            
                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        DisplayInfo();
                        Console.WriteLine();
                        break;
                    case "2":
                        Console.Write("Enter new password:\n");
                        string newPassword = Console.ReadLine();
                        ChangePassword(newPassword);
                        Console.WriteLine("Password has been changed.\n");
                        break;
                    case "3":
                        if (!IsCheckedIn)
                        {
                            if (SurgeryCompleted)
                            {   //Patient cannot check in after surgery is completed
                                Console.WriteLine("You are unable to check in at this time.\n");
                            }
                            else
                            {   //Patient checks in
                                CheckIn();
                                Console.WriteLine($"Patient {Name} has been checked in.\n");
                            }
                        }
                        else
                        {
                            if (!SurgeryCompleted)
                            {   //Patient cannot check out before surgery is completed
                                Console.WriteLine("You are unable to check out at this time.\n");
                            }
                            else
                            {   //Patient checks out
                                CheckOut();
                                Console.WriteLine($"Patient {Name} has been checked out.\n");
                            }
                        }
                        break;
                    case "4":
                        if (RoomNumber == null)
                        {
                            Console.WriteLine("You do not have an assigned room.\n");
                        }
                        else
                        {
                            Console.WriteLine($"Your room is number {RoomNumber} on floor {FloorNumber}.\n");
                        }
                        break;
                    case "5":
                        if (SurgeonAssigned == null)
                        {
                            Console.WriteLine("You do not have an assigned surgeon.\n");
                        }
                        else
                        {
                            Console.WriteLine($"Your surgeon is {SurgeonAssigned}.\n");
                        }
                        break;
                    case "6":
                        if (SurgeryDateTime == null)
                        {
                            Console.WriteLine("You do not have assigned surgery.\n");
                        }
                        else
                        {
                            Console.WriteLine($"Your surgery time is {SurgeryDateTime:HH:mm dd/MM/yyyy}.\n");
                        }
                        break;
                    case "7":
                        Console.WriteLine($"Patient {Name} has logged out.\n");
                        return;
                    default:
                        UserInputHelper.DisplayError("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Represents an abstract base class for the hospitals staff members
    /// </summary>
    public abstract class Staff : User
    {
        /// <summary>
        /// Gets or sets the staff member's ID
        /// </summary>
        public string StaffID { get; set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="Staff"/> class
        /// </summary>
        /// <param name="name">The staff member's name </param>
        /// <param name="age">The staff member's age</param>
        /// <param name="email">The staff member's  email</param>
        /// <param name="mobileNumber">The staff member's mobile number</param>
        /// <param name="password">The staff member's password</param>
        /// <param name="staffID">The staff member's staffID</param>

        protected Staff(string name, int age, string email, string mobileNumber, string password, string staffID)
            : base(name, age, email, mobileNumber, password)
        {
            StaffID = staffID;
        }

        /// <summary>
        /// Displays the staff member's info
        /// </summary>
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Staff ID: {StaffID}");
        }
    }

    /// <summary>
    /// Represents a floor manager responsible for managing patient rooms and surgeries on a specific floor
    /// </summary>
    public class FloorManager : Staff
    {

        /// <summary>
        /// Gets or sets the floor number managed by the floor manager
        /// </summary>
        public string FloorNumber { get; set; }

        /// <summary>
        /// Initialises new instance of the <see cref="FloorManager"/> class 
        /// </summary>
        /// <param name="name">The floor manager's name</param>
        /// <param name="age">The floor manager's age</param>
        /// <param name="email">The floor manager's email</param>
        /// <param name="mobileNumber">The floor manager's mobile number</param>
        /// <param name="password">The floor manager's password</param>
        /// <param name="staffID">The floor manager's staff ID</param>
        /// <param name="floorNumber">The floor number assigned to the manager </param>
        public FloorManager(string name, int age, string email, string mobileNumber, string password, string staffID, string floorNumber)
            : base(name, age, email, mobileNumber, password, staffID)
        {
            FloorNumber = floorNumber;
        }

        /// <summary>
        /// Assigns room to a patient
        /// </summary>
        /// <param name="patient">The patient to assign a room to</param>
        /// <param name="roomNumber">The room number to assign to the patient</param>
        public void AssignRoomToPatient(Patient patient, string roomNumber)
        {
            if (patient == null)
            {
                Console.WriteLine("Error: Patient not found.");
                return;
            }
            patient.AssignRoom(roomNumber, FloorNumber);
        }

        /// <summary>
        /// Assigns a surgeon to a patient then schedules a surgery
        /// </summary>
        /// <param name="patient">The patient to assign a surgeon to</param>
        /// <param name="surgeon">The surgeon to assign</param>
        /// <param name="surgeryDateTime">The scheduled date and time for surgery</param>
        public void AssignSurgeonToPatient(Patient patient, Surgeon surgeon, DateTime surgeryDateTime)
        {
            if (patient == null)
            {
                Console.WriteLine("Error: Patient not found.");
                return;
            }
            if (surgeon == null)
            {
                Console.WriteLine("Error: Surgeon not found.");
                return;
            }
            patient.AssignSurgeon(surgeon.Name, surgeryDateTime);
            surgeon.AddSurgery(patient, surgeryDateTime);
        }

        /// <summary>
        /// Displays user info
        /// </summary>
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Floor: {FloorNumber}.");
        }

        /// <summary>
        /// Performs floor manager-specific actions within the hospital system
        /// </summary>
        /// <param name="hospital"></param>
        public override void PerformActions(Hospital hospital)
        {
            while (true)
            {
                Console.WriteLine("Floor Manager Menu.");
                Console.WriteLine("Please choose from the menu below:");
                Console.WriteLine("1. Display my details");
                Console.WriteLine("2. Change password");
                Console.WriteLine("3. Assign room to patient");
                Console.WriteLine("4. Assign surgery");
                Console.WriteLine("5. Unassign room");
                Console.WriteLine("6. Log out");
                Console.Write($"Please enter a choice between {Program.MIN_MENU_OPTION} and {Program.MAX_MENU_OPTION_FLOOR_MANAGER}.\n");
                Console.WriteLine();

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        DisplayInfo();
                        Console.WriteLine();
                        break;
                    case "2":
                        Console.Write("Enter new password:\n");
                        string newPassword = Console.ReadLine();
                        ChangePassword(newPassword);
                        Console.WriteLine("Password has been changed.\n");
                        break;
                    case "3":
                        AssignRoomToPatientAction(hospital);
                        break;
                    case "4":
                        AssignSurgeryToPatientAction(hospital);
                        break;
                    case "5":
                        UnassignRoomFromPatientAction(hospital);
                        break;
                    case "6":
                        Console.WriteLine($"Floor manager {Name} has logged out.\n");
                        return;
                    default:
                        UserInputHelper.DisplayError("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private void AssignRoomToPatientAction(Hospital hospital)
        {   //Check if there are any registered patients
            if (hospital.GetAllPatients().Count == 0)
            {
                Console.WriteLine("There are no registered patients.\n");
                return;
            }
            //Get patients who are checked in and without assigned rooms
            var patientsWithoutRooms = hospital.GetAllPatients().Where(p => p.IsCheckedIn && p.RoomNumber == null).ToList();
            if (patientsWithoutRooms.Count == 0)
            {
                Console.WriteLine("There are no checked in patients.\n");
                return;
            }
            //Check if there are available rooms on the floor
            if (hospital.GetAvailableRooms(FloorNumber).Count == 0)
            {
                UserInputHelper.DisplayError("All rooms on this floor are assigned.");
                return;
            }
            //Let the floor manager select a patient from the
            Patient patient = InteractionHelper.SelectFromList(patientsWithoutRooms, "Please select your patient:");
            if (patient == null)
                return;

            string roomNumber = GetValidRoomNumber(hospital);
            if (roomNumber == null)
                return;
            //Assign room to the patient
            AssignRoomToPatient(patient, roomNumber);
            Console.WriteLine($"Patient {patient.Name} has been assigned to room number {roomNumber} on floor {FloorNumber}.\n");
        }

        private string GetValidRoomNumber(Hospital hospital)
        {
            while (true)
            {
                Console.Write($"Please enter your room ({Program.MIN_ROOM_NUMBER}-{Program.MAX_ROOM_NUMBER}):\n");
                string roomNumber = Console.ReadLine();
                Console.WriteLine();
                //Validate the room number input
                if (!int.TryParse(roomNumber, out int roomNum) || roomNum < Program.MIN_ROOM_NUMBER || roomNum > Program.MAX_ROOM_NUMBER)
                {
                    UserInputHelper.DisplayError("Supplied value is out of range, please try again.");
                    continue;
                }
                //Check if the room is already assigned
                else if (!hospital.GetAvailableRooms(FloorNumber).Contains(roomNumber))
                {
                    UserInputHelper.DisplayError("Room has been assigned to another patient, please try again.");
                    continue;
                }
                else
                {   //Valid room number
                    return roomNumber;
                }
            }
        }

        private void AssignSurgeryToPatientAction(Hospital hospital)
        {
            if (hospital.GetAllPatients().Count == 0)
            {
                Console.WriteLine("There are no registered patients.\n");
                return;
            }
            var patientsWithRooms = hospital.GetCheckedInPatients().Where(p => p.RoomNumber != null && p.SurgeonAssigned == null).ToList();
            if (patientsWithRooms.Count == 0)
            {
                Console.WriteLine("There are no patients ready for surgery.\n");
                return;
            }

            Patient patient = InteractionHelper.SelectFromList(patientsWithRooms, "Please select your patient:");
            if (patient == null)
                return;

            var surgeons = hospital.GetAllSurgeons();
            if (surgeons.Count == 0)
            {
                Console.WriteLine("No surgeons registered.\n");
                return;
            }

            Surgeon surgeon = InteractionHelper.SelectFromList(surgeons, "Please select your surgeon:");
            if (surgeon == null)
                return;

            DateTime surgeryDateTime = InteractionHelper.GetValidSurgeryDateTime();
            AssignSurgeonToPatient(patient, surgeon, surgeryDateTime);
            Console.WriteLine($"Surgeon {surgeon.Name} has been assigned to patient {patient.Name}.");
            Console.WriteLine($"Surgery will take place on {surgeryDateTime:HH:mm dd/MM/yyyy}.\n");
        }

        private void UnassignRoomFromPatientAction(Hospital hospital)
        {
            var patientsWithRooms = hospital.GetAllPatients().Where(p => p.RoomNumber != null).ToList();
            if (patientsWithRooms.Count == 0)
            {
                Console.WriteLine("There are no patients ready to have their rooms unassigned.\n");
                return;
            }

            Patient patient = InteractionHelper.SelectFromList(patientsWithRooms, "Please select your patient:");
            if (patient == null)
                return;

            string roomNumber = patient.RoomNumber;
            string floorNumber = patient.FloorNumber;

            AssignRoomToPatient(patient, null);
            Console.WriteLine($"Room number {roomNumber} on floor {floorNumber} has been unassigned.\n");
        }
    }

    /// <summary>
    /// Represents a surgeon who performs surgeries on patients
    /// </summary>
    public class Surgeon : Staff
    {
        /// <summary>
        /// Gets or sets the surgeon's speciality
        /// </summary>
        public string Speciality { get; set; }

        /// <summary>
        /// Get the list of surgeries scheduled for the surgeon
        /// </summary>
        public List<(Patient Patient, DateTime SurgeryDateTime)> Surgeries { get; private set; } = new List<(Patient, DateTime)>();


        /// <summary>
        /// Initialises a new instance of the <see cref="Surgeon"/> class
        /// </summary>
        /// <param name="name">The surgeon's name </param>
        /// <param name="age">The surgeon's age</param>
        /// <param name="email">The surgeon's email address</param>
        /// <param name="mobileNumber">The surgeon's mobile number</param>
        /// <param name="password">The surgeon's password</param>
        /// <param name="staffID">The surgeon's staff ID</param>
        /// <param name="speciality">The surgeon's speciality</param>
        public Surgeon(string name, int age, string email, string mobileNumber, string password, string staffID, string speciality)
            : base(name, age, email, mobileNumber, password, staffID)
        {
            Speciality = speciality;
        }

        /// <summary>
        /// Adds a surgery to the surgeon's schedule
        /// </summary>
        /// <param name="patient">The patient scheduled for surgery</param>
        /// <param name="surgeryDateTime">The date and time of the surgery</param>
        public void AddSurgery(Patient patient, DateTime surgeryDateTime)
        {
            if (patient != null)
            {
                Surgeries.Add((patient, surgeryDateTime));
                // Don't sort to maintain assigned order
            }
        }

        /// <summary>
        /// Performs surgery on patient
        /// </summary>
        /// <param name="patient">The patient to operate on</param>
        public void PerformSurgery(Patient patient)
        {
            if (patient == null)
            {
                Console.WriteLine("Error: Patient not found.");
                return;
            }
            Console.WriteLine($"Surgery performed on {patient.Name} by {Name}.");
            Surgeries.RemoveAll(s => s.Patient == patient);
            patient.AssignSurgeon(null, null);
            patient.MarkSurgeryCompleted();
        }

        /// <summary>
        /// Displays the surgeon's schedule
        /// </summary>
        public void DisplaySchedule()
        {
            if (Surgeries.Count == 0)
            {
                Console.WriteLine("Your schedule.");
                Console.WriteLine("You do not have any patients assigned.");
                return;
            }
            Console.WriteLine("Your schedule.");
            var sortedSurgeries = Surgeries.OrderBy(s => s.SurgeryDateTime);
            foreach (var surgery in sortedSurgeries)
            {
                Console.WriteLine($"Performing surgery on patient {surgery.Patient.Name} on {surgery.SurgeryDateTime:HH:mm dd/MM/yyyy}");
            }
        }

        /// <summary>
        /// Displays the surgeons info
        /// </summary>
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Speciality: {Speciality}");
        }

        /// <summary>
        /// Performs surgeon-specific actions within the hospital system
        /// </summary>
        /// <param name="hospital"></param>
        public override void PerformActions(Hospital hospital)
        {
            while (true)
            {
                Console.WriteLine("Surgeon Menu.");
                Console.WriteLine("Please choose from the menu below:");
                Console.WriteLine("1. Display my details");
                Console.WriteLine("2. Change password");
                Console.WriteLine("3. See your list of patients");
                Console.WriteLine("4. See your schedule");
                Console.WriteLine("5. Perform surgery");
                Console.WriteLine("6. Log out");
                Console.Write($"Please enter a choice between {Program.MIN_MENU_OPTION} and {Program.MAX_MENU_OPTION_SURGEON}.\n");
                Console.WriteLine();

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        DisplayInfo();
                        Console.WriteLine();
                        break;
                    case "2":
                        Console.Write("Enter new password:\n");
                        string newPassword = Console.ReadLine();
                        ChangePassword(newPassword);
                        Console.WriteLine("Password has been changed.\n");
                        break;
                    case "3":
                        //Display list of patients assigned to the surgeon
                        DisplaySurgeonPatients();
                        break;
                    case "4":
                        //Displays the surgeon's schedule
                        DisplaySchedule();
                        Console.WriteLine();
                        break;
                    case "5":
                        //Perform the surgery on a selected patient
                        PerformSurgeryOnPatient();
                        break;
                    case "6":
                        Console.WriteLine($"Surgeon {Name} has logged out.\n");
                        return;
                    default:
                        UserInputHelper.DisplayError("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private void DisplaySurgeonPatients()
        {
            if (Surgeries.Count == 0)
            {
                Console.WriteLine("Your Patients.");
                Console.WriteLine("You do not have any patients assigned.\n");
            }
            else
            {
                Console.WriteLine("Your Patients.");
                int index = 1;
                foreach (var surgery in Surgeries)
                {
                    Console.WriteLine($"{index}. {surgery.Patient.Name}");
                    index++;
                }
                Console.WriteLine();
            }
        }

        private void PerformSurgeryOnPatient()
        {
            if (Surgeries.Count == 0)
            {
                Console.WriteLine("Your schedule.");
                Console.WriteLine("You do not have any patients assigned.\n");
            }
            else
            {   //Let the surgeon select a patient to perform surgery on
                Patient selectedPatient = InteractionHelper.SelectFromList(Surgeries.Select(s => s.Patient).ToList(), "Please select your patient:");
                if (selectedPatient != null)
                {   //Perform surgery on the selected patient
                    PerformSurgery(selectedPatient);
                    Console.WriteLine();
                }
            }
        }
    }

    /// <summary>
    /// Manages users, rooms, and operations within the hospital
    /// </summary>
    public class Hospital
    {
        private List<User> Users = new List<User>();
        private List<string> AvailableRooms = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

        /// <summary>
        /// Gets a list of floor numbers that have been assigned to floor managers
        /// </summary>
        /// <returns>A list of assigned floor numbers</returns>
        public List<string> GetAssignedFloorNumbers()
        {
            return Users.OfType<FloorManager>().Select(fm => fm.FloorNumber).ToList();
        }

        /// <summary>
        /// Registers a new user in the hospital
        /// </summary>
        /// <param name="user">The user to register</param>
        /// <exception cref="Exception">Thrown when the email is already registered</exception>
        public void RegisterUser(User user)
        {
            if (Users.Exists(u => u.Email == user.Email))
            {
                throw new Exception("Email must be unique.");
            }
            Users.Add(user);
        }
        /// <summary>
        /// Checks if the email is already registered in the system
        /// </summary>
        /// <param name="email"The email to check></param>
        /// <returns><c>true</c> if the email exists; otherwise, <c>false</c>.</returns>              
        public bool EmailExists(string email)
        {
            return Users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Checks if a staff ID is already registered in the system
        /// </summary>
        /// <param name="staffID">The staff ID to check</param>
        /// <returns><c>true</c> if the staff ID is registered; otherwise<c>false</c>.</returns>
        public bool IsStaffIDRegistered(string staffID)
        {
            return Users.OfType<Staff>().Any(s => s.StaffID == staffID);
        }

        /// <summary>
        /// Checks if a floor number is already assigned to a floor manager
        /// </summary>
        /// <param name="floorNumber">The floor number to check</param>
        /// <returns><c>true</c> if the floor number is assigned; otherwise, <c>false</c>.</returns>
        public bool IsFloorNumberAssigned(string floorNumber)
        {
            return Users.OfType<FloorManager>().Any(fm => fm.FloorNumber == floorNumber);
        }

        /// <summary>
        /// Determines whether there are any users registered in the system
        /// </summary>
        /// <returns><c>true</c> if there are registered users; otherwise<c>false</c>.</returns>
        public bool HasRegisteredUsers()
        {
            return Users.Count > 0;
        }

        /// <summary>
        /// Authenticates a user based on email and password
        /// </summary>
        /// <param name="email">The user's email</param>
        /// <param name="password">The user's password</param>
        /// <returns>The authenticated <see cref="User"/> object.</returns>
        /// <exception cref="Exception"></exception>
        public User Login(string email, string password)
        {   //Find the user by email
            var user = Users.Find(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (user == null)
            {   //Throw exception if email is not registered
                throw new Exception("#####\n#Error - Email is not registered.\n#####");
            }
            else if (!user.Authenticate(email, password))
            {   //Throw exception if password is incorrect
                throw new Exception("#####\n#Error - Wrong Password.\n#####");
            }
            else
            {   //Return the verified user
                return user;
            }
        }

        /// <summary>
        /// Retrieves all patients registered in the hospital
        /// </summary>
        /// <returns>A list of all registered patients</returns>
        public List<Patient> GetAllPatients()
        {
            return Users.OfType<Patient>().ToList();
        }

        /// <summary>
        /// Retrieves all patients who are currently checked in
        /// </summary>
        /// <returns>A list of checked-in patients.</returns>
        public List<Patient> GetCheckedInPatients()
        {
            return Users.OfType<Patient>().Where(p => p.IsCheckedIn).ToList();
        }

        /// <summary>
        /// Retrieves all surgeons registered in the hospital
        /// </summary>
        /// <returns>A list of all registered surgeons</returns>
        public List<Surgeon> GetAllSurgeons()
        {
            return Users.OfType<Surgeon>().ToList();
        }
        /// <summary>
        /// Gets a list of available rooms on a specific floor
        /// </summary>
        /// <param name="floorNumber">The floor number to check for available rooms</param>
        /// <returns>A list of available room numbers</returns>
        public List<string> GetAvailableRooms(string floorNumber)
        {
            return AvailableRooms.Where(room => !Users.OfType<Patient>().Any(p => p.RoomNumber == room && p.FloorNumber == floorNumber)).ToList();
        }
    }

    /// <summary>
    ///  Provides validation methods for user input
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// Validates if the provided name is valid
        /// </summary>
        /// <param name="name">The name to validate</param>
        /// <returns><c>true</c> if the name is valid; otherwise, <c>false</c>.</returns>
        public static bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;
            return name.All(c => char.IsLetter(c) || c == ' ');
        }

        /// <summary>
        /// Validates if the provided age is within the specified range
        /// </summary>
        /// <param name="age">The age to validate</param>
        /// <param name="minAge">The minimum valid age</param>
        /// <param name="maxAge">The maximum valid age</param>
        /// <returns><c>true</c> if the age is valid; otherwise, <c>false</c>.</returns>
        public static bool IsValidAge(int age, int minAge, int maxAge)
        {
            return age >= minAge && age <= maxAge;
        }

        /// <summary>
        ///  Validates if the provided email is in a valid format.
        /// </summary>
        /// <param name="email">The email to validate.</param>
        /// <returns><c>true</c> if the email is valid; otherwise, <c>false</c>.</returns>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        ///  Validates if the inputted password meets the required criteria
        /// </summary>
        /// <param name="password">The password to validate</param>
        /// <returns><c>true</c> if the password is valid; otherwise, <c>false</c>.</returns>
        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;
            if (password.Length < 8)
                return false;
            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            return hasUpper && hasLower && hasDigit;
        }

        /// <summary>
        /// Validates if the provided mobile number is in a valid format
        /// </summary>
        /// <param name="mobileNumber">The mobile number to validate</param>
        /// <returns><c>true</c> if the mobile number is valid; otherwise, <c>false</c>.</returns>
        public static bool IsValidMobileNumber(string mobileNumber)
        {
            return !string.IsNullOrWhiteSpace(mobileNumber) &&
                   mobileNumber.All(char.IsDigit) &&
                   mobileNumber.Length == 10 &&
                   mobileNumber.StartsWith("0");
        }

        /// <summary>
        /// Validates if the inputted staff ID is within the valid range
        /// </summary>
        /// <param name="staffID">The staff ID to validate</param>
        /// <param name="minStaffID">The minimum valid staff ID</param>
        /// <param name="maxStaffID">The maximum valid staff ID</param>
        /// <returns><c>true</c> if the staff ID is valid; otherwise, <c>false</c>.</returns>
        public static bool IsValidStaffID(string staffID, int minStaffID, int maxStaffID)
        {
            if (int.TryParse(staffID, out int staffIDInt))
            {
                return staffIDInt >= minStaffID && staffIDInt <= maxStaffID;
            }
            return false;
        }
    }

    /// <summary>
    /// Handles user input and validation
    /// </summary>
    public static class UserInputHelper
    {
        /// <summary>
        /// Prompts the user to enter and validates a name
        /// </summary>
        /// <returns>The validated name</returns>
        public static string GetValidatedName()
        {
            while (true)
            {
                Console.Write("Please enter in your name:\n");
                string name = Console.ReadLine();
                if (Validator.IsValidName(name))
                    return name;
                else
                    DisplayError("Supplied name is invalid, please try again.");
            }
        }

        /// <summary>
        /// Prompts the user to enter and validates an age
        /// </summary>
        /// <param name="minAge">The minimum valid age</param>
        /// <param name="maxAge">The maximum valid age</param>
        /// <returns>The validated age</returns>
        public static int GetValidatedAge(int minAge, int maxAge)
        {
            while (true)
            {
                Console.Write("Please enter in your age:\n");
                string ageInput = Console.ReadLine();
                if (!int.TryParse(ageInput, out int age))
                {
                    DisplayError("Supplied value is not an integer, please try again.");
                }
                else if (!Validator.IsValidAge(age, minAge, maxAge))
                {
                    DisplayError("Supplied age is invalid, please try again.");
                }
                else
                {
                    return age;
                }
            }
        }

        /// <summary>
        /// Prompts the user to enter and validates the inputted mobile number
        /// </summary>
        /// <returns>The validated mobile number</returns>
        public static string GetValidatedMobileNumber()
        {
            while (true)
            {
                Console.Write("Please enter in your mobile number:\n");
                string mobileNumber = Console.ReadLine();
                if (Validator.IsValidMobileNumber(mobileNumber))
                    return mobileNumber;
                else
                    DisplayError("Supplied mobile number is invalid, please try again.");
            }
        }

        /// <summary>
        /// Prompts the user to enter and validates the inputted email
        /// </summary>
        /// <param name="hospital">The hospital instance for checking registered emails </param>
        /// <returns>The validated email</returns>
        public static string GetValidatedEmail(Hospital hospital)
        {
            while (true)
            {
                Console.Write("Please enter in your email:\n");
                string email = Console.ReadLine();
                if (!Validator.IsValidEmail(email))
                {   //Email format is invalid
                    DisplayError("Supplied email is invalid, please try again.");
                }
                else if (hospital.EmailExists(email))
                {   //Email is already registered
                    DisplayError("Email is already registered, please try again.");
                }
                else
                {   //Valid unique email
                    return email;
                }
            }
        }

        /// <summary>
        /// Prompts the user to enter and validates the inputted password
        /// </summary>
        /// <returns>The validated password</returns>
        public static string GetValidatedPassword()
        {
            while (true)
            {
                Console.Write("Please enter in your password:\n");
                string password = Console.ReadLine();
                if (Validator.IsValidPassword(password))
                    return password;
                else
                    DisplayError("Supplied password is invalid, please try again.");
            }
        }

        /// <summary>
        /// Prompts the user to enter and validates the inputted staff ID
        /// </summary>
        /// <param name="hospital">The hospital instance for checking registered staff IDs</param>
        /// <param name="minStaffID">The minimum valid staff ID</param>
        /// <param name="maxStaffID">The maximum valid staff ID</param>
        /// <returns>The validated staff ID</returns>
        public static string GetValidatedStaffID(Hospital hospital, int minStaffID, int maxStaffID)
        {
            while (true)
            {
                Console.Write("Please enter in your staff ID:\n");
                string staffID = Console.ReadLine();
                if (!Validator.IsValidStaffID(staffID, minStaffID, maxStaffID))
                {
                    DisplayError("Supplied staff identification number is invalid, please try again.");
                }
                else if (hospital.IsStaffIDRegistered(staffID))
                {
                    DisplayError("Staff ID is already registered, please try again.");
                }
                else
                {
                    return staffID;
                }
            }
        }

        /// <summary>
        /// Prompts the user to enter and validates the inputted floor number
        /// </summary>
        /// <param name="hospital">The hospital instance for checking assigned floor numbers </param>
        /// <param name="minFloorNumber">The minimum valid floor number</param>
        /// <param name="maxFloorNumber">The maximum valid floor number</param>
        /// <returns>The validated floor number</returns>
        public static string GetValidatedFloorNumber(Hospital hospital, int minFloorNumber, int maxFloorNumber)
        {
            while (true)
            {
                Console.Write("Please enter in your floor number:\n");
                string floorInput = Console.ReadLine();
                if (int.TryParse(floorInput, out int floorNumber) && floorNumber >= minFloorNumber && floorNumber <= maxFloorNumber)
                {
                    if (hospital.IsFloorNumberAssigned(floorInput))
                    {
                        DisplayError("Floor has been assigned to another floor manager, please try again.");
                    }
                    else
                    {
                        return floorInput;
                    }
                }
                else
                {
                    DisplayError("Supplied floor is invalid, please try again.");
                }
            }
        }

        /// <summary>
        /// Prompts the user to select and validates inputted surgeon speciality
        /// </summary>
        /// <returns>The validated speciality</returns>
        public static string GetValidatedSpeciality()
        {
            while (true)
            {
                Console.WriteLine("Please choose your speciality:");
                Console.WriteLine("1. General Surgeon");
                Console.WriteLine("2. Orthopaedic Surgeon");
                Console.WriteLine("3. Cardiothoracic Surgeon");
                Console.WriteLine("4. Neurosurgeon");
                Console.Write("Please enter a choice between 1 and 4.\n");

                string specialityChoice = Console.ReadLine();
                switch (specialityChoice)
                {
                    case "1":
                        return "General Surgeon";
                    case "2":
                        return "Orthopaedic Surgeon";
                    case "3":
                        return "Cardiothoracic Surgeon";
                    case "4":
                        return "Neurosurgeon";
                    default:
                        DisplayError("Non-valid speciality type, please try again.");
                        break;
                }
            }
        }

        /// <summary>
        /// Displays an error message to the user
        /// </summary>
        /// <param name="message">The error message to display</param>
        public static void DisplayError(string message)
        {
            Console.WriteLine("#####");
            Console.WriteLine($"#Error - {message}");
            Console.WriteLine("#####\n");
        }
    }

    /// <summary>
    /// Handles selection and date input
    /// </summary>
    public static class InteractionHelper
    {
        /// <summary>
        /// Prompts the user to select an item from a list
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="items">The list of items to select from</param>
        /// <param name="promptMessage">The prompt message that's displayed</param>
        /// <returns>The selected item</returns>
        public static T SelectFromList<T>(List<T> items, string promptMessage) where T : User
        {
            Console.WriteLine(promptMessage);
            for (int i = 0; i < items.Count; i++)
            {   //Display each item with an index
                Console.WriteLine($"{i + 1}. {items[i].Name}");
            }

            int choice;
            while (true)
            {
                Console.Write($"Please enter a choice between 1 and {items.Count}.\n");
                Console.WriteLine();

                if (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > items.Count)
                {   //Invalid input or out of range
                    UserInputHelper.DisplayError("Supplied value is out of range, please try again.");
                    continue;
                }
                else
                {   //Returns the selected item
                    return items[choice - 1];
                }
            }
        }

        /// <summary>
        /// Prompts the user to enter and validates the inputted surgery, date and time
        /// </summary>
        /// <returns>The validated surgery date and time</returns>
        public static DateTime GetValidSurgeryDateTime()
        {
            string format = "HH:mm dd/MM/yyyy";
            while (true)
            {
                Console.Write("Please enter a date and time (e.g. 14:30 31/01/2024).\n");
                string dateTimeInput = Console.ReadLine();
                Console.WriteLine();

                if (DateTime.TryParseExact(dateTimeInput, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime surgeryDateTime))
                {
                    return surgeryDateTime;
                }
                else
                {
                    UserInputHelper.DisplayError("Supplied value is not a valid DateTime.");
                }
            }
        }
    }

    /// <summary>
    /// The main entry point of the system
    /// </summary>    
    class Program
    {
        // Defining maximum allowed ages
        public const int MAX_AGE = 100;
        public const int MIN_AGE_PATIENT = 0;
        public const int MIN_AGE_STAFF = 21;
        public const int MAX_AGE_STAFF = 70;

        // Defining the other constants
        public const int MIN_MENU_OPTION = 1;
        public const int MAX_MENU_OPTION_MAIN = 3;
        public const int MAX_MENU_OPTION_REGISTER_USER = 3;
        public const int MAX_MENU_OPTION_REGISTER_STAFF = 3;
        public const int MAX_MENU_OPTION_PATIENT = 7;
        public const int MAX_MENU_OPTION_FLOOR_MANAGER = 6;
        public const int MAX_MENU_OPTION_SURGEON = 6;
        public const int MIN_FLOOR_NUMBER = 1;
        public const int MAX_FLOOR_NUMBER = 6;
        public const int MIN_STAFF_ID = 100;
        public const int MAX_STAFF_ID = 999;
        public const int MIN_ROOM_NUMBER = 1;
        public const int MAX_ROOM_NUMBER = 10;

        /// <summary>
        /// The main method that initiates the application
        /// </summary>
        /// <param name="args">Command-line arguments</param>
        static void Main(string[] args)
        {
            Hospital hospital = new Hospital();
            bool showWelcome = true;

            if (showWelcome)
            {   //Display the welcome message (only once)
                Console.WriteLine("=================================");
                Console.WriteLine("Welcome to Gardens Point Hospital");
                Console.WriteLine("=================================\n");
                showWelcome = false;
            }

            while (true)
            {
                Console.WriteLine("Please choose from the menu below:");
                Console.WriteLine("1. Login as a registered user");
                Console.WriteLine("2. Register as a new user");
                Console.WriteLine("3. Exit");
                Console.Write($"Please enter a choice between {MIN_MENU_OPTION} and {MAX_MENU_OPTION_MAIN}.\n");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        try
                        {   //Attempt to log in and perform actions
                            LoginAndPerformActions(hospital);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message + "\n");
                        }
                        break;
                    case "2":
                        try
                        {   
                            RegisterUser(hospital);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message + "\n");
                        }
                        break;
                    case "3":
                        Console.WriteLine("Goodbye. Please stay safe.");
                        return;
                    default:
                        UserInputHelper.DisplayError("Invalid Menu Option, please try again.");
                        break;
                }
            }
        }

        /// <summary>
        /// Handles the user registration process
        /// </summary>
        /// <param name="hospital">The hospital instance</param>
        static void RegisterUser(Hospital hospital)
        {
            Console.WriteLine("Register as which type of user:");
            Console.WriteLine("1. Patient");
            Console.WriteLine("2. Staff");
            Console.WriteLine("3. Return to the first menu");
            Console.Write($"Please enter a choice between {MIN_MENU_OPTION} and {MAX_MENU_OPTION_REGISTER_USER}.\n");

            string userType = Console.ReadLine();
            Console.WriteLine();

            switch (userType)
            {
                case "1":
                    RegisterPatient(hospital);
                    break;
                case "2":
                    RegisterStaff(hospital);
                    break;
                case "3":
                    // Return to main menu
                    break;
                default:
                    UserInputHelper.DisplayError("Invalid Menu Option, please try again.");
                    break;
            }
        }

        /// <summary>
        /// Registers a new patient into the system
        /// </summary>
        /// <param name="hospital">The hospital instance</param>
        static void RegisterPatient(Hospital hospital)
        {
            Console.WriteLine("Registering as a patient.");
            string name = UserInputHelper.GetValidatedName();
            int age = UserInputHelper.GetValidatedAge(MIN_AGE_PATIENT, MAX_AGE);
            string mobileNumber = UserInputHelper.GetValidatedMobileNumber();
            string email = UserInputHelper.GetValidatedEmail(hospital);
            string password = UserInputHelper.GetValidatedPassword();

            try
            {
                Patient patient = new Patient(name, age, email, mobileNumber, password);
                hospital.RegisterUser(patient);
                Console.WriteLine($"{name} is registered as a patient.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during registration: {ex.Message}\n");
            }
        }


        /// <summary>
        /// Handles the staff registration process
        /// </summary>
        /// <param name="hospital">The hospital instance</param>
        static void RegisterStaff(Hospital hospital)
        {
            Console.WriteLine("Register as which type of staff:");
            Console.WriteLine("1. Floor manager");
            Console.WriteLine("2. Surgeon");
            Console.WriteLine("3. Return to the first menu");
            Console.Write($"Please enter a choice between {MIN_MENU_OPTION} and {MAX_MENU_OPTION_REGISTER_STAFF}.\n");

            string staffType = Console.ReadLine();
            Console.WriteLine();

            switch (staffType)
            {
                case "1":
                    RegisterFloorManager(hospital);
                    break;
                case "2":
                    RegisterSurgeon(hospital);
                    break;
                case "3":
                    // Return to main menu
                    break;
                default:
                    UserInputHelper.DisplayError("Invalid Menu Option, please try again.");
                    break;
            }
        }

        /// <summary>
        /// Registers a floor manager in the hospital system
        /// </summary>
        /// <param name="hospital">The hospital</param>
        static void RegisterFloorManager(Hospital hospital)
        {
            if (hospital.GetAssignedFloorNumbers().Count >= MAX_FLOOR_NUMBER)
            {
                UserInputHelper.DisplayError("All floors are assigned.");
                return;
            }

            Console.WriteLine("Registering as a floor manager.");
            string name = UserInputHelper.GetValidatedName();
            int age = UserInputHelper.GetValidatedAge(MIN_AGE_STAFF, MAX_AGE_STAFF);
            string mobileNumber = UserInputHelper.GetValidatedMobileNumber();
            string email = UserInputHelper.GetValidatedEmail(hospital);
            string password = UserInputHelper.GetValidatedPassword();
            string staffID = UserInputHelper.GetValidatedStaffID(hospital, MIN_STAFF_ID, MAX_STAFF_ID);
            string floorNumber = UserInputHelper.GetValidatedFloorNumber(hospital, MIN_FLOOR_NUMBER, MAX_FLOOR_NUMBER);

            try
            {
                FloorManager floorManager = new FloorManager(name, age, email, mobileNumber, password, staffID, floorNumber);
                hospital.RegisterUser(floorManager);
                Console.WriteLine($"{name} is registered as a floor manager.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during registration: {ex.Message}\n");
            }
        }

        /// <summary>
        /// Registers a new surgeon in the hospital system
        /// </summary>
        /// <param name="hospital">The hospital instance</param>
        static void RegisterSurgeon(Hospital hospital)
        {
            Console.WriteLine("Registering as a surgeon.");
            string name = UserInputHelper.GetValidatedName();
            int age = UserInputHelper.GetValidatedAge(MIN_AGE_STAFF, MAX_AGE);
            string mobileNumber = UserInputHelper.GetValidatedMobileNumber();
            string email = UserInputHelper.GetValidatedEmail(hospital);
            string password = UserInputHelper.GetValidatedPassword();
            string staffID = UserInputHelper.GetValidatedStaffID(hospital, MIN_STAFF_ID, MAX_STAFF_ID);
            string speciality = UserInputHelper.GetValidatedSpeciality();

            try
            {
                Surgeon surgeon = new Surgeon(name, age, email, mobileNumber, password, staffID, speciality);
                hospital.RegisterUser(surgeon);
                Console.WriteLine($"{name} is registered as a surgeon.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during registration: {ex.Message}\n");
            }
        }

        /// <summary>
        /// Handles user login and initiates user-specific actions
        /// </summary>
        /// <param name="hospital">The hospital instance for user authentication</param>
        /// <exception cref="Exception">Thrown when invalid operation occurs</exception>
        static void LoginAndPerformActions(Hospital hospital)
        {
            Console.WriteLine("Login Menu.");
            if (!hospital.HasRegisteredUsers())
            {   //No users registered in the system
                throw new Exception("#####\n#Error - There are no people registered.\n#####");
            }
            Console.Write("Please enter in your email:\n");
            string email = Console.ReadLine();

            if (!hospital.EmailExists(email))
            {   //Email is not registered in the system
                Console.WriteLine("#####\n#Error - Email is not registered.\n#####");
                return;
            }

            Console.Write("Please enter in your password:\n");
            string password = Console.ReadLine();
            Console.WriteLine();

            try
            {   //Authenticate the user
                User user = hospital.Login(email, password);
                Console.WriteLine($"Hello {user.Name} welcome back.\n");
                //Perform user-specific actions
                user.PerformActions(hospital);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }
        }
    }
}
