create or replace procedure mvm_persist_rsched(my_id_pt NUMBER, v_id_sched IN OUT NUMBER, my_id_pricelist NUMBER, my_id_pi_template NUMBER,
v_start_dt DATE, v_start_type NUMBER, v_begin_offset NUMBER, v_end_dt DATE, v_end_type NUMBER, v_end_offset NUMBER, is_public NUMBER, my_id_sub NUMBER, v_id_csr IN NUMBER := 129) as
my_id_eff_date NUMBER;
curr_id_cycle_type NUMBER;
curr_day_of_month NUMBER;
my_start_dt DATE;
my_start_type NUMBER;
my_begin_offset NUMBER;
my_end_dt DATE;
my_end_type NUMBER;
my_end_offset NUMBER;
my_id_sched NUMBER;
has_tpl_map NUMBER;
l_id_audit NUMBER;
curr_max_tt_date DATE;

begin
    my_start_type := v_start_type;
    my_start_dt := v_start_dt;
    my_begin_offset := v_begin_offset;
    my_end_type := v_end_type;
    my_end_dt := v_end_dt;
    my_end_offset := v_end_offset;
    my_id_sched := v_id_sched;
    -- Cleanup relative dates. TBD: not handling type 2 (subscription relative)
    --my_start_dt := determine_absolute_dates(my_start_dt, my_start_type, my_begin_offset, my_id_acc, 1);
    --my_end_dt := determine_absolute_dates(my_end_dt, my_end_type, my_end_offset, my_id_acc, 0);
    my_start_type := 1;
    my_begin_offset := 0;
    my_end_type := 1;
    my_end_offset := 0;

    IF (my_id_sched IS NULL) THEN
        select SEQ_T_BASE_PROPS.nextval into my_id_sched from dual;
        v_id_sched := my_id_sched;
        IF (is_public = 0) THEN
          -- insert rate schedule create audit
          mvm_get_current_id('id_audit',1, l_id_audit);
          InsertAuditEvent (v_id_csr, 1400, 2, my_id_sched, getutcdate(), l_id_audit, 'MVM RATES: Adding schedule for pt: ' || my_id_pt || ' Rate Schedule Id: ' || my_id_sched);
        END IF;
        insert into t_base_props ( id_prop, n_kind, n_name, n_desc, nm_name, nm_desc, b_approved, b_archive, n_display_name, nm_display_name
        ) values( my_id_sched, 130, 0, 0, NULL, 'MVM RATES: Generated Rate Schedule', 'N', 'N', 0, NULL);
        select SEQ_T_BASE_PROPS.nextval into my_id_eff_date from dual;
        insert into t_base_props ( id_prop, n_kind, n_name, n_desc, nm_name, nm_desc, b_approved, b_archive, n_display_name, nm_display_name
        ) values( my_id_eff_date, 160, 0, 0, NULL, NULL, 'N', 'N', 0, NULL);
        IF (v_start_dt IS NULL AND (v_start_type IS NULL OR v_start_type = 4 OR v_start_type = 1)) THEN
             my_start_dt := NULL;
             my_start_type := 4;
        END IF;
        IF (v_end_dt IS NULL AND (v_end_type IS NULL OR v_end_type = 4 OR v_end_type = 1)) THEN
             my_end_dt := NULL;
             my_end_type := 4;
        END IF;

        insert into t_effectivedate (id_eff_date, n_begintype, dt_start, n_beginoffset, n_endtype, dt_end, n_endoffset)
        values(my_id_eff_date, my_start_type, my_start_dt, my_begin_offset, my_end_type, my_end_dt, my_end_offset);
        IF (is_public = 1) THEN
        NULL;
--            insert into t_rsched_pub (id_sched, id_pt, id_eff_date, id_pricelist, dt_mod, id_pi_template)
--            values(my_id_sched,my_id_pt, my_id_eff_date, my_id_pricelist, getutcdate(), my_id_pi_template);
        ELSE
            insert into t_rsched (id_sched, id_pt, id_eff_date, id_pricelist, dt_mod, id_pi_template)
            values(my_id_sched,my_id_pt, my_id_eff_date, my_id_pricelist, getutcdate(), my_id_pi_template);
        END IF;
        select count(*) into has_tpl_map from t_pl_map where id_sub = my_id_sub and id_paramtable = my_id_pt AND id_pricelist = my_id_pricelist AND id_pi_template = my_id_pi_template;
        IF (has_tpl_map = 0) THEN
          insert into t_pl_map (dt_modified, id_paramtable, id_pi_type, id_pi_template, id_pi_instance,
                    id_pi_instance_parent, id_sub, id_acc, id_po, id_pricelist, b_canicb)
              select getutcdate(), my_id_pt, id_pi_type, id_pi_template, id_pi_instance,
                  id_pi_instance_parent, my_id_sub, NULL, a.id_po, my_id_pricelist, 'Y'
               from t_pl_map a, t_sub b
               where b.id_sub = my_id_sub
               and b.id_po = a.id_po
               and nvl2(a.id_sub,NULL,0) = 0
               and nvl2(a.id_acc,NULL,0) = 0
               and a.id_paramtable = my_id_pt
               and a.id_pi_template = my_id_pi_template;
        END IF;
    ELSE
        IF (is_public = 1) THEN
        NULL;
--            select id_eff_date into my_id_eff_date from t_rsched_pub where id_sched = my_id_sched;
        ELSE
            select id_eff_date into my_id_eff_date from t_rsched where id_sched = my_id_sched;
        END IF;
        IF (is_public = 0) THEN
          -- insert rate schedule rules audit
          mvm_get_current_id('id_audit',1, l_id_audit);
          InsertAuditEvent (v_id_csr, 1402, 2, my_id_sched, getutcdate(), l_id_audit, 'MVM RATES: Changing schedule for pt: ' || my_id_pt || ' Rate Schedule Id: ' || my_id_sched);
          -- support nulls for private scheds
          IF (v_start_dt IS NULL AND (v_start_type IS NULL OR v_start_type = 4 OR v_start_type = 1)) THEN
            my_start_dt := NULL;
            my_start_type := 4;
          END IF;
          IF (v_end_dt IS NULL AND (v_end_type IS NULL OR v_end_type = 4 OR v_end_type = 1)) THEN
            my_end_dt := NULL;
            my_end_type := 4;
          END IF;
	select nvl(max(change_tt_end),mtmindate()) into curr_max_tt_date from rapid_rate_date_changes where id_sched = my_id_sched;
	  insert into rapid_rate_date_changes (id_sched,id_eff_date, orig_dt_start, orig_begintype, orig_dt_end, orig_endtype, change_tt_start, change_tt_end)
                    select my_id_sched, id_eff_date, dt_start, n_begintype, dt_end, n_endtype, 0, curr_max_tt_date, sysdate
                    from t_effectivedate 
                    where a.id_eff_date = my_id_eff_date and 
			(
				n_begintype != my_start_type 
				or dt_start != my_start_dt 
				or (dt_start IS NULL and my_start_dt IS NOT NULL) 
				or (dt_start IS NOT NULL and my_start_dt IS NULL)
		      		or n_beginoffset != my_begin_offset
                                or n_endtype != my_end_type 
				or dt_end != my_end_dt
			        or (dt_end IS NULL and my_end_dt IS NOT NULL) 
				or (dt_end IS NOT NULL and my_end_dt IS NULL)
				or n_endoffset != my_end_offset
			); 
	  
          update t_effectivedate set n_begintype = my_start_type, dt_start = my_start_dt, n_beginoffset = my_begin_offset, n_endtype = my_end_type,
              dt_end = my_end_dt, n_endoffset = my_end_offset
              where a.id_eff_date = my_id_eff_date and 
			(
				n_begintype != my_start_type 
				or dt_start != my_start_dt 
				or (dt_start IS NULL and my_start_dt IS NOT NULL) 
				or (dt_start IS NOT NULL and my_start_dt IS NULL)
		      		or n_beginoffset != my_begin_offset
                                or n_endtype != my_end_type 
				or dt_end != my_end_dt
			        or (dt_end IS NULL and my_end_dt IS NOT NULL) 
				or (dt_end IS NOT NULL and my_end_dt IS NULL)
				or n_endoffset != my_end_offset
			);
        ELSE
          -- do NOT support nulls for public scheds
          update t_effectivedate set n_begintype = my_start_type, dt_start = my_start_dt, n_beginoffset = my_begin_offset, n_endtype = my_end_type,
              dt_end = my_end_dt, n_endoffset = my_end_offset
              where a.id_eff_date = my_id_eff_date and 
			(
				n_begintype != my_start_type 
				or dt_start != my_start_dt 
				or (dt_start IS NULL and my_start_dt IS NOT NULL) 
				or (dt_start IS NOT NULL and my_start_dt IS NULL)
		      		or n_beginoffset != my_begin_offset
                                or n_endtype != my_end_type 
				or dt_end != my_end_dt
			        or (dt_end IS NULL and my_end_dt IS NOT NULL) 
				or (dt_end IS NOT NULL and my_end_dt IS NULL)
				or n_endoffset != my_end_offset
			);
        END IF;
    END IF;

end mvm_persist_rsched;

