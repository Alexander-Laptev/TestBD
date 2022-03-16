using System;
using System.Collections.Generic;
using System.IO;

namespace Тест_csv_
{
    internal class Program
    {
        static void Main()
        {
           
            //хранит таблицу Департамент
            List<Department> department = new List<Department>();
            //хранит таблицу Сотрудники
            List<Employee>employee = new List<Employee>();
            //хранит id руководителей
            List<int>Direct = new List<int>();

            Read(department, employee);
            //1 задание
            Console.WriteLine("Задание 1:");
            Task1(employee, Direct);
            //2 задание
            Console.WriteLine("Задание 2:");
            Task2(department, employee);
            //3 задание
            Console.WriteLine("Задание 3:");
            Task3(employee, Direct);
            
            Console.ReadKey();
        }

        private static void Read(List<Department> department, List<Employee> employee)
        {
            //В строковый массив NameFolder записываются названия файлов, находящихся в папке Documents
            string[] NameFolder = Directory.GetFiles("Documents");

            for (int i = 0; i < NameFolder.Length; i++)
            {
                //Открытие потока для чтения файла
                using (StreamReader sr = new StreamReader(NameFolder[i]))
                {
                    //Если файл Департамент
                    if (NameFolder[i] == "Documents\\department.csv")
                    {
                        string line;
                        int j = 0;
                        while ((line = sr.ReadLine()) != null)
                        {
                            Department NewDepartment = new Department();
                            if (j > 0)
                            {
                                string[] s = line.Split(';');

                                NewDepartment.id = Convert.ToInt32(s[0]);
                                NewDepartment.name = s[1];
                                department.Add(NewDepartment);
                            }
                            j++;
                        }
                    }
                    //Если файл Сотрудники
                    else if (NameFolder[i] == "Documents\\employee.csv")
                    {
                        string line;
                        int j = 0;
                        while ((line = sr.ReadLine()) != null)
                        {
                            Employee NewEmployee = new Employee();
                            if (j > 0)
                            {
                                string[] s = line.Split(';');

                                NewEmployee.id = Convert.ToInt32(s[0]);
                                NewEmployee.depId = Convert.ToInt32(s[1]);
                                //Если поле пустое
                                if (s[2] == "") NewEmployee.chiefId = 0;
                                else NewEmployee.chiefId = Convert.ToInt32(s[2]);
                                NewEmployee.name = s[3];
                                NewEmployee.salary= Convert.ToInt32(s[4]);
                                employee.Add(NewEmployee);
                            }
                            j++;
                        }
                        
                    }
                }
            }
        }
        private static void Task1(List<Employee> employee, List<int> Direct)
        {
            //поиск руководителей в списке сотрудников
            for (int i = 0; i< employee.Count; i++)
            {
                if (Direct.Contains(employee[i].chiefId) == false & employee[i].chiefId != 0)
                {
                    Direct.Add(employee[i].chiefId);
                }
            }
            int SalSum = 0; // Сумма зарплат с руководителями
            for (int i = 0; i < employee.Count; i++)
            {
                SalSum += employee[i].salary;
            }
            int SalDirect = 0; // Сумма зарплат руководителей
            for (int i = 0; i < Direct.Count; i++)
            {
                SalDirect += employee[Direct[i] - 1].salary;
            }
            int SalEmpl = SalSum - SalDirect; // Сумма зарплат сотрудников без руководителей
            Console.WriteLine($"Зарплата всех сотрудников равна = {SalSum}");
            Console.WriteLine($"Зарплата сотрудников без руководителей равна = {SalEmpl}\n");

        }
        private static void Task2(List<Department> department, List<Employee> employee)
        {
            int maxSalDep = 0, imaxSalDep = 0; //Максимальная зарплата Департамента, и его индекс
            for (int i = 0; i < employee.Count; i++)
            {
                if (employee[i].salary > maxSalDep)
                {
                    maxSalDep = employee[i].salary;
                    imaxSalDep = employee[i].depId;
                }
            }
            string Dep = null;
            Dep = department[imaxSalDep - 1].name; // Название департамента с сотруднником с самой высокой зарплатой
            Console.WriteLine($"{Dep} - Департамент, в котором у сотрудника зарплата максимальна\n");
        }
        private static void Task3(List<Employee> employee, List<int> Direct)
        {
            // Сортировка по убыванию зарплат
            for (int i = 0; i < Direct.Count; i++)
            {
                for (int j = 0; j < Direct.Count - 1; j++)
                {
                    if (employee[Direct[j] - 1].salary < employee[Direct[j + 1] - 1].salary)
                    {
                        int temp = Direct[j];
                        Direct[j] = Direct[j + 1];
                        Direct[j + 1] = temp;
                    }
                }
            }
            
            for (int i = 0; i < Direct.Count; i++)
            {
                Console.WriteLine($"Заработная плата {employee[Direct[i] - 1].name}: {employee[Direct[i] - 1].salary}");
            }
        }

        class Department
        {
            public int id;
            public string name;

        }
        
        class Employee
        {
            public int id;
            public int depId;
            public int chiefId;
            public string name;
            public int salary;
        }
    }
}
