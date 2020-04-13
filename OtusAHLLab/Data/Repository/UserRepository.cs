using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using OtusAHLLab.Data.Interfaces;
using OtusAHLLab.Modules.Secutity;

namespace OtusAHLLab.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private string _connectionString;

        public UserRepository(string conn)
        {
            _connectionString = conn;
        }

    

        public void Delete(int id)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                db.Execute("DELETE FROM AspNetUsers WHERE Id = @Id", new { @Id = id });
            }
        }

        public AppUser Get(int id)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                return db.Query<AppUser>("SELECT * FROM AspNetUsers where id=@Id", new {@Id = id}).FirstOrDefault();
            }
        }

        public IEnumerable<AppUser> GetUsers()
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                return db.Query<AppUser>("SELECT * FROM AspNetUsers").ToList();
            }
        }

        public void Update(AppUser user)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                db.Execute(@"UPDATE AspNetUsers
                SET UserName = @UserName,
                    FirstName = @FirstName,
                    LastName = @LastName',
                    Age = @Age,
                    Gender = @Gender,
                    City = @City,
                    Hobby = @Hobby
                WHERE Id = @Id; ", new
                {
                    UserName = user.Email,
                    user.FirstName,
                    user.LastName,
                    user.Age,
                    user.Gender,
                    user.City,
                    user.Hobby,
                    id = user.Id
                });
            }
        }
    }
}

 