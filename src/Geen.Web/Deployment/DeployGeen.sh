#!/bin/bash 

rm -r /home/zoxexivo/deploy_tmp/geen
mkdir /home/zoxexivo/deploy_tmp/geen
cd /home/zoxexivo/deploy_tmp/geen

git clone https://ZOXEXIVO@bitbucket.org/ZOXEXIVO/geen.git
cd geen

docker stop geen 
docker rm geen 
docker rmi geen 

docker build -t geen -f "build/Web.Dockerfile" .

docker run -d -p 172.17.0.1:7000:7000 --volume /home/zoxexivo/static/htmls:/html/geen --restart always --name geen geen
