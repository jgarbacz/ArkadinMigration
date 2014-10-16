if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[recon_worklist]'))
drop table [dbo].[recon_worklist]

create table recon_worklist (
tracked_id_acc INTEGER,
group_identifier VARCHAR(1000)
 );

