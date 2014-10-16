if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[recon_tables]'))
drop table [dbo].[recon_tables]

create table recon_tables (
table_name VARCHAR(1000),
join_rule VARCHAR(1000),
table_order INTEGER
 );

