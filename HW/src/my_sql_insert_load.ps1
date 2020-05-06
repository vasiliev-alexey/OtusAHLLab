
#подключаем библиотеку MySql.Data.dll
Add-Type –Path ‘D:\tmp\v4.5.2\MySql.Data.dll'
# строка подключения к БД, server - имя севрера, uid - имя mysql пользователя, pwd- пароль, database - имя БД на сервере
$Connection = [MySql.Data.MySqlClient.MySqlConnection]@{ConnectionString='server=192.168.1.68;port=4406;uid=root;pwd=111;database=mydb'}
$Connection.Open()
 
$i = 0



Do {

try {

$sql = New-Object MySql.Data.MySqlClient.MySqlCommand
$sql.Connection = $Connection

#записываем информацию в таблицу БД
$sql.CommandText = "INSERT INTO code1 VALUES (@val)"
$sql.Prepare()

$sql.Parameters.AddWithValue("@val", $i ) | out-null
$sql.ExecuteNonQuery() | out-null


#Start-Sleep  -Milliseconds 10
$i++ 

} Catch
{
Write-host $i
break;
}

} while ($True)

$Connection.Close()