if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[recon_ignored_fields]'))
drop table [dbo].[recon_ignored_fields]

create table recon_ignored_fields (
table_name VARCHAR(100),
field_name VARCHAR(1000)
 );

