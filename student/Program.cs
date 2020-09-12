using System;
using System.Collections.Generic;

namespace student
{
    class Student
    {
        public string Name;
        public string Gender;
        public string City;
        public string Country;
        public string Subject;

        public override string ToString()
        {
            return $"{Name}: {Gender}, {City}, {Country}, {Subject}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var students = new Student[]
            {
                new Student
                {
                    Name = "Carol",
                    Gender = "Girl"
                },
                new Student
                {
                    Name = "Elisa",
                    Gender = "Girl"
                },
                new Student
                {
                    Name = "Lucas",
                    Gender = "Boy"
                },
                new Student
                {
                    Name = "Oliver",
                    Gender = "Boy"
                }
            };

            var cities = new string[] { "Cambridge", "Edinburgh", "London", "Oxford" };
            var countries = new string[] { "Australia", "Canada", "SouthAfrica", "USA" };
            var subjects = new string[] { "Architecture", "History", "Law", "Medicine" };

            foreach (var cityPermutation in Permute(students.Length))
            {
                foreach (var countryPermutation in Permute(students.Length))
                {
                    foreach (var subjectPermutation in Permute(students.Length))
                    {
                        for (var i = 0; i < students.Length; i++)
                        {
                            students[i].City = cities[cityPermutation[i]];
                            students[i].Country = countries[countryPermutation[i]];
                            students[i].Subject = subjects[subjectPermutation[i]];
                        }

                        Validate(students);
                    }
                }
            }
        }

        private static List<int[]> Permute(int count)
        {
            var permutations = new List<int[]>();

            var permutation = new int[count];
            for (int i = 0; i < count; i++)
            {
                permutation[i] = i;
            }

            Permute(permutations, 0, permutation.Length - 1, permutation);

            return permutations;
        }

        private static void Permute(List<int[]> permutations, int start, int end, int[] permutation)
        {
            if (start == end)
            {
                permutations.Add((int[])permutation.Clone());
            }
            else
            {
                for (var i = start; i <= end; i++)
                {
                    Swap(permutation, start, i);
                    Permute(permutations, start + 1, end, permutation);
                    Swap(permutation, start, i);
                }
            }
        }

        private static void Swap(int[] permutation, int i, int j)
        {
            if (i == j)
            {
                return;
            }

            int temp = permutation[i];
            permutation[i] = permutation[j];
            permutation[j] = temp;
        }

        private static void Validate(Student[] students)
        {

            int girlCount = 0;
            int boyCount = 0;

            bool oneBoyFromAustralia = false;
            bool oneBoyStudyArchitecture = false;

            bool oneGirGotoCambridge = false;
            bool oneGirlStudyMedicine = false;

            foreach (var student in students)
            {
                // Rule 1
                // Exactly one boy and one girl choose a university in a city with the same initial of their names.
                if (student.City[0] == student.Name[0])
                {
                    if (student.Gender == "Girl")
                    {
                        girlCount++;
                    }
                    else
                    {
                        boyCount++;
                    }
                }

                // Rule 2
                // A boy is from Australia, the other studies Architecture.
                if (student.Gender == "Boy")
                {
                    if (student.Country == "Australia"
                    && student.Subject != "Architecture")
                    {
                        oneBoyFromAustralia = true;
                    }

                    if (student.Country != "Australia"
                    && student.Subject == "Architecture")
                    {
                        oneBoyStudyArchitecture = true;
                    }
                }

                // Rule 3
                // A girl goes to Cambridge, the other studies Medicine.
                if (student.Gender == "Girl")
                {
                    if (student.City == "Cambridge"
                    && student.Subject != "Medicine")
                    {
                        oneGirGotoCambridge = true;
                    }

                    if (student.Country != "Cambridge"
                    && student.Subject == "Medicine")
                    {
                        oneGirlStudyMedicine = true;
                    }
                }

                // Rule 4
                // Oliver studies Law or is from USA. He is not from South Africa.
                if (student.Name == "Oliver")
                {
                    if ((student.Subject != "Law" && student.Country != "USA")
                        || (student.Country == "SouthAfrica"))
                    {
                        return;
                    }
                }

                // Rule 5 && Rule 9
                // The student from Canada is either a historian or will go to Oxford.
                // The Canadian is not studying Law.
                if (student.Country == "Canada")
                {
                    if ((student.Subject != "History" && student.City != "Oxford")
                        || (student.Subject == "Law"))
                    {
                        return;
                    }
                }

                // Rule 6
                // The student from USA will go to Edinburgh or will study Medicine.
                if (student.Country == "USA")
                {
                    if (student.City != "Edinburgh" && student.Subject != "Medicine")
                    {
                        return;
                    }
                }

                // Rule 7
                // Lucas is not from USA and will not study History.
                if (student.Name == "Lucas")
                {
                    if (student.Country == "USA" || student.Subject == "History")
                    {
                        return;
                    }
                }

                // Rule 8
                // The student from South Africa is going to Edinburgh or will study Law
                if (student.Country == "SouthAfrica")
                {
                    if (student.City != "Edinburgh" && student.Subject != "Law")
                    {
                        return;
                    }
                }
            }

            if (girlCount != 1 || boyCount != 1)
            {
                return;
            }

            if (!oneBoyFromAustralia || !oneBoyStudyArchitecture)
            {
                return;
            }

            if (!oneGirGotoCambridge || !oneGirlStudyMedicine)
            {
                return;
            }

            Console.WriteLine("Found one!");
            foreach (var student in students)
            {
                Console.WriteLine(student);
            }
        }
    }
}
