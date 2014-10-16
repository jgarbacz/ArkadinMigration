delete recon_tables
insert into recon_tables values('t_acc_usage','id_acc',1000)
insert into recon_tables values('t_account','id_acc',10)
insert into recon_tables select LOWER(nm_table_name), 'id_sess', 1001 from t_prod_view
insert into recon_tables select LOWER(nm_table_name), 'id_acc', 101 from t_account_view_log

delete recon_join_rules
insert into recon_join_rules values('id_acc',' inner join recon_worklist b on a.id_acc = b.tracked_id_acc where 1 = 1 ')
insert into recon_join_rules values('id_sess',' inner join recon_worklist b on 1 = 1 inner join t_acc_usage c on b.tracked_id_acc = c.id_acc and c.id_view = $${TEMP.id_view} and a.id_sess = c.id_sess where 1 = 1 ')


delete recon_compressed_fields
insert into recon_compressed_fields values('t_account','id_acc','id_acc',1)
insert into recon_compressed_fields values('t_acc_usage','id_payee','id_acc',0)
insert into recon_compressed_fields values('t_acc_usage','id_se','id_acc',0)
insert into recon_compressed_fields values('t_acc_usage','id_sess','id_sess',1)
insert into recon_compressed_fields values('t_acc_usage','id_parent_sess','id_sess',0)
insert into recon_compressed_fields values(NULL,'id_sess','id_sess',0)
insert into recon_compressed_fields values(NULL,'id_acc','id_acc',0)

delete recon_ignored_fields
insert into recon_ignored_fields values('t_acc_usage','dt_crt')
insert into recon_ignored_fields values('t_acc_usage','tx_uid')
insert into recon_ignored_fields values('t_acc_usage','tx_batch')
