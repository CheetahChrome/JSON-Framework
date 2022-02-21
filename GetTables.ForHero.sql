select TABLE_SCHEMA + '.' + TABLE_NAME as [Table]
from information_schema.tables
where TABLE_SCHEMA <> 'sys' and TABLE_SCHEMA <> 'view';