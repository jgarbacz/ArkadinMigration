create procedure mvm_get_id_pricelist @my_id_acc INT, @my_id_sub INT, @my_id_pt INT, @my_id_pricelist INT OUT as
declare
    @my_currency_code VARCHAR(100);

select @my_id_pricelist = isnull(min(pm.id_pricelist),0) from t_pl_map pm
        inner join t_pricelist pl on pm.id_pricelist = pl.id_pricelist and pl.n_type = 0 
        where id_sub = @my_id_sub and id_paramtable = @my_id_pt;
    IF (@my_id_pricelist = 0) BEGIN
    	select @my_id_pricelist = isnull(min(pm.id_pricelist),0) from t_pl_map pm
        	inner join t_pricelist pl on pm.id_pricelist = pl.id_pricelist and pl.n_type = 0 
        	where id_sub = @my_id_sub;
    END;
    IF (@my_id_pricelist = 0) BEGIN
        select @my_currency_code = c_currency from t_av_internal where id_acc = @my_id_acc;
        insert into t_base_props ( n_kind, n_name, n_desc, nm_name, nm_desc, b_approved, b_archive, n_display_name, nm_display_name
        ) values( 150, 0, 0, NULL, NULL, 'N', 'N', 0, NULL);
	select @my_id_pricelist = @@identity
        insert into t_pricelist (id_pricelist, n_type, nm_currency_code) 
            values( @my_id_pricelist, 0, @my_currency_code);
    END;
go
