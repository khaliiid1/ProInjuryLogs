SELECT DISTINCT TeamName From [dbo].[athletes] WHERE Sports = 'Football';

SELECT FirstName, LastName, InjuryType FROM Athletes Join Injury on Athletes.Athlete_ID = Injury.Athlete_ID Where InjuryType Like 'Knee%';

SELECT LastName, FirstName, Sports, TeamName From [dbo].[Athletes] ORDER BY TeamName ASC;

SELECT Lastname, FirstName, InjuryType, StartDate, RecoveryDate FROM [dbo].[Athletes] JOIN Injury ON Athletes.Athlete_ID = Injury.Athlete_ID WHERE Athletes.Athlete_ID = 5;

SELECT LastName, FirstName, StartDate, RecoveryDate FROM [dbo].[Athletes], [dbo].[Injury] WHERE [dbo].[Athletes].[Athlete_ID] = [dbo].[Injury].[Athlete_ID] AND StartDate >= '2026-01-01' AND StartDate <= '2026-02-13';

SELECT SportName, LeagueName, TeamsCount FROM [dbo].[Sports] ORDER BY SportName;

SELECT TeamName, LeagueName, InjuredPlayerCount FROM dbo.Team ORDER BY InjuredPlayerCount DESC, TeamName ASC;

SELECT [dbo].[Injury].[InjuryType], DATEDIFF(day, [dbo].[Injury].[StartDate], [dbo].[Injury].[RecoveryDate]) AS DaysDuration FROM [dbo].[Injury];

SELECT 
    [dbo].[Athletes].[FirstName], 
    [dbo].[Athletes].[LastName], 
    [dbo].[Injury].[InjuryType]
FROM 
    [dbo].[Athletes], 
    [dbo].[Injury]
WHERE 
    [dbo].[Athletes].[Athlete_ID] = [dbo].[Injury].[Athlete_ID];

SELECT 
    [dbo].[Athletes].[FirstName], 
    [dbo].[Athletes].[LastName], 
    [dbo].[Athletes].[TeamName]
FROM 
    [dbo].[Athletes]
WHERE 
    [dbo].[Athletes].[LastName] LIKE 'Smit%';

SELECT 
    [dbo].[Sports].[SportName], 
    [dbo].[Sports].[LeagueName], 
    [dbo].[Sports].[ManagersCount]
FROM 
    [dbo].[Sports]
ORDER BY [dbo].[Sports].[ManagersCount] DESC;

SELECT 
    dbo.Sports.SportName, 
    dbo.Sports.LeagueName, 
    dbo.Sports.ManagersCount
FROM 
    dbo.Sports
WHERE 
    dbo.Sports.ManagersCount < 10
ORDER BY dbo.Sports.ManagersCount DESC;

SELECT 
    dbo.Athletes.FirstName, 
    dbo.Athletes.LastName, 
    dbo.Athletes.phone, 
    dbo.Athletes.TeamName
FROM 
    dbo.Athletes
WHERE 
    dbo.Athletes.TeamName LIKE 'Warrior%';

SELECT 
    dbo.Athletes.FirstName, 
    dbo.Athletes.LastName, 
    dbo.Athletes.Sports, 
    dbo.Athletes.TeamName
FROM 
    dbo.Athletes
WHERE 
    dbo.Athletes.LastName LIKE 'S%';

SELECT 
    dbo.Athletes.Athlete_ID,
    dbo.Athletes.FirstName, 
    dbo.Athletes.LastName, 
    dbo.Athletes.phone,
    dbo.Athletes.TeamName
FROM 
    dbo.Athletes
WHERE 
    dbo.Athletes.LastName NOT LIKE 'S%';