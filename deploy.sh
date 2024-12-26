#!/bin/bash
dotnet publish -c Release
scp -r bin/Release/net9.0/publish/* root@167.99.144.35:~/blokus/
ssh root@167.99.144.35 "./start.sh ; exit"
