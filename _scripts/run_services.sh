  
#!/bin/bash

Services=(Services.Identity Services.Customer Services.Notification)

cd ../src

for Service in ${Services[*]}
do	
	 echo ========================================================
	 echo Running the service: $Service
	 echo ========================================================
	 cd $Service
     dotnet run &
	 cd ..
done

read