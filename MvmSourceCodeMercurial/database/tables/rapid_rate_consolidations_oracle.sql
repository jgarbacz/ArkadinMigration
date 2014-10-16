create table rapid_rate_consolidations (
id_audit NUMBER(38),
id_sched NUMBER(38),
id_paramtable NUMBER(38),
current_id_sched NUMBER(38),
consolidation_date datetime
);

create index rr_consolidations_ndx on RAPID_RATE_CONSOLIDATIONS(id_audit, current_id_sched);
