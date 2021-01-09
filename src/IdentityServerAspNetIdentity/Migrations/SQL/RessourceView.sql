CREATE OR REPLACE VIEW Ressources AS

/*
SELECT 
	"HomeworkV2Id" AS "Id",
	"SchoolClassRoomName" || ' Session ' || "SessionNumberId" || ' ' || EXTRACT(YEAR FROM "HomeworkV2Date") AS "SessionNumber",
	"HomeworkV2Date" AS "Date", 
	"HomeworkV2Name" AS "Name", 
	"HomeworkTypeName" , 
	"Firstname" AS "Teacher", 
	'H' AS "RessourceType"
FROM public."HomeworkV2s"
NATURAL JOIN "HomeworkTypes"
NATURAL JOIN "Sessions"
NATURAL JOIN "SchoolClassRooms"
LEFT JOIN "AspNetUsers" ON "HomeworkV2s"."TeacherId" = "Id"

UNION
*/

SELECT 
	"TheoryId" AS "Id",
	"SchoolClassRoomName" || ' Session ' || "SessionNumberId" || ' ' || EXTRACT(YEAR FROM "HomeworkV2Date") AS "SessionNumber", 
	"HomeworkV2Date" AS "Date", 
	"HomeworkV2Name" AS "Name", 
	"HomeworkTypeName" , 
	"Firstname" AS "Teacher", 
	'T' AS "RessourceType"
FROM public."Theories"
NATURAL JOIN "HomeworkV2s"
NATURAL JOIN "HomeworkTypes"
NATURAL JOIN "Sessions"
NATURAL JOIN "SchoolClassRooms"
LEFT JOIN "AspNetUsers" ON "HomeworkV2s"."TeacherId" = "Id"

UNION

SELECT 
	"ExerciceId" AS "Id",
	"SchoolClassRoomName" || ' Session ' || "SessionNumberId" || ' ' || EXTRACT(YEAR FROM "HomeworkV2Date") AS "SessionNumber",
	"HomeworkV2Date" AS "Date", 
	"HomeworkV2Name" AS "Name", 
	"HomeworkTypeName" , 
	"Firstname" AS "Teacher", 
	'E' AS "RessourceType"
FROM public."Exercices"
LEFT JOIN "Theories" ON "Theories"."TheoryId" = "Exercices"."TheoryId"
LEFT JOIN "HomeworkV2s" ON "Theories"."HomeworkV2Id" = "HomeworkV2s"."HomeworkV2Id"
NATURAL JOIN "HomeworkTypes"
NATURAL JOIN "Sessions"
NATURAL JOIN "SchoolClassRooms"
LEFT JOIN "AspNetUsers" ON "HomeworkV2s"."TeacherId" = "Id"

UNION

SELECT 
	"ExerciceId" AS "Id",
	"SchoolClassRoomName" || ' Session ' || "SessionNumberId" || ' ' || EXTRACT(YEAR FROM "HomeworkV2Date") AS "SessionNumber", 
	"HomeworkV2Date" AS "Date", 
	"HomeworkV2Name" AS "Name", 
	"HomeworkTypeName" , 
	"Firstname" AS "Teacher", 
	'EA' AS "RessourceType"
FROM public."ExercicesAlone"
LEFT JOIN "HomeworkV2s" ON "ExercicesAlone"."HomeworkV2Id" = "HomeworkV2s"."HomeworkV2Id"
NATURAL JOIN "HomeworkTypes"
NATURAL JOIN "Sessions"
NATURAL JOIN "SchoolClassRooms"
LEFT JOIN "AspNetUsers" ON "HomeworkV2s"."TeacherId" = "Id";
