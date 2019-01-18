-- Quering the "WideHistory" table in "INSQL" liked server
DECLARE @tagNames NVARCHAR(MAX) = '',
@openQuery NVARCHAR(MAX),
@tsql NVARCHAR(MAX),
@linkedServer NVARCHAR(MAX) = 'INSQL';

SELECT @tagNames += QUOTENAME(TagName, '[') + ','
FROM TagRef
WHERE TagName LIKE '%Mixer%';

SET @tagNames = LEFT(@tagNames, LEN(@tagNames) - 1)
SET @tsql = 'SELECT DateTime, ' + @tagNames + '
             FROM WideHistory
             WHERE DateTime > DATEADD(HH, -4, GETDATE())';

SET @openQuery = 'SELECT *
                  FROM OPENQUERY(' + @linkedServer + ','' ' + @tsql + ' '' )
                  ORDER BY DateTime ASC';

EXECUTE (@openQuery);