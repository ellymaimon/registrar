using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Registrar;

namespace Registrar.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Department(string name, int id = 0)
        {
            Id = id;
            Name = name;
        }

        public override bool Equals(System.Object otherDepartment)
        {
            if (!(otherDepartment is Department))
            {
                return false;
            }
            else
            {
                Department newDepartment = (Department) otherDepartment;
                bool idEquality = (this.Id == newDepartment.Id);
                bool nameEquality = (this.Name == newDepartment.Name);
                return (idEquality && nameEquality);
            }
        }

        public static List<Department> GetAll()
        {
            List<Department> allDepartments = new List<Department> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM departments;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int departmentId = rdr.GetInt32(0);
                string departmentName = rdr.GetString(1);

                Department newDepartment = new Department (departmentName, departmentId);
                allDepartments.Add(newDepartment);
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allDepartments;
        }

        public static void DeleteAll()
        {
            List<Department> allDepartments = new List<Department> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM departments;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO departments (name) VALUES (@departmentName);";
            cmd.Parameters.AddWithValue("@departmentName", this.Name);
            cmd.ExecuteNonQuery();

            Id = (int) cmd.LastInsertedId;

            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
        }

        public static Department Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM departments WHERE id = @departmentId;";
            cmd.Parameters.AddWithValue("@departmentId", id);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

            int departmentId = 0;
            string departmentName = "";

            while (rdr.Read())
            {
                departmentId = rdr.GetInt32(0);
                departmentName = rdr.GetString(1);
            }

            Department foundDepartment = new Department(departmentName, departmentId);
            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
            return foundDepartment;
        }
    }
}
