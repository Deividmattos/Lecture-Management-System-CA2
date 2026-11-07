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


}
