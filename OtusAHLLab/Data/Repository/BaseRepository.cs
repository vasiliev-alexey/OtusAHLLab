using System;
using System.Data;
using Dapper;
using MySql.Data.MySqlClient;

namespace OtusAHLLab.Data.Repository
{
    public class BaseRepository
    {
        private readonly string _conn;


        protected BaseRepository(string conn)
        {
            _conn = conn;
        }


        protected IDbConnection GetDbConnection() => new MySqlConnection(_conn);
    }
}

