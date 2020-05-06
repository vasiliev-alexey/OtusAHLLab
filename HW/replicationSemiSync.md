# Домашнее задание:  Полусинхронная репликация
### Цель:   В результате выполнения ДЗ вы настроите полусинхронную репликацию и убедитесь, что теперь вы не теряете транзакции в случае аварии. В данном задании тренируются навыки: - обеспечение отказоустойчивости проекта; - администрирование MySQL.

1) Настроить 2 слейва и 1 мастер.
2) Включить row-based репликацию.
3) Включить GTID.
4) Настроить полусинхронную репликацию.
5) Создать нагрузку на запись в любую тестовую таблицу. На стороне, которой нагружаем считать, сколько строк мы успешно записали.
6) С помощью kill -9 убиваем мастер MySQL.
7) Заканчиваем нагрузку на запись.
8) Выбираем самый свежий слейв. Промоутим его до мастера. Переключаем на него второй слейв.
9) Проверяем, есть ли потери транзакций.

 Результатом сдачи ДЗ будет отчет в текстовом виде, где вы отразите как вы выполняли каждый из пунктов.
Критерии оценки: Оценка происходит по принципу зачет/незачет.

Требования:
В отчете описано как включить row-based репликацию и GTID
Проведен эксперимент по потере и непотере транзакций при аварийной остановке master. 

#### Журнал работ:
1. Создаем репликацию 2 слейва и 1 мастер через [docker-compose](./src/docker-mysql-master-slave/docker-compose.yml)
2. Настраиваем semisync  репликацию (./src/docker-mysql-master-slave/build.sh)
3. Пишем [скрипт нагрузки](./src/my_sql_insert_load.ps1) 
4. Проводим лаботраторые работы


 
 

### Ответы на ДЗ
1. Создан кластер из 3 инстансов MySQL [docker-compose](./src/docker-mysql-master-slave/docker-compose.yml)
2. Создан [bash-сценарий по настройке](./src/docker-mysql-master-slave/build.sh)

#####    Алгоритм настройки  
    1. На mysql_master  инсталируем плагин, высталям конфиги:

            INSTALL PLUGIN rpl_semi_sync_master SONAME 'semisync_master.so';  
            SET GLOBAL rpl_semi_sync_master_enabled = 1;   
            SET GLOBAL rpl_semi_sync_master_timeout = 5000;

        Конфиг [мастера](./src/docker-mysql-master-slave/master/conf/mysql.conf.cnf )


                server-id = 1
                # включаем gtid
                gtid-mode = on
                enforce_gtid_consistency = on

                log_bin = /var/log/mysql/mysql-bin.log
                binlog_format = ROW
                binlog_do_db = mydb
 



    2. Перезапускаем mysql_master
    3. На mysql_slave_1 и mysql_slave_2  интсалируем плагин, высталям конфиги:

            INSTALL PLUGIN rpl_semi_sync_slave SONAME "semisync_slave.so";
            SET GLOBAL rpl_semi_sync_slave_enabled = 1;


        Конфиг [слейва](./src/docker-mysql-master-slave/slave_1/conf/mysql.conf.cnf )


                [mysqld]

                skip-host-cache
                skip-name-resolve

                server-id=2

                gtid-mode = on
                enforce_gtid_consistency = on

                relay-log = /var/log/mysql/mysql-relay-bin.log
                log_bin = /var/log/mysql/mysql-bin.log
                binlog_do_db = mydb


    4. Промоутим мастер на слейвах  

            CHANGE MASTER TO
            MASTER_HOST='mysql_master',
            MASTER_USER='mydb_slave_user',
            MASTER_PASSWORD='mydb_slave_pwd',
            MASTER_LOG_FILE='mysql-bin.00000X',
            MASTER_LOG_POS= xxx;
    5. если все заведется т в логах можно увидеть  

            mysql_master     | 2020-05-05T19:29:37.696096Z 3 [Note] Start asynchronous binlog_dump to slave (server_id: 2), pos(mysql-bin.000002, 154)
    6. или  
          show slave status

            Slave_SQL_Running Yes  
            Slave_IO_Running  Yes

##### Эксперименты с транзакциями
1. Запускаем [скрипт нагрузки](./src/my_sql_insert_load.ps1) 
2. Убиваем контейнер  с мастером  

        docker kill mysql_master  
3. Скрипт нагрузки показывает последний удачно записанный ID -  показал 6200
4. На обоих слевах увы оказалось 6119 - потеряли
5. Промоутим mysql_slave_1  на mysql_slave_2 - меняя конфиги от мастера на слейв

        STOP SLAVE FOR CHANNEL '';
        reset slave;

        CHANGE MASTER TO
        MASTER_HOST='mysql_slave_1',
        MASTER_USER='mydb_slave_user',
        MASTER_PASSWORD='mydb_slave_pwd',
        MASTER_AUTO_POSITION = 1;

        start slave;
6. Перенакат транзакций со слейва увы не удался ни в одной и попыток


#### Материалы:
- [Настройка репликации в Mysql](http://www.rldp.ru/mysql/mysql80/replica.htm)
- [Как промоутить slave](https://severalnines.com/blog/mysql-slave-promotion-with-and-without-using-gtid)