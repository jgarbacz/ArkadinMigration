if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[recon_join_rules]'))
drop table [dbo].[recon_join_rules]

create table recon_join_rules (
join_rule VARCHAR(100),
expression VARCHAR(1000)
 );

