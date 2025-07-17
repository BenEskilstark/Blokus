#!/bin/bash
dotnet publish -c Release
scp -r bin/Release/net9.0/publish/* root@161.35.14.17:~/Blokus/
ssh root@161.35.14.17 "~/Blokus/start.sh ; exit"
