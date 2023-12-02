using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;

namespace Exercise_13
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            IList<Student> students;

            using (StreamReader fs = File.OpenText($".{Path.DirectorySeparatorChar}vstup.html")) 
            {
                students = await LoadStudentsFromTextSource(fs);
            }

            if (students.Count != 0) 
            {
                string[] studentsSpecializationsNames = Enum.GetNames(typeof(Student.StudentSpecialization));
                Array.Sort(studentsSpecializationsNames);

                IEnumerable<Student> filteredStudents;
                Student.StudentSpecialization studentsSpecialization;
                int order;

                StringBuilder textOutput = new StringBuilder();

                foreach (var studentsSpecializationName in studentsSpecializationsNames) 
                {
                    studentsSpecialization = 
                        (Student.StudentSpecialization) Enum.Parse(typeof(Student.StudentSpecialization), 
                        studentsSpecializationName);

                    filteredStudents = students.
                        Where(x => x.Specialization == studentsSpecialization);
                    filteredStudents = filteredStudents.Where(x => x.PersonalNumber % 2 != 0).OrderBy(x => x.PersonalNumber).
                        Union(filteredStudents.Where(x => x.PersonalNumber % 2 == 0).OrderBy(x => x.PersonalNumber)).AsEnumerable();

                    if (filteredStudents.Count() != 0) 
                    {
                        order = 0;

                        textOutput.AppendLine($"{studentsSpecializationName}:");

                        foreach (var student in filteredStudents)
                        {
                            order++;
                            textOutput.AppendLine($"{order,2}: {student}");
                        }

                        textOutput.AppendLine();
                    }
                }

                textOutput.Remove(textOutput.Length - 1, 1);

                Console.Write(textOutput);
            }
        }

        private static async Task<IList<Student>> LoadStudentsFromTextSource(TextReader reader) 
        {
            IList<Student> students = new List<Student>();

            string? htmlFile = await ReadTextSource(reader);

            if (htmlFile != null) 
            {
                IDictionary<string, Regex> studentsHtmlMatchers = LoadStudentsHtmlMatchers();

                MatchCollection studentsOrders = studentsHtmlMatchers["studentsOrders"].Matches(htmlFile);
                MatchCollection studentsFullNames = studentsHtmlMatchers["studentsFullNames"].Matches(htmlFile);
                MatchCollection studentsFullPersonalNumbers = studentsHtmlMatchers["studentsFullPersonalNumbers"].Matches(htmlFile);
                MatchCollection studentsSpecializations = studentsHtmlMatchers["studentsSpecializations"].Matches(htmlFile);

                ICollection<int> matchesCounts = new Collection<int>()
                {
                    studentsOrders.Count,
                    studentsFullNames.Count,
                    studentsFullPersonalNumbers.Count,
                    studentsSpecializations.Count
                };

                if (matchesCounts.All(x => x == studentsOrders.Count) == true) 
                {
                    Group studentOrder;
                    Group studentForename;
                    Group studentSurname;
                    Group studentPersonalNumber;
                    Group studentPersonalNumberCodeLetter;
                    Group studentSpecialization;

                    string[] availableValidSpecializations;
                    bool isStudentSpecializationValid;
                    Student.StudentSpecialization studentValidSpecialization;

                    for (int i = 0; i < studentsOrders.Count; i++) 
                    {
                        studentOrder = studentsOrders[i].Groups["studentOrder"];
                        studentForename = studentsFullNames[i].Groups["studentForename"];
                        studentSurname = studentsFullNames[i].Groups["studentSurname"];
                        studentPersonalNumber = studentsFullPersonalNumbers[i].Groups["studentPersonalNumber"];
                        studentPersonalNumberCodeLetter = studentsFullPersonalNumbers[i].Groups["studentPersonalNumberCodeLetter"];
                        studentSpecialization = studentsSpecializations[i].Groups["studentSpecialization"];

                        isStudentSpecializationValid = false;
                        availableValidSpecializations = Enum.GetNames(typeof(Student.StudentSpecialization));

                        foreach (var specialization in availableValidSpecializations)
                        {
                            if (string.Compare(studentSpecialization.Value, specialization) == 0) 
                            {
                                isStudentSpecializationValid = true;
                                break;
                            }
                        }

                        if (isStudentSpecializationValid == true) 
                        {
                            studentValidSpecialization =
                                (Student.StudentSpecialization) Enum.Parse(typeof(Student.StudentSpecialization), 
                                studentSpecialization.Value);

                            students.Add(new Student(int.Parse(studentOrder.Value), studentForename.Value, studentSurname.Value,
                                studentValidSpecialization, long.Parse(studentPersonalNumber.Value), 
                                char.Parse(studentPersonalNumberCodeLetter.Value)));
                        }
                    }
                }
            }

            return students;
        }

        private static async Task<string?> ReadTextSource(TextReader reader)
        {
            var textSource = await reader.ReadToEndAsync();

            return textSource;
        }

        private static IDictionary<string, Regex> LoadStudentsHtmlMatchers() 
        {
            IDictionary<string, Regex> htmlStudentsMatchers = new Dictionary<string, Regex>();

            var htmlAttributesDivider = $@"([\s\t]|{Regex.Escape(Environment.NewLine)})";
            var htmlAttributes =
                $@"([\w\-]+{htmlAttributesDivider}*={htmlAttributesDivider}*"".*""{htmlAttributesDivider}*)*";

            var studentsOrdersMatcherGroup = @"(?<studentOrder>[1-9]\d*)";
            var studentsOrdersMatcher = new Regex
            (
                $@"<td{htmlAttributesDivider}+{htmlAttributes}\bdata-header{htmlAttributesDivider}*=" +
                $@"{htmlAttributesDivider}*""Por\.\: ""{htmlAttributesDivider}*{htmlAttributes}>{htmlAttributesDivider}*" +
                $@"<b{htmlAttributesDivider}*{htmlAttributes}>{htmlAttributesDivider}*" +
                $@"{studentsOrdersMatcherGroup}\.{htmlAttributesDivider}*<\/b{htmlAttributesDivider}*>"
            );

            var studentsFullNamesMatcherGroup =
                $@"(?<studentSurname>[a-zá-žA-ZÁ-Ž]+){htmlAttributesDivider}+(?<studentForename>[a-zá-žA-ZÁ-Ž]+)";
            var studentsFullNamesMatcher = new Regex
            (
                $@"<td{htmlAttributesDivider}+{htmlAttributes}\bdata-header{htmlAttributesDivider}*=" +
                $@"{htmlAttributesDivider}*""Por\.\: ""{htmlAttributesDivider}*{htmlAttributes}>{htmlAttributesDivider}*" +
                $@"<b{htmlAttributesDivider}*{htmlAttributes}>{htmlAttributesDivider}*" +
                $@"{studentsOrdersMatcherGroup}\.{htmlAttributesDivider}*<\/b{htmlAttributesDivider}*>{htmlAttributesDivider}*" +
                $@"<a{htmlAttributesDivider}+{htmlAttributes}\bclass{htmlAttributesDivider}*=" +
                $@"{htmlAttributesDivider}*""xg_stag_a_ent ""{htmlAttributesDivider}*{htmlAttributes}>{htmlAttributesDivider}*" +
                $@"{studentsFullNamesMatcherGroup}{htmlAttributesDivider}*<\/a{htmlAttributesDivider}*>"
            );

            var studentsFullPersonalNumbersMatcherGroup =
                @"(?<studentPersonalNumberCodeLetter>[A-Z])(?<studentPersonalNumber>[1-9]\d{7})";
            var studentsFullPersonalNumbersMatcher = new Regex
            (
                $@"<td{htmlAttributesDivider}+{htmlAttributes}\bdata-header{htmlAttributesDivider}*=" +
                $@"{htmlAttributesDivider}*""Fakulta: ""{htmlAttributesDivider}*{htmlAttributes}>{htmlAttributesDivider}*" +
                $@"{studentsFullPersonalNumbersMatcherGroup}"
            );

            var studentsSpecializationsMatcherGroup =
                @"(?<studentSpecialization>[\p{Lu}]+)";
            var studentsSpecializationsMatcher = new Regex
            (
                $@"<td{htmlAttributesDivider}+{htmlAttributes}\bdata-header{htmlAttributesDivider}*=" +
                $@"{htmlAttributesDivider}*""Stav: ""{htmlAttributesDivider}*{htmlAttributes}>{htmlAttributesDivider}*" +
                $@"{studentsSpecializationsMatcherGroup}{htmlAttributesDivider}*<\/td{htmlAttributesDivider}*>"
            );

            htmlStudentsMatchers.Add("studentsOrders", studentsOrdersMatcher);
            htmlStudentsMatchers.Add("studentsFullNames", studentsFullNamesMatcher);
            htmlStudentsMatchers.Add("studentsFullPersonalNumbers", studentsFullPersonalNumbersMatcher);
            htmlStudentsMatchers.Add("studentsSpecializations", studentsSpecializationsMatcher);

            return htmlStudentsMatchers;
        }
    }
}

