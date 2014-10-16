create table rapid_rate_date_changes (
id_sched INTEGER,
id_eff_date INTEGER,
orig_dt_start datetime,
orig_begintype INTEGER,
orig_dt_end datetime,
orig_endtype INTEGER,
final_change INTEGER,
change_tt_start datetime,
change_tt_end datetime
);

create index rr_dt_changes_ndx on RAPID_RATE_DATE_CHANGES(id_sched);

