#!/bin/bash

echo "⏳ Waiting for SQL Server to be available..."

while true
do
  echo "⏵ Trying: sqlcmd -S db -U sa -P HakanBahsi_123 -Q 'SELECT 1' -C"
  /opt/mssql-tools18/bin/sqlcmd -S db -U sa -P "HakanBahsi_123" -Q "SELECT 1" -C > /dev/null 2>&1
  if [ $? -eq 0 ]; then
    echo "✅ SQL Server is up - applying init.sql"
    break
  else
    echo "❌ SQL Server is unavailable - sleeping"
    sleep 2
  fi
done

echo "📄 Running init.sql..."
/opt/mssql-tools18/bin/sqlcmd -S db -U sa -P "HakanBahsi_123" -d master -i /app/init.sql -b -e -C

echo "🚀 Starting API..."
/usr/bin/dotnet GlassLewisChallange.API.dll