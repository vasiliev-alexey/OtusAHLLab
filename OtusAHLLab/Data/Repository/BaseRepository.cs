using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;
using OtusAHLLab.Utils;

namespace OtusAHLLab.Data.Repository
{
    public class BaseRepository
    {
        private readonly string _conn;
        private readonly IEnumerable<string> _readOnlyConnList;


        protected BaseRepository(string conn, IEnumerable<string> readOnlyConnList =  null)
        {
            _conn = conn;
            _readOnlyConnList = readOnlyConnList;
        }


        protected IDbConnection GetDbConnection() => new MySqlConnection(_conn);

        protected IDbConnection GetReadOnlyDbConnection()
        {
            if (_readOnlyConnList != null && _readOnlyConnList.Any())
                return new MySqlConnection(_readOnlyConnList.Shuffle().First());


            return new MySqlConnection(_conn);
        }
    }
}

