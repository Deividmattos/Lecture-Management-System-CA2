using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace CA2_Lecture_Management_System
{
    // Base class
    public class Lecture
    {
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string LecturerName { get; set; }
        public int LectureHours { get; set; }

        // Static member required by the CA2
        public static int LectureCount { get; private set; }

        public Lecture(string subjectCode, string subjectName, string lecturerName, int lectureHours)
        {
            SubjectCode = subjectCode;
            SubjectName = subjectName;
            LecturerName = lecturerName;
            LectureHours = lectureHours;
            LectureCount++;
        }

        // Used by the loader so it does not count loaded records twice.
        public static void SetLectureCount(int count)
        {
            LectureCount = count;
        }

        public virtual string GetLectureType()
        {
            return "General Lecture";
        }

        public virtual string GetDescription()
        {
            return $"{SubjectCode} - {SubjectName}";
        }
    }

    // Derived class 1
    public class Programming : Lecture
    {
        public Programming(string subjectCode, string subjectName, string lecturerName, int lectureHours)
            : base(subjectCode, subjectName, lecturerName, lectureHours)
        {
        }

        public override string GetLectureType()
        {
            return "Programming";
        }

        public override string GetDescription()
        {
            return $"{base.GetDescription()} | Focus: Software development and programming";
        }
    }

    // Derived class 2
    public class Database : Lecture
    {
        public Database(string subjectCode, string subjectName, string lecturerName, int lectureHours)
            : base(subjectCode, subjectName, lecturerName, lectureHours)
        {
        }

        public override string GetLectureType()
        {
            return "Database";
        }

        public override string GetDescription()
        {
            return $"{base.GetDescription()} | Focus: Data management and databases";
        }
    }

      // Derived class 3
    public class Networking : Lecture
    {
        public Networking(string subjectCode, string subjectName, string lecturerName, int lectureHours)
            : base(subjectCode, subjectName, lecturerName, lectureHours)
        {
        }

        public override string GetLectureType()
        {
            return "Networking";
        }

        public override string GetDescription()
        {
            return $"{base.GetDescription()} | Focus: Computer networks and communication";
        }
    }

    // Simple DTO used for polymorphic file storage.
    public class LectureData
    {
        public string Type { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string LecturerName { get; set; }
        public int LectureHours { get; set; }
    }

    public class LectureManager
    {
        private readonly List<Lecture> lectures = new List<Lecture>();
        private const string FileName = "lectures.json";

        public void Run()
        {
            LoadFromFile();

            bool running = true;

            while (running)
            {
                Console.Clear();
                DisplayHeader();
                DisplayMenu();

                Console.Write("Select an option: ");
                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        AddLecture();
                        break;
                    case "2":
                        ViewLecture();
                        break;
                    case "3":
                        ListLectures();
                        break;
                    case "4":
                        EditLecture();
                        break;
                    case "5":
                        DeleteLecture();
                        break;
                    case "6":
                        SaveToFile();
                        running = false;
                        Console.WriteLine("\nData saved. Goodbye!");
                        break;
                    default:
                        ShowError("Invalid menu selection. Please choose 1-6.");
                        break;
                }
            }
        }

        private void DisplayHeader()
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("       LECTURE MANAGEMENT SYSTEM - CA2");
            Console.WriteLine("==============================================");
            Console.WriteLine($"Number of lectures: {lectures.Count}");
            Console.WriteLine($"Static LectureCount: {Lecture.LectureCount}");
            Console.WriteLine();
        }

        private void DisplayMenu()
        {
            Console.WriteLine("1. Add Lecture");
            Console.WriteLine("2. View Lecture");
            Console.WriteLine("3. List Lectures");
            Console.WriteLine("4. Edit Lecture");
            Console.WriteLine("5. Delete Lecture");
            Console.WriteLine("6. Exit");
            Console.WriteLine();
        }

        private void AddLecture()
        {
            Console.Clear();
            Console.WriteLine("--------------- ADD LECTURE ---------------");

            string code = ReadRequiredString("Subject Code: ");

            if (FindLecture(code) != null)
            {
                ShowError("A lecture with this subject code already exists.");
                return;
            }

            string name = ReadRequiredString("Subject Name: ");
            string lecturer = ReadRequiredString("Lecturer Name: ");
            int hours = ReadPositiveInt("Number of Lecture Hours: ");
            string type = ReadLectureType();

            Lecture lecture = CreateLecture(type, code, name, lecturer, hours);
            lectures.Add(lecture);
            SaveToFile();

            Console.WriteLine("\nLecture added successfully.");
            Pause();
        }
          private void ViewLecture()
        {
            Console.Clear();
            Console.WriteLine("--------------- VIEW LECTURE ---------------");

            string code = ReadRequiredString("Enter Subject Code: ");
            Lecture lecture = FindLecture(code);

            if (lecture == null)
            {
                ShowError("Lecture not found.");
                return;
            }

            DisplayLectureDetails(lecture);
            Pause();
        }

        private void ListLectures()
        {
            Console.Clear();
            Console.WriteLine("--------------- ALL LECTURES ---------------");

            if (lectures.Count == 0)
            {
                Console.WriteLine("No lectures are currently stored.");
                Pause();
                return;
            }

            foreach (Lecture lecture in lectures)
            {
                Console.WriteLine($"[{lecture.GetLectureType()}] {lecture.SubjectCode} - {lecture.SubjectName}");
                Console.WriteLine($"Lecturer: {lecture.LecturerName} | Hours: {lecture.LectureHours}");
                Console.WriteLine($"Description: {lecture.GetDescription()}");
                Console.WriteLine("----------------------------------------------");
            }

            Pause();
        }

        private void EditLecture()
        {
            
        }
        Console.Clear();
            Console.WriteLine("--------------- EDIT LECTURE ---------------");

            string code = ReadRequiredString("Enter Subject Code to edit: ");
            Lecture lecture = FindLecture(code);

            if (lecture == null)
            {
                ShowError("Lecture not found.");
                return;
            }

            Console.WriteLine("\nPress ENTER to keep the current value.");

            Console.Write($"Subject Name ({lecture.SubjectName}): ");
            string name = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(name))
                lecture.SubjectName = name.Trim();

            Console.Write($"Lecturer Name ({lecture.LecturerName}): ");
            string lecturer = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(lecturer))
                lecture.LecturerName = lecturer.Trim();

            Console.Write($"Lecture Hours ({lecture.LectureHours}): ");
            string hoursInput = Console.ReadLine() ?? "";

            if (!string.IsNullOrWhiteSpace(hoursInput))
            {
                if (int.TryParse(hoursInput, out int hours) && hours > 0)
                    lecture.LectureHours = hours;
                else
                    ShowError("Invalid hours. The previous value was kept.");
            }

            SaveToFile();
            Console.WriteLine("\nLecture updated successfully.");
            Pause();
        }

        private void DeleteLecture()
        {
            Console.Clear();
            Console.WriteLine("--------------- DELETE LECTURE ---------------");

            string code = ReadRequiredString("Enter Subject Code to delete: ");
            Lecture lecture = FindLecture(code);

            if (lecture == null)
            {
                ShowError("Lecture not found.");
                return;
            }

            DisplayLectureDetails(lecture);

            Console.Write("\nAre you sure you want to delete this lecture? (Y/N): ");
            string confirmation = (Console.ReadLine() ?? "").Trim().ToUpper();

            if (confirmation == "Y")
            {
                lectures.Remove(lecture);
                Lecture.SetLectureCount(lectures.Count);
                SaveToFile();
                Console.WriteLine("\nLecture deleted successfully.");
            }
            else
            {
                Console.WriteLine("\nDelete cancelled.");
            }

            Pause();
        }
    }
    
}
