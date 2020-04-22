# Домашняя работа по теме индексы

Задача:
1)   Сгенерировать любым способ 1,000,000 анкет. Имена и Фамилии должны быть реальными (чтобы учитывать селективность индекса)
2) Реализовать функционал поиска анкет по префиксу имени и фамилии (одновременно) в вашей социальной сети (запрос в форме firstName LIKE ? and secondName LIKE ?). Сортировать вывод по id анкеты. Использовать InnoDB движок.
3) С помощью wrk провести нагрузочные тесты по этой странице. Поиграть с количеством одновременных запросов. 1/10/100/1000.
4) Построить графики и сохранить их в отчет
5) Сделать подходящий индекс.
6) Повторить пункт 3 и 4.
7) В качестве результата предоставить отчет в котором должны быть:
    - графики latency до индекса;
    - графики throughput до индекса;
    - графики latency после индекса;
    - графики throughput после индекса;
    - запрос добавления индекса;
    - explain запросов после индекса;
    - объяснение почему индекс именно такой;


Решение:
1. Сгенерировали 1_000_000 анткет, используя БД [имен и фамилий](https://mydata.biz/storage/download/346359f64cc54ff1a2af506561987ebe/%D0%91%D0%B0%D0%B7%D0%B0%20%D0%B4%D0%B0%D0%BD%D0%BD%D1%8B%D1%85%20%D0%B8%D0%BC%D0%B5%D0%BD%20%D0%B8%20%D1%84%D0%B0%D0%BC%D0%B8%D0%BB%D0%B8%D0%B9%20%D0%B2%20%D1%84%D0%BE%D1%80%D0%BC%D0%B0%D1%82%D0%B5%20MySql.zip)

2. Создаем контроллер прнимающий 2 текстовых параметра и выозращающий  json c  отбранными  анкетами: 
        
        UsersController.Get(string firstName, string lastName)

3. Тестовый URL для контроллера:  
        
        https://localhost:5001/api/users?firstname=Бехзод&lastName=%

4. Создаем метод в репозитории доступа по  sql выражению

        public IEnumerable<AppUser> GetCandidatesByNames(string firstNamePattern, string lastNamePatterns)
        {
            var sql = @" select * from
                            aspnetusers u 
                            where u.FirstName like  @FirstNamePattern and u.LastName like @LastNamePatterns
                            limit 1000
                        ";
            using var db = GetDbConnection();
            return db.Query<AppUser>(sql, new { FirstNamePattern = firstNamePattern, LastNamePatterns = lastNamePatterns });
        }


5.   [Устанавливаем на Linux wrk](https://github.com/wg/wrk/wiki/Installing-Wrk-on-Linux)
6.   Пример для запуска  

         wrk -c1000 -t30 -d60s  --timeout 30s --latency   https://192.168.1.50:5001/api/users?firstname=%D0%90%D0%BB%D0%B5%&lastName=%D0%9A%D0%B0%D0%B7%

7. Создаем индекс:

        create index fn_ln on aspnetusers (FirstName,LastName)
8. Прогоняем тесты нагрузки повторно

План по индексу:
Explain:

        {
        "query_block": {
            "select_id": 1,
            "cost_info": {
            "query_cost": "32436.11"
            },
            "table": {
            "table_name": "u",
            "access_type": "range",
            "possible_keys": [
                "fn_ln"
            ],
            "key": "fn_ln",
            "used_key_parts": [
                "FirstName"
            ],
            "key_length": "404",
            "rows_examined_per_scan": 47700,
            "rows_produced_per_join": 5299,
            "filtered": "11.11",
            "index_condition": "((`otushla`.`u`.`FirstName` like 'Ал%') and (`otushla`.`u`.`LastName` like 'Каз%'))",
            "using_MRR": true,
            "cost_info": {
                "read_cost": "31906.17",
                "eval_cost": "529.95",
                "prefix_cost": "32436.12",
                "data_read_per_join": "45M"
            },
            "used_columns": [
                "Id",
                "UserName",
                "NormalizedUserName",
                "Email",
                "NormalizedEmail",
                "EmailConfirmed",
                "PasswordHash",
                "SecurityStamp",
                "ConcurrencyStamp",
                "PhoneNumber",
                "PhoneNumberConfirmed",
                "TwoFactorEnabled",
                "LockoutEnd",
                "LockoutEnabled",
                "AccessFailedCount",
                "FirstName",
                "LastName",
                "Age",
                "Gender",
                "City",
                "Hobby"
            ]
            }
        }
        }

Latency  без индексов  
 ![Alt text](https://github.com/vasiliev-alexey/OtusAHLLab/blob/master/HW/src/lat_with_index.png)

графики throughput до индекса;  
 ![Alt text](https://github.com/vasiliev-alexey/OtusAHLLab/blob/master/HW/src/throughput_without_index.png)

Latency  c индексами  
 ![Alt text](https://github.com/vasiliev-alexey/OtusAHLLab/blob/master/HW/src/lat_with_index.png)

 графики throughput с индексом;  
 ![Alt text](https://github.com/vasiliev-alexey/OtusAHLLab/blob/master/HW/src/throughput_with_index.png)

 
