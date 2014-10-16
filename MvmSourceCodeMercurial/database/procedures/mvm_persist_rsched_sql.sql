create procedure mvm_persist_rsched @my_id_pt INT, @v_id_sched INT OUT, 
@my_id_pricelist INT, @my_id_pi_template INT,
@v_start_dt DATETIME, @v_start_type INT, @v_begin_offset INT, 
@v_end_dt DATETIME, @v_end_type INT, @v_end_offset INT, 
@is_public INT, @my_id_sub INT, @v_id_csr INT = 129 as

declare @my_id_eff_date INT,
@my_start_dt DATE,
@my_start_type INT,
@my_begin_offset INT,
@my_end_dt DATE,
@my_end_type INT,
@my_end_offset INT,
@my_id_sched INT,
@has_tpl_map INT,
@l_id_audit INT,
@my_message VARCHAR(1000),
@my_utc_date datetime,
@curr_max_tt_date datetime


    set @my_start_type = @v_start_type;
    set @my_start_dt = @v_start_dt;
    set @my_begin_offset = @v_begin_offset;
    set @my_end_type = @v_end_type;
    set @my_end_dt = @v_end_dt;
    set @my_end_offset = @v_end_offset;
    set @my_id_sched = @v_id_sched;
    -- Cleanup relative dates. TBD: not handling type 2 (subscription relative)
    --set @my_start_dt = determine_absolute_dates(@my_start_dt, @my_start_type, @my_begin_offset, @my_id_acc, 1);
    --set @my_end_dt = determine_absolute_dates(@my_end_dt, @my_end_type, @my_end_offset, @my_id_acc, 0);
    set @my_start_type = 1;
    set @my_begin_offset = 0;
    set @my_end_type = 1;
    set @my_end_offset = 0;

   select @my_end_dt = case substring(convert(varchar,@my_end_dt, 120),12,12) when '00:00:00' then dateadd(second,-1,@my_end_dt) else @my_end_dt end;
   select @my_end_dt = case substring(convert(varchar,@my_end_dt, 120),1,10) when '2037-12-31' then dateadd(second,1,@my_end_dt) else @my_end_dt end;
   

    IF (@my_id_sched IS NULL) BEGIN
        
        --IF (@is_public = 0) THEN
          -- insert rate schedule create audit

        --END IF;
        insert into t_base_props (n_kind, n_name, n_desc, nm_name, nm_desc, b_approved, b_archive, n_display_name, nm_display_name
        ) values(130, 0, 0, NULL, NULL, 'N', 'N', 0, NULL)
        select @my_id_sched =@@identity
        set @v_id_sched = @my_id_sched;
        
        exec dbo.GetCurrentID 'id_audit', @l_id_audit OUT;
        select @my_utc_date = GETUTCDATE()
        set @my_message = 'MASS RATE: Adding schedule for pt: ' +  CAST(@my_id_pt AS VARCHAR(30)) + ' Rate Schedule Id: ' + CAST(@my_id_sched as VARCHAR(30))
        EXEC dbo.InsertAuditEvent @v_id_csr, 1400, 2, @my_id_sched, @my_utc_date, @l_id_audit, @my_message;
        
        insert into t_base_props (n_kind, n_name, n_desc, nm_name, nm_desc, b_approved, b_archive, n_display_name, nm_display_name
        ) values(160, 0, 0, NULL, NULL, 'N', 'N', 0, NULL);
        select @my_id_eff_date =@@identity

        insert into t_effectivedate (id_eff_date, n_begintype, dt_start, n_beginoffset, n_endtype, dt_end, n_endoffset)
        values(@my_id_eff_date, @my_start_type, @my_start_dt, @my_begin_offset, @my_end_type, @my_end_dt, @my_end_offset);
        --IF (@is_public = 1) THEN
        --    insert into t_rsched_pub (id_sched, id_pt, id_eff_date, id_pricelist, dt_mod, id_pi_template)
        --    values(@my_id_sched,@my_id_pt, @my_id_eff_date, @my_id_pricelist, getdate(), @my_id_pi_template);
        --ELSE
            insert into t_rsched (id_sched, id_pt, id_eff_date, id_pricelist, dt_mod, id_pi_template)
            values(@my_id_sched,@my_id_pt, @my_id_eff_date, @my_id_pricelist, getdate(), @my_id_pi_template);
        --END IF;
        select @has_tpl_map = count(*) from t_pl_map where id_sub = @my_id_sub and id_paramtable = @my_id_pt AND id_pricelist = @my_id_pricelist AND id_pi_template = @my_id_pi_template;
        IF (@has_tpl_map = 0) BEGIN
          insert into t_pl_map (dt_modified, id_paramtable, id_pi_type, id_pi_template, id_pi_instance,
                    id_pi_instance_parent, id_sub, id_acc, id_po, id_pricelist, b_canicb)
              select getdate(), @my_id_pt, id_pi_type, id_pi_template, id_pi_instance,
                  id_pi_instance_parent, @my_id_sub, NULL, a.id_po, @my_id_pricelist, 'N'
               from t_pl_map a, t_sub b
               where b.id_sub = @my_id_sub
               and b.id_po = a.id_po
               and a.id_sub IS NULL
               and a.id_acc IS NULL
               and a.id_paramtable = @my_id_pt
               and a.id_pi_template = @my_id_pi_template;
        END
    END
    ELSE
    BEGIN
        --IF (@is_public = 1) BEGIN
        --    select id_eff_date into @my_id_eff_date from t_rsched_pub where id_sched = @my_id_sched;
        --    END
        --ELSE
        --BEGIN
            select @my_id_eff_date = id_eff_date from t_rsched where id_sched = @my_id_sched;
        --END;
        IF (@is_public = 0) BEGIN
          -- insert rate schedule rules audit
          exec dbo.GetCurrentID 'id_audit', @l_id_audit OUT;
        select @my_utc_date = GETUTCDATE()
          set @my_message = 'MASS RATE: Changing schedule for pt: ' +  CAST(@my_id_pt AS VARCHAR(30)) + ' Rate Schedule Id: ' + CAST(@my_id_sched as VARCHAR(30));
          exec dbo.InsertAuditEvent @v_id_csr, 1402, 2, @my_id_sched, @my_utc_date, @l_id_audit, @my_message;
          -- support nulls for private scheds
          IF (@v_start_dt IS NULL AND (@v_start_type IS NULL OR @v_start_type = 4 OR @v_start_type = 1)) BEGIN
            set @my_start_dt = NULL;
            set @my_start_type = 4;
          END;
          IF (@v_end_dt IS NULL AND (@v_end_type IS NULL OR @v_end_type = 4 OR @v_end_type = 1)) BEGIN
            set @my_end_dt = NULL;
            set @my_end_type = 4;
          END;
	  select @curr_max_tt_date = isnull(max(change_tt_end),dbo.MTMinDate()) from rapid_rate_date_changes where id_sched = @my_id_sched;
	  insert into rapid_rate_date_changes (id_sched,id_eff_date, orig_dt_start, orig_begintype, orig_dt_end, orig_endtype, change_tt_start, change_tt_end)
                    select @my_id_sched, id_eff_date, dt_start, n_begintype, dt_end, n_endtype, @curr_max_tt_date, @my_utc_date
                    from t_effectivedate 
                    where id_eff_date = @my_id_eff_date 
			and (
				n_begintype != @my_start_type 
				or dt_start != @my_start_dt
			        or (dt_start IS NULL and @my_start_dt IS NOT NULL)	
			        or (dt_start IS NOT NULL and @my_start_dt IS NULL)	
				or n_beginoffset != @my_begin_offset
                                or n_endtype != @my_end_type 
				or dt_end != @my_end_dt 
			        or (dt_end IS NULL and @my_end_dt IS NOT NULL)	
			        or (dt_end IS NOT NULL and @my_end_dt IS NULL)	
				or n_endoffset != @my_end_offset);
          update t_effectivedate set n_begintype = @my_start_type, dt_start = @my_start_dt, n_beginoffset = @my_begin_offset, n_endtype = @my_end_type,
              dt_end = @my_end_dt, n_endoffset = @my_end_offset
              where id_eff_date = @my_id_eff_date and (n_begintype != @my_start_type or dt_start != @my_start_dt or n_beginoffset != @my_begin_offset
                                      or n_endtype != @my_end_type or dt_end != @my_end_dt or n_endoffset != @my_end_offset);
        END
        --ELSE
        --BEGIN
        --  -- do NOT support nulls for public scheds
        --  update t_effectivedate set n_begintype = @my_start_type, dt_start = @my_start_dt, n_beginoffset = @my_begin_offset, n_endtype = @my_end_type,
        --      dt_end = @my_end_dt, n_endoffset = @my_end_offset
        --      where id_eff_date = @my_id_eff_date and (n_begintype != @my_start_type or dt_start != @my_start_dt or n_beginoffset != @my_begin_offset
        --                              or n_endtype != @my_end_type or dt_end != @my_end_dt or n_endoffset != @my_end_offset);
        --END;
    END

go
