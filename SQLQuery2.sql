SELECT DISTINCT
   p.primaryName AS Name, 
   p.birthYear AS BirthYear, 
   p.deathYear AS DeathYear
FROM 
   Names p 
JOIN 
   Principals n
ON 
   p.nameID = n.nameID
WHERE 
   n.job_category = 'writer'

 ORDER BY 
   p.primaryName 

SELECT * FROM Principals; 

SELECT * FROM Names;