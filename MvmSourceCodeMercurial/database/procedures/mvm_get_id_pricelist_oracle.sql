create or replace procedure mvm_get_id_pricelist(my_id_acc number, my_id_sub number, my_id_pt number, my_id_pricelist out number) as
    my_currency_code varchar2(100);
begin
    select nvl(min(pm.id_pricelist),0) into my_id_pricelist from t_pl_map pm
        inner join t_pricelist pl on pm.id_pricelist = pl.id_pricelist and pl.n_type = 0
        where id_sub = my_id_sub and id_paramtable = my_id_pt;
    if (my_id_pricelist = 0) then
      select nvl(min(pm.id_pricelist),0) into my_id_pricelist from t_pl_map pm
          inner join t_pricelist pl on pm.id_pricelist = pl.id_pricelist and pl.n_type = 0
          where id_sub = my_id_sub;
    end if;
    if (my_id_pricelist = 0) then
        select c_currency into my_currency_code from t_av_internal where id_acc = my_id_acc;
        select seq_t_base_props.nextval into my_id_pricelist from dual;
        insert into t_base_props ( id_prop, n_kind, n_name, n_desc, nm_name, nm_desc, b_approved, b_archive, n_display_name, nm_display_name
        ) values( my_id_pricelist, 150, 0, 0, null, null, 'n', 'n', 0, null);
        insert into t_pricelist (id_pricelist, n_type, nm_currency_code)
            values( my_id_pricelist, 0, my_currency_code);
    end if;
end mvm_get_id_pricelist;

