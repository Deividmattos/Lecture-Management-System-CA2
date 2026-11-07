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
    }
    
}
