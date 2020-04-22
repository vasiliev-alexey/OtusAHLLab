#!/bin/bash
echo ssss

 wrk -c1 -t1 -d5s https://192.168.1.50:5001/api/users?firstname=%D0%91%D0%B5%D1%85%D0%B7%D0%BE%D0%B4&lastName=%


 echo 100

  wrk -c10 -t10 -d50s https://192.168.1.50:5001/api/users?firstname=%D0%91%D0%B5%D1%85%D0%B7%D0%BE%D0%B4&lastName=%

 
 