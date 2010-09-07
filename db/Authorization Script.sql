/****** Script for SelectTopNRows command from SSMS  ******/
USE [Foundry]
GO
  
DECLARE @expand table(Operation varchar(20), ImpliedOperation varchar(20))

INSERT INTO @expand VALUES('Write', 'Read')
INSERT INTO @expand VALUES('*', 'Read')
INSERT INTO @expand VALUES('*', 'Write')
INSERT INTO @expand VALUES('*', 'Delete')

SELECT derived.[Level],
	derived.Operation,
	derived.SubjectId,
	derived.SubjectName,
	derived.SubjectType,
	rep.Allow
FROM
	[UserPermissionsReports] rep
INNER JOIN
(
  SELECT MAX([Level]) AS [Level]
      ,COALESCE(ex.ImpliedOperation, rep.[Operation]) AS Operation
      ,[SubjectId]
      ,[SubjectName]
      ,[SubjectType]
  FROM [UserPermissionsReports] rep
  LEFT OUTER JOIN @expand ex ON rep.Operation = ex.Operation
  WHERE UserId In('6D685C93-51C8-400E-9227-C1BD3BB9108C', '00000000-0000-0000-0000-000000000000')
  GROUP BY SubjectId, SubjectName, SubjectType, COALESCE(ex.ImpliedOperation, rep.[Operation])
) derived ON rep.Operation = derived.Operation AND 
	rep.Level = derived.Level AND
	rep.SubjectId = derived.SubjectId AND
	rep.SubjectName = derived.SubjectName AND
	rep.SubjectType = derived.SubjectType

