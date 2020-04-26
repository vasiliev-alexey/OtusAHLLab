using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using OtusAHLLab.Data.Interfaces;
using OtusAHLLab.Modules.Enums;
using OtusAHLLab.Modules.Secutity;

namespace OtusAHLLab.Data.Repository
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(string conn, IEnumerable<string> readOnlyConnList) : base(conn, readOnlyConnList)
        {
        }


        public void Delete(int id)
        {
            using (var db = GetDbConnection())
            {
                db.Execute("DELETE FROM AspNetUsers WHERE Id = @Id", new {@Id = id});
            }
        }

        public AppUser Get(int id)
        {
            using (var db = GetDbConnection())
            {
                return db.Query<AppUser>("SELECT * FROM AspNetUsers where id=@Id", new {@Id = id}).FirstOrDefault();
            }
        }

        public IEnumerable<AppUser> GetUsers(StatusCode statusCode)
        {
            var sql = "SELECT * FROM AspNetUsers limit 10";
            if (statusCode != StatusCode.All)
                sql = @"SELECT * FROM AspNetUsers u join  
                            friendship f on u.Id = f.user_two_id 
                                and f.status = ${statusCode}  limit 10";

            using (var db = GetDbConnection())
            {
                return db.Query<AppUser>(sql).ToList();
            }
        }

        /// <summary>
        /// Метод возвращает список анкет-кандидатов для дружбы, для текущего пользователя
        /// </summary>
        /// <param name="currentUser">Идентификатор текущего пользователя</param>
        /// <returns></returns>
        public IEnumerable<AppUser> GetCandidateUsers(long currentUser)
        {
            var sql = @"SELECT *
                            FROM AspNetUsers u
                            where u.Id <> @Id
                                and u.Id not in 
                            (select f.user_two_id from friendship f where f.user_one_id =@Id
                                   union all
                                   select f.user_one_id from friendship f where f.user_two_id = @Id)
                    limit 100    
                    ";


            using var db = GetReadOnlyDbConnection();
            return db.Query<AppUser>(sql, new {@Id = currentUser});
        }


        public IEnumerable<AppUser> GetReqFriendShips(long currentUser)
        {
            var sql = $" SELECT * FROM AspNetUsers u    where u.Id  in " +
                      $"(select  f.user_two_id from friendship f  where f.status = 0 " +
                      $"and f.user_one_id = @Id)";


            using var db = GetDbConnection();
            return db.Query<AppUser>(sql, new {@Id = currentUser});
        }

        public IEnumerable<AppUser> GetFriends(long curAppuserId)
        {
            var sql = @"SELECT *
                        FROM AspNetUsers u
                               join friendship f on f.status = 1
                              and ((f.user_one_id = @Id and u.Id = f.user_two_id) or
                                   (f.user_two_id = @Id and u.Id = f.user_one_id))";


            using var db = GetDbConnection();
            return db.Query<AppUser>(sql, new {@Id = curAppuserId});
        }

        public IEnumerable<AppUser> GetIncomeReqFriendShips(long curAppUserId)
        {
            var sql = @" SELECT * FROM AspNetUsers u    where u.Id  in  
                              (select  f.user_one_id from friendship f  where f.status = 0  
                               and f.user_two_id = @Id)";


            using var db = GetDbConnection();
            return db.Query<AppUser>(sql, new {@Id = curAppUserId});
        }

        /// <summary>
        /// Метод возвращает список анкет с фильтром по имени (firstNamePattern) и фамилии (lastNamePatterns)
        /// </summary>
        /// <param name="firstNamePattern"></param>
        /// <param name="lastNamePatterns"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AppUser>> GetCandidatesByNames(string firstNamePattern, string lastNamePatterns)
        {
            var sql = @" select * from
                            AspNetUsers u 
                            where u.FirstName like  @FirstNamePattern and u.LastName like @LastNamePatterns
                            limit 1000
                        ";
            using var db = GetReadOnlyDbConnection();
            var users = await db.QueryAsync<AppUser>(sql,
                new {FirstNamePattern = firstNamePattern, LastNamePatterns = lastNamePatterns});
            return users;
        }

        public void Update(AppUser user)
        {
            using (var db = GetDbConnection())
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