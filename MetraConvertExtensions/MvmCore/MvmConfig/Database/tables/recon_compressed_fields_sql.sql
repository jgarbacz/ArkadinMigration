if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[recon_compressed_fields]'))
drop table [dbo].[recon_compressed_fields]

create table recon_compressed_fields (
table_name VARCHAR(100),
field_name VARCHAR(1000),
compression_key VARCHAR(1000),
is_master INTEGER
 );

