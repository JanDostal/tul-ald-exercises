using System;

namespace Exercise_13
{
    public class Student
    {
        public int Order { get; }

        public string Forename { get; }

        public string Surname { get; }

        public StudentSpecialization Specialization { get; }

        public long PersonalNumber { get; }

        public char PersonalNumberCodeLetter { get; }

        public Student(int order, string forename, string surname, 
                       StudentSpecialization specialization, long personalNumber, char personalNumberCodeLetter) 
        {
            Order = order;
            Forename = forename;
            Surname = surname;
            Specialization = specialization;
            PersonalNumber = personalNumber;
            PersonalNumberCodeLetter = personalNumberCodeLetter;
        }

        public enum StudentSpecialization
        {
            AVI,
            AI,
            IS,
            IT
        }

        public override string ToString() 
        {
            string fullName = $"{Surname.ToUpper()} {Forename}";

            return $"{PersonalNumberCodeLetter} {fullName, -19}{Specialization, -4}{PersonalNumber}";
        }
    }
}
