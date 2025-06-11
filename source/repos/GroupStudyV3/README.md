---------Project Description---------

This web app was built to help students organize and collaborate on coursework.
It lets students register, track assignments by course, and create or join study groups with others taking similar classes.

Key features:

Student Profiles: Add yourself as a student and update your information any time.
Assignment Tracker: Log your assignments, set the type (Homework, Quiz, Test, Project), and keep track of your academic workload.
Smart Group Formation: Search for students by course, assignment type, gender, or class standing to find study partners who match your criteria.
Group Dashboard: Create new groups or join existing ones. All group members can see each other’s assignments for the shared course, making it easier to collaborate and stay organized.
Edit Members & Info: Update your group’s members, remove yourself from groups, or edit your profile information whenever you want.
This project was made for learning and demo purposes


---------Setting Up Project Environment---------

Needed: Docker, Visual Studio, optionally SQL Server Management Studio (SSMS) to modify database.

1. Clone the Repository:
    git clone (url of repo)
    cd your-repo-name

2. Start SQL Server Docker Container:
    docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Version2!" -p 1433:1433 --name groupstudy-sql -d mcr.microsoft.com/mssql/server:2022-latest

3. Add the Database Script to the Container (database_script.sql):
    docker cp database_script.sql groupstudy-sql:/database_script.sql

4. Run Database Script inside container:
    docker exec -it groupstudy-sql /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P Version2! -d master -i /database_script.sql

5. (if needed) Update connection string in appsettings.json:
    "ConnectionStrings": {
        "GroupStudyV2Context" : "Server=localhost,1433;Database=GroupStudy;User Id=SA;Password=Version2!;"
    }

6. Open Solution in Visual Studio then build run.



If you want to use SQL Server Management Studio (SSMS) to modify and view database:

    - Install SQL Server Management Studio (SSMS) 
    - Open SSMS click "Connect" in the Object Explorer -> "Database Engine"
    - Connect to SQL Server:
    - Server: localhost,1433
    - Authentication: SQL Server Authentication
    - Login: SA
    - Password: Version2!
