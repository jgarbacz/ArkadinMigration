create table rapid_rate_date_changes (
id_sched NUMBER(38),
id_eff_date NUMBER(38),
orig_dt_start DATE,
orig_begintype INTEGER,
orig_dt_end DATE,
orig_endtype INTEGER,
final_change INTEGER,
change_date DATE
);

create index rr_dt_changes_ndx on RAPID_RATE_DATE_CHANGES(id_sched);

