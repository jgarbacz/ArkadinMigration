create table rapid_rate_consolidations (
id_audit INTEGER,
id_sched INTEGER,
id_paramtable INTEGER,
current_id_sched INTEGER,
consolidation_date datetime
);

create index rr_consolidations_ndx on RAPID_RATE_CONSOLIDATIONS(id_audit, current_id_sched);

